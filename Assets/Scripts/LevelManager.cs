using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private string music_group_name;

    private void Start()
    {
        AudioManager.instance.StartBGM(music_group_name);
    }
}
