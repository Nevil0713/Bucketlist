using UnityEngine;

public class EndingDialogue : MonoBehaviour
{
    [SerializeField] private DialogueController dialogueController;
    private GameData gameData => dialogueController.GetData();

    private void Awake()
    {

        if(gameData.MinigameFails >= 4)
        {
            dialogueController.SetDialogueName("DeadEnd");
        }
        else if(gameData.MinigameFails >= 2)
        {
            dialogueController.SetDialogueName("BadEnd");
        }
        else
        {
            dialogueController.SetDialogueName("HappyEnd");
        }

        ScreenFader.FadeIn();
        dialogueController.StartFirstDialogue();
    }
}
