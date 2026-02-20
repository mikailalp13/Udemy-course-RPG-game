using UnityEngine;
using System.Collections;
public class Player_VFX : Entity_VFX
{
    [Header("Image Echo VFX")]
    [Range(0.01f, 0.2f)]
    [SerializeField] private float image_echo_interval = 0.05f;
    [SerializeField] private GameObject image_echo_prefab;
    private Coroutine image_echo_co;


    public void DoImageEchoEffect(float duration)
    {
        if (image_echo_co != null)
            StopCoroutine(image_echo_co);
        image_echo_co = StartCoroutine(ImageEchoEffectCo(duration));
    }

    private IEnumerator ImageEchoEffectCo(float duration)
    {
        float time_tracker = 0;

        while (time_tracker < duration)
        {
            CreateImageEcho();

            yield return new WaitForSeconds(image_echo_interval);
            time_tracker += image_echo_interval;
        }
    }

    private void CreateImageEcho()
    {
        GameObject image_echo = Instantiate(image_echo_prefab, transform.position, transform.rotation);
        image_echo.GetComponentInChildren<SpriteRenderer>().sprite = sr.sprite;
    }
}
