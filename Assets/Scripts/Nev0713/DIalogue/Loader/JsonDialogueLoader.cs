using UnityEngine;


public class JsonDialogueLoader : IDialogueLoader
{
    public SceneData LoadDialogue(string pFileName)
    {
        TextAsset json = Resources.Load<TextAsset>(pFileName);
        if (json == null)
        {
            Debug.LogError($"Can't find any JSON file: {pFileName}");
            return null;
        }

        return JsonUtility.FromJson<SceneData>(json.text);
    }
}