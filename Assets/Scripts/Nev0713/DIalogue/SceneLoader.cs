using System;
using UnityEngine.SceneManagement;


public class TitleButton
{
    public void LoadScene(string pSceneName, Action pOnComplete)
    {
        SceneManager.LoadScene(pSceneName);
        pOnComplete?.Invoke();
    }
}
