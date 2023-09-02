using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using WjChallenge;

public enum MathpidStatus
{
    Undefined = 0,    
    OnStartAnimation,
    OnSolving, // 문제 풀이 중
    OnSolveAnimation, // 문제 풀이 직후 연출 중
    Finished
}

// 1. 시작 시 : 문제 알아오기 요청
// 2. 문제를 알아온 직후 : 문제를 UI에 적용

// 3. 판넬이 활성화 되었을 때 : 시작 애니메이션 재생
// 4. 문제 답 제출 시 : 정답/오답 애니메이션 재생
// 5. 정답/오답 애니메이션 종료 시 : 창 닫기, 다음 문제 알아와 UI 적용

// Play 씬에서, mathpid 발판을 밟았을 때 학습 문제 표시 및 체점
public class WJ_Sample_Play : MonoBehaviour
{
    public static WJ_Sample_Play Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<WJ_Sample_Play>();
            }
            return instance;
        }
    }
    private static WJ_Sample_Play instance;

    [SerializeField]
    MathpidStatus status;

    [Header("Panels")]
    [SerializeField] GameObject panel_question;         //문제 패널(진단,학습)

    [SerializeField] Text textDescription;        //문제 설명 텍스트
    [SerializeField] TEXDraw textEquation;           //문제 텍스트(※TextDraw로 변경 필요)
    [SerializeField] Button[] btAnsr = new Button[4]; //정답 버튼들
    TEXDraw[] textAnsr;                  //정답 버튼들 텍스트(※TextDraw로 변경 필요)
   
    [Header("Status")]
    [SerializeField]
    int currentQuestionIndex;
    [SerializeField]
    int loop = 0; // 문제 묶음(8개) 받아 온 횟수    
    float questionSolveTime;
    bool isCorrect;

    [Header("문제 별 풀이 제한시간")]
    [SerializeField]
    float solveTime = 10;

    [Header("TEXDraw 폰트 설정 문자열")]
    [SerializeField]
    string texDrawfontText = @"\cmb";

    [Header("시작 연출")]
    public UnityEvent onStart;

    [Header("Debug")]
    [SerializeField]
    bool dataSetting = false; // 데이터 갱신 중?    
    [SerializeField]
    float timeScaleOnActivePanel = 0.1f; // 문제 풀이 중 시간 스케일

    [Header("시간 게이지")]
    [SerializeField]
    TimeBar timeBar;

    [Header("연출 애니메이터")]
    [SerializeField]
    Animator anim;
    [SerializeField]
    CorrectCreator correctCreator;

    WJ_Connector wj_conn => WJ_Connector.Instance;
    Learning_Question qst;

    #region 유니티 콜백

    private void Awake()
    {
        // 버튼의 TEXDraw 컴포넌트 알아오기
        textAnsr = new TEXDraw[btAnsr.Length];
        for (int i = 0; i < btAnsr.Length; ++i)
        {
            textAnsr[i] = btAnsr[i].GetComponentInChildren<TEXDraw>();
        }
    }

    private void Start()
    {
        // 문제 정보를 새롭게 받아올 때
        wj_conn.onGetLearning.AddListener(delegate
        {
            dataSetting = false;
        });

        ActivePanel(false);
        SetNewQuestionData();
    }

    private void Update()
    {
        if (status == MathpidStatus.OnSolving) questionSolveTime += Time.deltaTime;
    }

    #endregion

    // 새로운 문제들 받아오기 : 최초 1번만 (문제 묶음(8문항) 모두 풀었을 때 -> 문제 제출 시 자동으로 다음 문제들 받아옴)
    public void SetNewQuestionData()
    {
        if (wj_conn == null) Debug.LogError("Cannot find Connector");

        // 문제 정보를 새롭게 받아옴
        wj_conn.Learning_GetQuestion();
        dataSetting = true;
        
    }

    // 문제 정보 설정, 문제 정보대로 UI 표기
    void SetQst(int _index)
    {
        // 받아온 데이터를 가지고 문제를 표시
        void MakeQuestion(string textCn, string qstCn, string qstCransr, string qstWransr)
        {
            //Debug.Log("MakeQuestion");
            timeBar.StartTimeBar(solveTime);   

            string correctAnswer;
            string[] wrongAnswers;

            textDescription.text = textCn;
            textEquation.text = texDrawfontText + qstCn;

            correctAnswer = qstCransr;
            wrongAnswers = qstWransr.Split(',');

            int ansrCount = Mathf.Clamp(wrongAnswers.Length, 0, 3) + 1;

            for (int i = 0; i < btAnsr.Length; i++)
            {
                if (i < ansrCount)
                    btAnsr[i].gameObject.SetActive(true);
                else
                    btAnsr[i].gameObject.SetActive(false);
            }

            int ansrIndex = Random.Range(0, ansrCount);

            for (int i = 0, q = 0; i < ansrCount; ++i, ++q)
            {
                if (i == ansrIndex)
                {
                    textAnsr[i].text = texDrawfontText + correctAnswer;
                    --q;
                }
                else
                {
                    textAnsr[i].text = texDrawfontText + wrongAnswers[q];
                }
            }
            status = MathpidStatus.OnSolving;
            questionSolveTime = 0;
        }

        if (dataSetting)
        {
            Debug.Log("문제 표기 실패 : 데이터 받아오는 중");
            return;
        }

        Debug.Log("SetQst : " + _index);

        // index에 해당하는 문제 ui에 표기
        qst = wj_conn.cLearnSet.data.qsts[_index];        
        MakeQuestion(qst.textCn, qst.qstCn, qst.qstCransr, qst.qstWransr);

        // 연출
        anim.SetTrigger("appear");
    }
    
    // 답을 고르고 맞았는 지 체크 : 버튼 이벤트로 호출
    public void SelectAnswer(int _idx = -1)
    {                        
        timeBar.StopTimeBar();

        //bool isCorrect;
        string ansrCwYn;
        string myAnsr;
        string currectAnsr = qst.qstCransr;

        if (_idx == -1) myAnsr = ""; // 답안 제출하지 못함 (공란?)                        
        else
        {
            myAnsr = textAnsr[_idx].text;
            myAnsr = myAnsr.Replace(texDrawfontText, ""); // 폰트 문자열 제거
        }

        // 답안 평가
        isCorrect = myAnsr.CompareTo(currectAnsr) == 0 ? true : false;
        ansrCwYn = isCorrect ? "Y" : "N";

        // 커넥터 통해 문제 답안 결과 보내기
        wj_conn.Learning_SelectAnswer(currentQuestionIndex, myAnsr, ansrCwYn, (int)(questionSolveTime * 1000));

        // 현재 상태 : 애니메이션 연출 상태
        status = MathpidStatus.OnSolveAnimation;                            
        
        currentQuestionIndex++;
        if (currentQuestionIndex >= 8)
        {
            // 문제 묶음 (8문항) 모두 풀었을 때
            SetNewQuestionData();
            currentQuestionIndex = 0;
            loop++;
        }

        // 정답/오답 시 처리        
        if (isCorrect)
        {
            // 정답 처리
            SoundManager.Instance.PlaySound("correct");
            correctCreator.CreateCorrectUI();            

            // 연출
            switch (_idx)
            {
                case 0:
                    anim.SetTrigger("click0");
                    break;
                case 1:
                    anim.SetTrigger("click1");
                    break;
                case 2:
                    anim.SetTrigger("click2");
                    break;
                case 3:
                    anim.SetTrigger("click3");
                    break;
                default:
                    Debug.Log("_idx error");
                    break;
            }
        }
        else
        {
            // 오답 처리
            SoundManager.Instance.PlaySound("incorrect");
            correctCreator.CreateIncorrectUI();

            anim.SetTrigger("disappear");
        }

        // 디버그
        {
            Debug.Log("loop : " + loop);
            Debug.Log("currentQuestionIndex : " + currentQuestionIndex);
            Debug.Log("myAnsr idx : " + _idx);
            
            Debug.Log("myAnsr : " + myAnsr);
            Debug.Log("currectAnsr : " + currectAnsr);
            
            //if (wj_conn == null) Debug.Log("null 6");
            if (wj_conn.cLearnSet == null) Debug.Log("null 5");
            if (wj_conn.cLearnSet.data == null) Debug.Log("null 4");
            //if (wj_conn.cLearnSet.data.qsts == null) Debug.Log("null 3");
            //if (wj_conn.cLearnSet.data.qsts[currentQuestionIndex] == null) Debug.Log("null 2");
            //if (wj_conn.cLearnSet.data.qsts[currentQuestionIndex].qstCransr == null) Debug.Log("null 1");             
        }
    }

    // 모든 애니메이션 연출 종료
    public void OnEndAnim()
    {
        ActivePanel(false);        
        if (isCorrect) UIManager_Play.Instance.ActiveUpgradeUI(true);
    }

    public void ActivePanel(bool active)
    {
        //Debug.Log("ActivePanel : " + active);

        if (active == true)
        {
            TimeManager.Instance.SetScale(timeScaleOnActivePanel);
            panel_question.SetActive(true);

            // 시작 애니메이션 연출
            onStart.Invoke();

            // 문제 표기
            SetQst(currentQuestionIndex);
            //currentQuestionIndex++;

            // 정답 여부 초기화
            isCorrect = false;
        }
        else if (active == false)
        {
            TimeManager.Instance.SetScale(1);
            panel_question.SetActive(false);            
        }
    }

    public void SetTimeLimit(float _time)
    {
        solveTime = _time;
    }
}
