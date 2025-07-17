using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Scriptable Objects/GameData")]
public class GameData : ScriptableObject
{
    public int MinigameFails;

    public void Init()
    {
        MinigameFails = 0;
    }
}
