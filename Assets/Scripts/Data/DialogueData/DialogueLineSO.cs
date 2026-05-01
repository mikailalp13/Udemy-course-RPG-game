using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup / Dialogue Data / New Line Data", fileName = "Line - ")]

public class DialogueLineSO : ScriptableObject
{
    [Header("Dialogue Info")]
    public string dialogue_group_name;
    public DialogueSpeakerSO speaker;


    [Header("Text Options")]
    [TextArea] public string[] text_line;


    [Header("Choices Info")]
    [TextArea] public string player_choice_answer;
    public DialogueLineSO[] choice_lines;


    [Header("Dialogue Action")]
    [TextArea] public string action_line;
    public DialogueActionType action_type;



    public string GetFirstLine() => text_line[0];
    
    public string GetRandomLine()
    {
        return text_line[Random.Range(0, text_line.Length)];
    }
}
