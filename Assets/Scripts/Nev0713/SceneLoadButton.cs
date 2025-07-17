using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadButton : MonoBehaviour
{
    [SerializeField] string sceneName;

    public void OnClicked()
    {
        StartCoroutine(LoadScene());
    }

    private IEnumerator LoadScene()
    {
        if (sceneName != null)
        {
            ScreenFader.FadeIn();
            yield return new WaitForSeconds(1);
            SceneManager.LoadScene(sceneName);
        }
    }

    public void OnClickedExit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
