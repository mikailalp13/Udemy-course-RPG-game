using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UI_Dialogue : MonoBehaviour
{
    private UI ui;
    private DialogueNpcData npc_data;
    private Player_QuestManager quest_manager;


    [SerializeField] private Image speaker_portrait;
    [SerializeField] private TextMeshProUGUI speaker_name;
    [SerializeField] private TextMeshProUGUI dialogue_text;
    [SerializeField] private TextMeshProUGUI[] dialogue_choices_text;

    [Space]
    [SerializeField] private float text_speed = 0.1f;
    private string full_text_to_display;
    private Coroutine type_text_co;

    private DialogueLineSO current_line;
    private DialogueLineSO selected_choice;
    private DialogueLineSO[] current_choices;
    private int selected_choice_index;

    private bool waiting_to_confirm;
    private bool can_interact;


    private void Awake()
    {
        ui = GetComponentInParent<UI>();
        quest_manager = Player.instance.quest_manager;
    }


    public void SetupNpcData(DialogueNpcData npc_data) => this.npc_data = npc_data;


    public void PlayDialogueLine(DialogueLineSO line)
    {
        current_line = line;
        current_choices = line.choice_lines;
        can_interact = false;
        selected_choice = null;
        selected_choice_index = 0; // selecet first option by default 
        
        HideAllChoices();

        speaker_portrait.sprite = line.speaker.speaker_portrait;
        speaker_name.text = line.speaker.speaker_name;

        full_text_to_display = line.action_type == DialogueActionType.None || line.action_type == DialogueActionType.PlayerMakeChoice ? 
                        line.GetRandomLine() : line.action_line;

        type_text_co = StartCoroutine(TypeTextCo(full_text_to_display));
        StartCoroutine(EnableInteractionCo());
    }


    private void HandleNextAction()
    {
        switch (current_line.action_type)
        {
            case DialogueActionType.OpenShop:
                ui.SwitchToInGameUI();
                ui.OpenMerchantUI(true);
                break;

            case DialogueActionType.PlayerMakeChoice: 
                if (selected_choice == null) 
                { 
                    ShowChoices(); 
                } 

                else 
                { 
                    DialogueLineSO chosen = current_choices[selected_choice_index]; 
                    PlayDialogueLine(chosen); 
                }
                break;
            
            case DialogueActionType.OpenQuest:
                ui.SwitchToInGameUI();
                ui.OpenQuestUI(npc_data.quests);
                break;
            
            case DialogueActionType.GetQuestReward:
                ui.SwitchToInGameUI();
                quest_manager.TryGetRewardFrom(npc_data.npc_reward_type);
                break;

            case DialogueActionType.OpenCraft:
                ui.SwitchToInGameUI();
                ui.OpenCraftUI(true);
                break;

            case DialogueActionType.CloseDialogue:
                ui.SwitchToInGameUI();
                break;
        }
    }


    public void DialogueInteraction() 
    { 
        if (can_interact == false) 
            return; 

        if (type_text_co != null) 
        { 
            CompleteTyping(); 

            if (current_line.action_type != DialogueActionType.PlayerMakeChoice)
                waiting_to_confirm = true;
            else
                HandleNextAction();
            
            return; 
        } 

        if (waiting_to_confirm || selected_choice != null) 
        { 
            waiting_to_confirm = false; 
            HandleNextAction(); 
        } 
    }


    private void CompleteTyping()
    {
        if (type_text_co != null)
        {
            StopCoroutine(type_text_co);
            dialogue_text.text = full_text_to_display;
            type_text_co = null;
        }    
    }


    private void ShowChoices()
    {
        for (int i = 0; i < dialogue_choices_text.Length; i++)
        {
            if (i < current_choices.Length)
            {
                DialogueLineSO choice = current_choices[i];
                string choice_text = choice.player_choice_answer;

                dialogue_choices_text[i].gameObject.SetActive(true);
                dialogue_choices_text[i].text = selected_choice_index == i ? $"<color=yellow> → {choice_text}" : $"→ {choice_text}";

                if (choice.action_type == DialogueActionType.GetQuestReward && quest_manager.HasCompletedQuest() == false)
                    dialogue_choices_text[i].gameObject.SetActive(false);
            }
            else
            {
                dialogue_choices_text[i].gameObject.SetActive(false);
            }
        }
        selected_choice = current_choices[selected_choice_index];
    }


    private void HideAllChoices()
    {

        foreach (var obj in dialogue_choices_text)
            obj.gameObject.SetActive(false);
    }


    public void NavigateChoice(int direction)
    {
        if (current_choices == null || current_choices.Length <= 1)
            return;
        
        selected_choice_index += direction;
        selected_choice_index = Mathf.Clamp(selected_choice_index, 0, current_choices.Length - 1); 
        ShowChoices();
    }


    private IEnumerator TypeTextCo(string text)
    {
        dialogue_text.text = "";

        foreach (char letter in text)
        {
            dialogue_text.text += letter;
            yield return new WaitForSeconds(text_speed);
        }

        if (current_line.action_type != DialogueActionType.PlayerMakeChoice)
        {
            waiting_to_confirm = true;
        }
        else
        {
            yield return new WaitForSeconds(0.2f);   
            selected_choice = null; 
            HandleNextAction();
        }
        type_text_co = null;
    }


    private IEnumerator EnableInteractionCo()
    {
        yield return null;
        can_interact = true;
    }
}
