using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField]
    float fadeDelay = 1;

    [SerializeField]
    float fadeDuration = 1;

    [SerializeField]
    CanvasGroup group;

    private void OnEnable()
    {        
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        group.alpha = 0;

        yield return new WaitForSeconds(fadeDelay);

        float t = 0;
        while (true)
        {
            group.alpha = t;
            t += Time.deltaTime / fadeDuration;

            yield return null;

            if (t == 0) break;
        }

        group.alpha = 1;
    }
}
