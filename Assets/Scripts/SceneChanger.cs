using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public static SceneChanger Instance
    {
        get
        {
            // 만약 싱글톤 변수에 아직 오브젝트가 할당되지 않았다면
            if (instance == null)
            {
                // 씬에서 GameManager 오브젝트를 찾아 할당
                instance = FindObjectOfType<SceneChanger>();
            }

            // 싱글톤 오브젝트를 반환
            return instance;
        }
    }
    private static SceneChanger instance; // 싱글톤이 할당될 static 변수

    [Header("페이드 효과")]
    [SerializeField]
    float fadeSpeed = 1;    
    [SerializeField]
    Image fadeImage;
    // scene 전환 도중 다른 전환 방지
    bool isChanging = false;

    // Start is called before the first frame update
    void Start()
    {
        // 타이틀 scene에서는 진입효과 넣지 않음
        if (SceneManager.GetActiveScene().name != "Title")
            StartCoroutine(SceneIn());
    }

    float SetFadeAlpha(float a)
    {
        a = Mathf.Clamp01(a);
        Color color = fadeImage.color;
        color.a = a;
        fadeImage.color = color;
        return a;
    }

    public void SceneChange(string seceneName)
    {
        //Debug.Log("SceneChange : " + seceneName);
        //StopAllCoroutines();
        StartCoroutine( SceneChangeCr(seceneName));
    }


    // scene 에서 나갈때 호출, 인수는 전환 대상 scene
    IEnumerator SceneChangeCr(string seceneName)
    {
        //Debug.Log("SceneChangeCr : " + seceneName);

        Time.timeScale = 1f; // GamePause가 먼저 호출된 경우 대비

        if (isChanging) yield break;
        isChanging = true;

        //Debug.Log("SceneChangeCr2 : " + seceneName);

        float f = 0;
        SetFadeAlpha(f);        

        while (true)
        {
            f += Time.deltaTime * fadeSpeed;
            f = SetFadeAlpha(f);
            
            yield return new WaitForEndOfFrame();
            if (f == 1) break;
        }        

        SceneManager.LoadScene(seceneName);
        isChanging = false;
    }

    // scene에 새롭게 진입했을 떄 호출
    IEnumerator SceneIn()
    {
        Debug.Log("SceneIn");

        if (isChanging) yield break;
        isChanging = true;       

        float f = 1;
        SetFadeAlpha(f);        

        while (true)
        {
            f -= Time.deltaTime * fadeSpeed;            
            f = SetFadeAlpha(f);            

            yield return new WaitForEndOfFrame();
            if (f == 0) break;
        }

        isChanging = false;        
    }
}
