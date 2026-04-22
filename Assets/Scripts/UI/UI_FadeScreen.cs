using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UI_FadeScreen : MonoBehaviour
{
    private Image fade_image;
    public Coroutine fade_effect_co { get; private set; }


    private void Awake()
    {
        fade_image = GetComponent<Image>();
        fade_image.color = new Color(0, 0, 0, 1);
    }

    
    public void DoFadeIn(float duration = 1f) // black -> transparent
    {
        fade_image.color = new Color(0, 0, 0, 1);
        FadeEffect(0f, duration);
    }


    public void DoFadeOut(float duration = 1) // transparent -> black
    {
        fade_image.color = new Color(0, 0, 0, 0);
        FadeEffect(1f, duration);
    }


    private void FadeEffect(float target_alpha, float duration)
    {
        if (fade_effect_co != null)
            StopCoroutine(fade_effect_co);

        fade_effect_co = StartCoroutine(FadeEffectCo(target_alpha, duration));
    }


    private IEnumerator FadeEffectCo(float target_alpha, float duration)
    {
        float start_alpha = fade_image.color.a;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;

            Color color = fade_image.color;
            // Lerp gives almost the percentage between start and target by using the value "time / duration" gives -> fade_effect
            color.a = Mathf.Lerp(start_alpha, target_alpha, time / duration); 
            fade_image.color = color;

            yield return null; // waits for the next frame so time.deltaTime works correctly
        }

        fade_image.color = new Color(fade_image.color.r, fade_image.color.g, fade_image.color.b, target_alpha);
    }
}
