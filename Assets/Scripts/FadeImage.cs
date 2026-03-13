using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeImage : MonoBehaviour
{
    public Image image;
    public float fadeDuration = 30.0f;
    public float blinkSpeed = 30.0f;

    void Awake()
    {
        image = GetComponent<Image>();
    }
    public void StartEffect()
    {
        StartCoroutine(FadeThenBlink());
    }

    IEnumerator FadeThenBlink()
    {
        // Fade In
        float time = 0;
        Color c = image.color;
        c.a = 0.3f;
        image.color = c;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            c.a = Mathf.Lerp(0.3f, 0.5f, time / fadeDuration);
            image.color = c;
            yield return null;
        }

        // Blink
        StartCoroutine(Blink());
    }

    IEnumerator Blink()
    {
        while (true)
        {
            Color c = image.color;

            // descendre opacité
            while (c.a > 0.3f)
            {
                c.a -= Time.deltaTime * blinkSpeed;
                image.color = c;
                yield return null;
            }

            // monter opacité
            while (c.a < 0.5f)
            {
                c.a += Time.deltaTime * blinkSpeed;
                image.color = c;
                yield return null;
            }
        }
    }
}
