using System;
using System.IO;
using UnityEngine;

public class FileDataHandler
{
    private string full_path;
    private bool encrypt_data;
    private string code_word = "unityrpg";

    public FileDataHandler(string data_dir_path, string data_file_name, bool encrypt_data)
    {
        full_path = Path.Combine(data_dir_path, data_file_name);
        this.encrypt_data = encrypt_data;
    }

    public void SaveData(GameData game_data)
    {
        try
        {
            // 1. Create directory if it doesn't exist
            Directory.CreateDirectory(Path.GetDirectoryName(full_path)); 

            // 2. Convert GameData to JSON string
            string data_to_save = JsonUtility.ToJson(game_data, true); 

            if (encrypt_data)
                data_to_save = EncryptDecrypt(data_to_save);

            // 3. Open or create the file itself
            using(FileStream stream = new FileStream(full_path, FileMode.Create)) 
            {
                // 4. Write JSON text to the file
                using(StreamWriter write = new StreamWriter(stream)) 
                {
                    write.Write(data_to_save);
                }
            }
        }
        catch (Exception e)
        {
            // Log any error that happens
            Debug.LogError("Error on trying to save data to file: " + full_path + "\n" + e);
        }
    }

    
    public GameData LoadData()
    {
        GameData load_data = null;

        // 1. Check if the save file exists
        if (File.Exists(full_path))
        {
            try
            {
                string data_to_load = "";

                // 2. Open the file
                using(FileStream stream = new FileStream(full_path, FileMode.Open))
                {
                    // 3. Read file's text content
                    using(StreamReader reader = new StreamReader(stream))
                    {
                        data_to_load = reader.ReadToEnd();
                    }
                }

                if(encrypt_data)
                    data_to_load = EncryptDecrypt(data_to_load);

                // 4. Convert the JSON string back into a GameData object 
                load_data = JsonUtility.FromJson<GameData>(data_to_load);
            }
            catch (Exception e)
            {
                // Log any error that happens
                Debug.LogError("Error on trying to load data from file: " + full_path + "\n" + e);
            }
        }

        return load_data;
    }

    public void Delete()
    {
        if (File.Exists(full_path))
            File.Delete(full_path);
    }

    private string EncryptDecrypt(string data)
    {
        string modified_data = "";

        for (int i = 0; i < data.Length; i++)
        {
            modified_data += (char)(data[i] ^ code_word[i % code_word.Length]);
        }

        return modified_data;
    }
}
