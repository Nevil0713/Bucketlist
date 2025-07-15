using UnityEngine;


public class JsonDialogueLoader : IDialogueLoader
{
    public SceneData LoadDialogue(string pFileName)
    {
        TextAsset json = Resources.Load<TextAsset>(pFileName);
        if (json == null)
        {
            Debug.LogError($"JSON ������ ã�� �� �����ϴ�: {pFileName}");
            return null;
        }

        return JsonUtility.FromJson<SceneData>(json.text);
    }
}