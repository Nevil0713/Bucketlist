using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "GameData", menuName = "Scriptable Objects/GameData")]
public class GameData : ScriptableObject
{
    public string LastSceneName;
    public int MinigameFails;

    public void Init()
    {
        LastSceneName = "MainScene";
        MinigameFails = 0;
    }
}
