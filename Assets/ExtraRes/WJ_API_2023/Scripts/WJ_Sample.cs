using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WjChallenge;

public enum CurrentStatus { WAITING, DIAGNOSIS, LEARNING }
public class WJ_Sample : MonoBehaviour
{
    //[SerializeField] WJ_Connector       wj_conn;
    WJ_Connector wj_conn => WJ_Connector.Instance;

    [SerializeField] CurrentStatus      currentStatus;
    public CurrentStatus                CurrentStatus => currentStatus;

    [Header("Panels")]
    [SerializeField] GameObject         panel_diag_chooseDiff;  //난이도 선택 패널
    [SerializeField] GameObject         panel_question;         //문제 패널(진단,학습)

    [SerializeField] Text   textDescription;        //문제 설명 텍스트
    [SerializeField] TEXDraw   textEquation;           //문제 텍스트(※TextDraw로 변경 필요)
    [SerializeField] Button[]           btAnsr = new Button[4]; //정답 버튼들
    TEXDraw[]                textAnsr;                  //정답 버튼들 텍스트(※TextDraw로 변경 필요)

    [Header("Status")]
    int     currentQuestionIndex;
    bool    isSolvingQuestion;
    float   questionSolveTime;

    [Header("For Debug")]
    [SerializeField] WJ_DisplayText     wj_displayText;         //텍스트 표시용(필수X)
    [SerializeField] Button             getLearningButton;      //문제 받아오기 버튼

    private void Awake()
    {
        textAnsr = new TEXDraw[btAnsr.Length];
        for (int i = 0; i < btAnsr.Length; ++i)

            textAnsr[i] = btAnsr[i].GetComponentInChildren<TEXDraw>();

        wj_displayText.SetState("대기중", "", "", "");
    }

    private void OnEnable()
    {
        Setup();
    }

    private void Setup()
    {
        switch (currentStatus)
        {
            case CurrentStatus.WAITING:
                panel_diag_chooseDiff.SetActive(true);
                break;
        }        

        if (wj_conn != null)
        {            
            wj_conn.onGetDiagnosis.AddListener(() => GetDiagnosis());
            wj_conn.onGetLearning.AddListener(() => GetLearning(0));
        }
        else Debug.LogError("Cannot find Connector");
    }

    private void Update()
    {
        if (isSolvingQuestion) questionSolveTime += Time.deltaTime;
    }

    /// <summary>
    /// 진단평가 문제 받아오기
    /// </summary>
    private void GetDiagnosis()
    {
        switch (wj_conn.cDiagnotics.data.prgsCd)
        {
            case "W":
                MakeQuestion(wj_conn.cDiagnotics.data.textCn, 
                            wj_conn.cDiagnotics.data.qstCn, 
                            wj_conn.cDiagnotics.data.qstCransr, 
                            wj_conn.cDiagnotics.data.qstWransr);
                wj_displayText.SetState("진단평가 중", "", "", "");
                break;
            case "E":
                Debug.Log("진단평가 끝! 학습 단계로 넘어갑니다.");
                wj_displayText.SetState("진단평가 완료", "", "", "");
                currentStatus = CurrentStatus.LEARNING;
                getLearningButton.interactable = true;
                break;
        }
    }

    /// <summary>
    ///  n 번째 학습 문제 받아오기
    /// </summary>
    private void GetLearning(int _index)
    {
        if (_index == 0) currentQuestionIndex = 0;

        MakeQuestion(wj_conn.cLearnSet.data.qsts[_index].textCn,
                    wj_conn.cLearnSet.data.qsts[_index].qstCn,
                    wj_conn.cLearnSet.data.qsts[_index].qstCransr,
                    wj_conn.cLearnSet.data.qsts[_index].qstWransr);
    }

    /// <summary>
    /// 받아온 데이터를 가지고 문제를 표시
    /// </summary>
    private void MakeQuestion(string textCn, string qstCn, string qstCransr, string qstWransr)
    {
        Debug.Log("MakeQuestion");
        DiagonosticManager.Instance.StartTimeBar();

        panel_diag_chooseDiff.SetActive(false);
        panel_question.SetActive(true);

        string      correctAnswer;
        string[]    wrongAnswers;

        textDescription.text = textCn;
        textEquation.text = qstCn;

        correctAnswer = qstCransr;
        wrongAnswers    = qstWransr.Split(',');

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
                textAnsr[i].text = correctAnswer;
                --q;
            }
            else
                textAnsr[i].text = wrongAnswers[q];
        }
        isSolvingQuestion = true;
    }

    int loop = 0;

    /// <summary>
    /// 답을 고르고 맞았는 지 체크
    /// </summary>
    public void SelectAnswer(int _idx = -1)
    {
        Debug.Log("SelectAnswer idx : " + _idx);
        DiagonosticManager.Instance.InitTimeBar();

        bool isCorrect;
        string ansrCwYn = "N";
        string ansr;

        switch (currentStatus)
        {
            case CurrentStatus.DIAGNOSIS:                
                if (_idx == -1) ansr = ""; // 답안 제출하지 못함 (공란?)                
                else ansr = textAnsr[_idx].text;               

                // 답안 평가
                isCorrect = ansr.CompareTo(wj_conn.cDiagnotics.data.qstCransr) == 0 ? true : false;
                ansrCwYn = isCorrect ? "Y" : "N";

                isSolvingQuestion = false;                

                // 커넥터 통해 문제 답안 결과 보내기
                wj_conn.Diagnosis_SelectAnswer(ansr, ansrCwYn, (int)(questionSolveTime * 1000));

                wj_displayText.SetState("진단평가 중", ansr, ansrCwYn, questionSolveTime + " 초");

                panel_question.SetActive(false);
                questionSolveTime = 0;
                break;

            case CurrentStatus.LEARNING:
                if (_idx == -1) ansr = ""; // 답안 제출하지 못함 (공란?)                
                else ansr = textAnsr[_idx].text;

                // 답안 평가
                Debug.Log("loop : " + loop + " || currentQuestionIndex : " + currentQuestionIndex);

                //if (wj_conn == null) Debug.Log("null 6");
                //if (wj_conn.cLearnSet == null) Debug.Log("null 5");
                //if (wj_conn.cLearnSet.data == null) Debug.Log("null 4");
                //if (wj_conn.cLearnSet.data.qsts == null) Debug.Log("null 3");
                //if (wj_conn.cLearnSet.data.qsts[currentQuestionIndex] == null) Debug.Log("null 2");
                //if (wj_conn.cLearnSet.data.qsts[currentQuestionIndex].qstCransr == null) Debug.Log("null 1");             

                isCorrect = ansr.CompareTo(wj_conn.cLearnSet.data.qsts[currentQuestionIndex].qstCransr) == 0 ? true : false;
                ansrCwYn = isCorrect ? "Y" : "N";

                isSolvingQuestion = false;
                currentQuestionIndex++;

                // 커넥터 통해 문제 답안 결과 보내기
                wj_conn.Learning_SelectAnswer(currentQuestionIndex, ansr, ansrCwYn, (int)(questionSolveTime * 1000));

                wj_displayText.SetState("문제풀이 중", ansr, ansrCwYn, questionSolveTime + " 초");
                
                if (currentQuestionIndex >= 8)
                {
                    //panel_question.SetActive(false);
                    //wj_displayText.SetState("문제풀이 완료", "", "", "");

                    loop++;
                    ButtonEvent_GetLearning();
                }
                else GetLearning(currentQuestionIndex);

                questionSolveTime = 0;
                break;
        }
    }

    public void DisplayCurrentState(string state, string myAnswer, string isCorrect, string svTime)
    {
        if (wj_displayText == null) return;

        wj_displayText.SetState(state, myAnswer, isCorrect, svTime);
    }

    #region Unity ButtonEvent
    public void ButtonEvent_ChooseDifficulty(int a)
    {
        currentStatus = CurrentStatus.DIAGNOSIS;
        wj_conn.FirstRun_Diagnosis(a);
    }
    public void ButtonEvent_GetLearning()
    {
        //Debug.Log("ButtonEvent_GetLearning");
        wj_conn.Learning_GetQuestion();
        wj_displayText.SetState("문제풀이 중", "-", "-", "-");
    }
    #endregion
}
