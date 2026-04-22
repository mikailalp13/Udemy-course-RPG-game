using UnityEngine;
using UnityEngine.UI;

public class UI_Options : MonoBehaviour
{
    private Player player;
    [SerializeField] private Toggle health_bar_toggle;


    private void Start()
    {
        player = FindFirstObjectByType<Player>();

        
        health_bar_toggle.onValueChanged.AddListener(OnHealthBarToggleChanged);
    }


    private void OnHealthBarToggleChanged(bool is_on)
    {
        player.health.EnableHealthBar(is_on);
    }


    public void GoMainMenuButton() => GameManager.instance.ChangeScene("MainMenu", RespawnType.NoneSpecific);
}
