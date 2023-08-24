using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

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

    WJ_Connector wj_conn => WJ_Connector.Instance;       

    [Header("Panels")]    
    [SerializeField] GameObject panel_question;         //문제 패널(진단,학습)

    [SerializeField] Text textDescription;        //문제 설명 텍스트
    [SerializeField] TEXDraw textEquation;           //문제 텍스트(※TextDraw로 변경 필요)
    [SerializeField] Button[] btAnsr = new Button[4]; //정답 버튼들
    TEXDraw[] textAnsr;                  //정답 버튼들 텍스트(※TextDraw로 변경 필요)

    [Header("Status")]
    int currentQuestionIndex;
    bool isSolvingQuestion;
    float questionSolveTime;

    [Header("For Debug")]
    [SerializeField] bool dataSetting = false; // 데이터 갱신 중?
    [SerializeField] WJ_DisplayText wj_displayText;         //텍스트 표시용(필수X)    

    #region 유니티 콜백

    private void Awake()
    {
        // 버튼의 TEXDraw 컴포넌트 알아오기
        textAnsr = new TEXDraw[btAnsr.Length];
        for (int i = 0; i < btAnsr.Length; ++i)
            textAnsr[i] = btAnsr[i].GetComponentInChildren<TEXDraw>();

        wj_displayText.SetState("대기중", "", "", "");

        //panel_question.SetActive(false);
        InactivePanel();
    }

    private void Update()
    {
        if (isSolvingQuestion) questionSolveTime += Time.deltaTime;
    }

    #endregion

    // 문제 풀이 시작
    public void StartQuestion()
    {
        ActivePanel();
        //panel_question.SetActive(true);

        // 문제 표기
        if (currentQuestionIndex % 8 == 0)
        {
            loop++;
            DoNewQuestions();
        }
        else
        {
            DoNextQuestions();
        }
    }

    /// <summary>
    ///  n 번째 학습 문제 받아와, 문제 표기
    /// </summary>
    private void GetLearning(int _index)
    {
        //Debug.Log("GetLearning");
        //if (_index == 0) currentQuestionIndex = 0;

        dataSetting = false;
        WjChallenge.Learning_Question qst = wj_conn.cLearnSet.data.qsts[_index];
        MakeQuestion(qst.textCn, qst.qstCn, qst.qstCransr, qst.qstWransr);                   
    }

    /// <summary>
    /// 받아온 데이터를 가지고 문제를 표시
    /// </summary>
    void MakeQuestion(string textCn, string qstCn, string qstCransr, string qstWransr)
    {
        Debug.Log("MakeQuestion");
        StartTimeBar();

        //panel_question.SetActive(true);
        ActivePanel();

        string correctAnswer;
        string[] wrongAnswers;

        textDescription.text = textCn;
        textEquation.text = qstCn;

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
                textAnsr[i].text = correctAnswer;
                --q;
            }
            else
            {
                if (textAnsr == null) Debug.Log("null 1");
                if (textAnsr[i] == null) Debug.Log("null 2");
                if (textAnsr[i].text == null) Debug.Log("null 3");
                if (wrongAnswers == null) Debug.Log("null 4");
                if (wrongAnswers[q] == null) Debug.Log("null 5");

                textAnsr[i].text = wrongAnswers[q]; 
            }            
        }
        isSolvingQuestion = true;
    }

    int loop = 0;

    /// <summary>
    /// 답을 고르고 맞았는 지 체크
    /// </summary>
    public void SelectAnswer(int _idx = -1)
    {
        if (dataSetting) Debug.Log("데이터 받아오는 중");

        Debug.Log("SelectAnswer idx : " + _idx);
        InitTimeBar();

        bool isCorrect;
        string ansrCwYn;
        string ansr;

        if (_idx == -1) ansr = ""; // 답안 제출하지 못함 (공란?)                
        else ansr = textAnsr[_idx].text;

        // 답안 평가
        Debug.Log("loop : " + loop + " || currentQuestionIndex : " + currentQuestionIndex);

        //if (wj_conn == null) Debug.Log("null 6");
        if (wj_conn.cLearnSet == null) Debug.Log("null 5");
        if (wj_conn.cLearnSet.data == null) Debug.Log("null 4");
        //if (wj_conn.cLearnSet.data.qsts == null) Debug.Log("null 3");
        //if (wj_conn.cLearnSet.data.qsts[currentQuestionIndex] == null) Debug.Log("null 2");
        //if (wj_conn.cLearnSet.data.qsts[currentQuestionIndex].qstCransr == null) Debug.Log("null 1");             

        isCorrect = ansr.CompareTo(wj_conn.cLearnSet.data.qsts[currentQuestionIndex].qstCransr) == 0 ? true : false;
        ansrCwYn = isCorrect ? "Y" : "N";
        
        // 커넥터 통해 문제 답안 결과 보내기
        wj_conn.Learning_SelectAnswer(currentQuestionIndex, ansr, ansrCwYn, (int)(questionSolveTime * 1000));

        wj_displayText.SetState("문제풀이 중", ansr, ansrCwYn, questionSolveTime + " 초");
        isSolvingQuestion = false;
        questionSolveTime = 0;
        
        currentQuestionIndex++;
        currentQuestionIndex %= 8;

        //panel_question.SetActive(false);
        InactivePanel();        
    }

    // 새로운 문제들 받아와 표기
    public void DoNewQuestions()
    {
        if (wj_conn == null) Debug.LogError("Cannot find Connector");

        wj_displayText.SetState("문제풀이 중", "-", "-", "-");

        dataSetting = true;
        // 문제 정보를 새롭게 받아올 때, 0번 문제를 UI에 표시하도록 이벤트 등록
        wj_conn.onGetLearning.AddListener(() => GetLearning(0));
        // 문제 정보를 새롭게 받아옴
        wj_conn.Learning_GetQuestion();        
    }

    // 다음 문제를 표기
    public void DoNextQuestions()
    {
        GetLearning(currentQuestionIndex);
    }

    // 문제 풀이 시간 초과
    void OnOverTime()
    {
        SelectAnswer(-1);
    }

    #region 시간 게이지

    [Header("문제당 풀이시간")]
    [SerializeField]
    float timeLimit = 10;

    [SerializeField]
    Image timeGage;

    [SerializeField]
    //public UnityEvent onTimeEnd;

    public void SetTimeLimit(float time)
    {
        timeLimit = time;
    }

    public void StartTimeBar()
    {
        StopAllCoroutines();
        StartCoroutine(TimeBarCr());
    }

    public void InitTimeBar()
    {
        StopAllCoroutines();
        timeGage.fillAmount = 1;
    }

    IEnumerator TimeBarCr()
    {
        float leftTime = timeLimit;
        while (true)
        {
            leftTime -= Time.unscaledDeltaTime;
            float ratio = Mathf.Clamp01(leftTime / timeLimit);
            timeGage.fillAmount = ratio;
            yield return null;
            if (ratio == 0) break;
        }

        Debug.Log("time end");

        // 문제 풀이 시간 초과
        //onTimeEnd.Invoke();
        OnOverTime();
    }

    #endregion

    #region 판넬 제어

    float timeScaleOnActivePanel = 0.1f;

    void ActivePanel()
    {
        TimeManager.Instance.SetScale(timeScaleOnActivePanel);
        panel_question.SetActive(true);
    }

    void InactivePanel()
    {
        TimeManager.Instance.SetScale(1);
        panel_question.SetActive(false);
    }

    #endregion


}
