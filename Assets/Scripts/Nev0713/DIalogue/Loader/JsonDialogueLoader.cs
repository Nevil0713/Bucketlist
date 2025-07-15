using UnityEngine;


public class JsonDialogueLoader : IDialogueLoader
{
    public SceneData LoadDialogue(string pFileName)
    {
        TextAsset json = Resources.Load<TextAsset>(pFileName);
        if (json == null)
        {
            Debug.LogError($"JSON 파일을 찾을 수 없습니다: {pFileName}");
            return null;
        }

        return JsonUtility.FromJson<SceneData>(json.text);
    }
}