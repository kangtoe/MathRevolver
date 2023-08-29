using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

//public enum CurrentStatus 
//{ 
//    WAITING, 
//    DIAGNOSIS, 
//    LEARNING 
//}

public enum DiagonosticStatus
{
    Undefined = 0,
    ChoosingDiff,
    OnSolving, // 문제 풀이 중
    WaitNextQuestion, // 연출 중 :다음 문제 대기
    DiagonosticFinished
}

public class WJ_Sample_Diagonostic : MonoBehaviour
{
    [SerializeField]
    DiagonosticStatus status;
    
    [Header("Panels")]
    [SerializeField] GameObject panel_diag_chooseDiff;  //난이도 선택 패널
    [SerializeField] GameObject panel_question;         //문제 패널(진단,학습)
    [SerializeField] GameObject panel_finish;         //문제 패널(진단,학습)

    [SerializeField] Text       textDescription;        //문제 설명 텍스트
    [SerializeField] TEXDraw    textEquation;           //문제 텍스트(※TextDraw로 변경 필요)
    [SerializeField] Button[]   btAnsr = new Button[4]; //정답 버튼들
    TEXDraw[]                   textAnsr;                  //정답 버튼들 텍스트(※TextDraw로 변경 필요)

    [Header("Status")]
    int     currentQuestionIndex;
    bool    isSolvingQuestion;
    float   questionSolveTime;    

    [Header("TEXDraw 폰트 설정 문자열")]
    [SerializeField]
    string texDrawfontText = @"\galmuri";

    [Header("진단 평가 점수")]
    [SerializeField]
    int score;
    [SerializeField]
    int scorePreCurrect = 5; // 문제 정답 시 점수

    [Header("처음으로 문제 정보를 알아왔을 때 한번만 실행")]
    public UnityEvent onGetQuestionFirst;
    bool eventInvoked = false;

    WJ_Connector wj_conn => WJ_Connector.Instance;

    //[Header("For Debug")]
    //[SerializeField] WJ_DisplayText wj_displayText;         //텍스트 표시용(필수X)
    //[SerializeField] Button getLearningButton;      //문제 받아오기 버튼

    //[SerializeField] WJ_Connector       wj_conn;
    //CurrentStatus               currentStatus;
    //public CurrentStatus      CurrentStatus => currentStatus;

    private void Awake()
    {
        textAnsr = new TEXDraw[btAnsr.Length];
        for (int i = 0; i < btAnsr.Length; ++i)
        {
            textAnsr[i] = btAnsr[i].GetComponentInChildren<TEXDraw>();
        }        
    }

    private void OnEnable()
    {
        OnStartDiagonostic();
    }

    private void Update()
    {
        if (isSolvingQuestion) questionSolveTime += Time.deltaTime;
    }

    void InvokeOnGetQuestion()
    {
        if (eventInvoked) return;
        eventInvoked = true;

        //Debug.Log("InvokeOnGetQuestion");
        onGetQuestionFirst.Invoke();
    }

    // 진단 평가 시작 시
    void OnStartDiagonostic()
    {        
        if (wj_conn is null) Debug.LogError("Cannot find Connector");
        wj_conn.onGetDiagnosis.AddListener(delegate {
            GetDiagnosis();
            InvokeOnGetQuestion();              
        });        

        //wj_conn.onGetDiagnosis.RemoveListener()

        panel_diag_chooseDiff.SetActive(true);
        status = DiagonosticStatus.ChoosingDiff;
    }

    // 진단 평가 완료 시
    void OnEndDiagonostic()
    {
        panel_question.SetActive(false);
        panel_finish.SetActive(true);

        SaveManager.DiagonosticCompleted = true;
        SaveManager.DiagonosticScore = score;

        status = DiagonosticStatus.DiagonosticFinished;
    }

    // 난이도 버튼 선택 시 : 버튼 이벤트로 호출
    public void OnChooseDifficulty(int a)
    {        
        wj_conn.FirstRun_Diagnosis(a);
        status = DiagonosticStatus.OnSolving;
    }

    // 정답 제출 시
    void OnCurrectAnswer()
    {
        UIManager_Diagonostic.Instance.CreateCurrectUI();

        // 정답 시 점수 추가
        score += scorePreCurrect;
    }

    // 오답 제출 시
    void OnWrongAnswer()
    {
        UIManager_Diagonostic.Instance.CreateIncurrectUI();
    }

    // 진단평가 문제 받아오기
    void GetDiagnosis()
    {
        WjChallenge.Diagnotics_Data data = wj_conn.cDiagnotics.data;
        switch (data.prgsCd)
        {
            case "W":                
                UpdateQuestionUI(data.textCn, data.qstCn, data.qstCransr, data.qstWransr);
                Debug.Log("진단평가 중");
                
                break;
            case "E":
                Debug.Log("진단평가 완료");                
                OnEndDiagonostic();
                break;
        }
    }

    // 받아온 데이터를 가지고 문제를 표시
    private void UpdateQuestionUI(string textCn, string qstCn, string qstCransr, string qstWransr)
    {
        Debug.Log("MakeQuestion");
        DiagonosticManager.Instance.StartTimeBar();

        panel_diag_chooseDiff.SetActive(false);
        panel_question.SetActive(true);

        string      correctAnswer;
        string[]    wrongAnswers;

        textDescription.text = textCn;
        textEquation.text = texDrawfontText + qstCn;

        correctAnswer = qstCransr;
        wrongAnswers = qstWransr.Split(',');

        int ansrCount = Mathf.Clamp(wrongAnswers.Length, 0, 3) + 1;

        for(int i=0; i<btAnsr.Length; i++)
        {
            if (i < ansrCount)
                btAnsr[i].gameObject.SetActive(true);
            else
                btAnsr[i].gameObject.SetActive(false);
        }

        int ansrIndex = Random.Range(0, ansrCount);

        for(int i = 0, q = 0; i < ansrCount; ++i, ++q)
        {
            if (i == ansrIndex)
            {
                textAnsr[i].text = texDrawfontText + correctAnswer;
                --q;
            }
            else
                textAnsr[i].text = texDrawfontText + wrongAnswers[q];
        }

        isSolvingQuestion = true;
        questionSolveTime = 0;
    }

    // 답을 고르고 맞았는 지 체크
    public void SelectAnswer(int _idx = -1)
    {        
        DiagonosticManager.Instance.InitTimeBar();

        bool isCorrect;
        string ansrCwYn;
        string myAnsr;
        string currectAnsr = wj_conn.cDiagnotics.data.qstCransr;

        if (_idx == -1) myAnsr = ""; // 답안 제출하지 못함 (공란?)                        
        else
        {
            myAnsr = textAnsr[_idx].text;
            myAnsr = myAnsr.Replace(texDrawfontText, ""); // 폰트 문자열 제거
        }

        // 답안 평가
        isCorrect = myAnsr.CompareTo(currectAnsr) == 0 ? true : false;
        ansrCwYn = isCorrect ? "Y" : "N";

        // 디버깅
        Debug.Log("isCorrect : " + isCorrect);
        Debug.Log("SelectAnswer idx : " + _idx);
        Debug.Log("myAnsr : " + myAnsr);
        Debug.Log("currectAnsr : " + currectAnsr);
        //wj_displayText.SetState("진단평가 중", myAnsr, ansrCwYn, questionSolveTime + " 초");
        
        // 커넥터 통해 문제 답안 결과 보내기
        wj_conn.Diagnosis_SelectAnswer(myAnsr, ansrCwYn, (int)(questionSolveTime * 1000));

        isSolvingQuestion = false;
        //panel_question.SetActive(false);        

        // 정답/오답 시 처리        
        if (isCorrect) OnCurrectAnswer();
        else OnWrongAnswer();
    }

    #region 만료됨

    /// <summary>
    ///  n 번째 학습 문제 받아오기
    /// </summary>
    private void GetLearning(int _index)
    {
        //if (_index == 0) currentQuestionIndex = 0;

        //WjChallenge.Learning_Question qst = wj_conn.cLearnSet.data.qsts[_index];
        //MakeQuestion(qst.textCn, qst.qstCn, qst.qstCransr, qst.qstWransr);
    }

    public void DisplayCurrentState(string state, string myAnswer, string isCorrect, string svTime)
    {
        //if (wj_displayText == null) return;

        //wj_displayText.SetState(state, myAnswer, isCorrect, svTime);
    }

    public void EvaluateDiagonostic()
    {
        // 진단 평과 결과 종합하여 합산 (풀이 시간 등)
        //Debug.Log("EvaluateDiagonostic");
    }

    #endregion
}
