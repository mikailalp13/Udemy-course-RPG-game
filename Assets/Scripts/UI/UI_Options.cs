using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class UI_Options : MonoBehaviour
{
    private Player player;
    [SerializeField] private Toggle health_bar_toggle;

    [SerializeField] private AudioMixer audio_mixer;
    [SerializeField] private float mixer_multiplier = 25f;

    [Header("BGM Volume Settings")]
    [SerializeField] private Slider bgm_slider;
    [SerializeField] private string bgm_parameter;


    [Header("SFX Volume Settings")]
    [SerializeField] private Slider sfx_slider;
    [SerializeField] private string sfx_parameter;



    private void Start()
    {
        player = FindFirstObjectByType<Player>();

        
        health_bar_toggle.onValueChanged.AddListener(OnHealthBarToggleChanged);
    }


    public void BGMSliderValue(float value)
    {
        float new_slider_value = MathF.Log10(value) * mixer_multiplier;
        audio_mixer.SetFloat(bgm_parameter, new_slider_value);
    }


    public void SFXSliderValue(float value)
    {
        float new_slider_value = MathF.Log10(value) * mixer_multiplier;
        audio_mixer.SetFloat(sfx_parameter, new_slider_value);
    }


    private void OnHealthBarToggleChanged(bool is_on)
    {
        player.health.EnableHealthBar(is_on);
    }


    public void GoMainMenuButton() => GameManager.instance.ChangeScene("MainMenu", RespawnType.NoneSpecific);


    private void OnEnable()
    {
        sfx_slider.value = PlayerPrefs.GetFloat(sfx_parameter, 0.6f);
        bgm_slider.value = PlayerPrefs.GetFloat(bgm_parameter, 0.6f);
    }


    private void OnDisable()
    {
        PlayerPrefs.SetFloat(sfx_parameter, sfx_slider.value);
        PlayerPrefs.SetFloat(bgm_parameter, bgm_slider.value);
    }


    public void LoadUpVolume()
    {
        sfx_slider.value = PlayerPrefs.GetFloat(sfx_parameter, 0.6f);
        bgm_slider.value = PlayerPrefs.GetFloat(bgm_parameter, 0.6f);
    }
}
