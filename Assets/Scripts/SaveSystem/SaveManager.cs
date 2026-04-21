using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    private FileDataHandler data_handler;
    private GameData game_data;
    private List<ISaveable> all_saveables;

    [SerializeField] private string file_name = "unitygamesaving.json";
    [SerializeField] private bool encrypt_data = true;


    private void Awake()
    {
        instance = this;
    }

    // When you have start as an IEnumerator unity starts that coroutine automatically, we did this to avoid bugs related to start functions execute orders
    private IEnumerator Start()
    {
        Debug.Log(Application.persistentDataPath);
        data_handler = new FileDataHandler(Application.persistentDataPath, file_name, encrypt_data);
        all_saveables = FindISaveables();

        yield return new WaitForSeconds(0.01f);
        LoadGame();
    }


    private void LoadGame()
    {
        game_data = data_handler.LoadData();

        if (game_data == null)
        {
            Debug.Log("No saved data found, creating new save.");
            game_data = new GameData();
            return;
        }

        foreach (var saveable in all_saveables)
            saveable.LoadData(game_data);
    }


    public void SaveGame()
    {
        foreach (var saveable in all_saveables)
            saveable.SaveData(ref game_data);

        data_handler.SaveData(game_data);
    }


    public GameData GetGameData() => game_data;


    [ContextMenu("*** Delete Saved Data ***")]
    public void DeleteSaveData()
    {
        data_handler = new FileDataHandler(Application.persistentDataPath, file_name, encrypt_data);
        data_handler.Delete();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<ISaveable> FindISaveables()
    {
        return
            FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None)
            .OfType<ISaveable>()
            .ToList();
    }
}
