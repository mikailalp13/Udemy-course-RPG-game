using UnityEngine;
using UnityEngine.Rendering.Universal; 

public class LightFlicker : MonoBehaviour
{
    private Light2D candle_light;
    public float min_value = 4.8f;
    public float max_value = 5.2f;
    public float speed = 0.1f;


    void Start()
    {
        candle_light = GetComponent<Light2D>();
    }


    void Update()
    {
        float random_flicker = Random.Range(min_value, max_value);
        candle_light.intensity = Mathf.Lerp(candle_light.intensity, random_flicker, speed);
    }
}