using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] GameData gameData;
    [SerializeField] string sceneName;

    public void OnClicked()
    {
        StartCoroutine(LoadScene(sceneName));
    }

    public void OnClickedStartNewGame()
    {
        gameData.Init();
        StartCoroutine(LoadScene(sceneName));
    }

    public void OnClickedContinue()
    {
        StartCoroutine(LoadScene(gameData.LastSceneName));
    }

    private IEnumerator LoadScene(string pSceneName)
    {
        Debug.Log("StartLoadScene");
        if (pSceneName != null)
        {
            ScreenFader.FadeIn();
            yield return new WaitForSecondsRealtime(1);
            SceneManager.LoadScene(pSceneName);
        }
        Debug.Log("SceneLoaded");
    }

    public void FastLoadScene()
    {
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1;
    }

    public void OnClickedExit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
