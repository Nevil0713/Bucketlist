using TMPro;
using UnityEngine;

public class Teto_Timer : MonoBehaviour
{
    public Teto_Stage teteto;

    public float GameTime = 60f;
    public TMP_Text Time_Text;

    private bool isTimeUp = false;

    private void Start()
    {
        if (teteto == null)
        {
            teteto = FindFirstObjectByType<Teto_Stage>();
        }
    }
    private void Update()
    {
        if (isTimeUp) return;

        GameTime -= Time.deltaTime;

        if (GameTime <= 0f)
        {
            GameTime = 0f;
            Time_Text.text = "0";
            TimesUp();
            return;
        }

        Time_Text.text = "Time: " + Mathf.CeilToInt(GameTime).ToString();
    }

    //Time Over Game Over
    void TimesUp()
    {
        isTimeUp = true;

        if (!teteto.gameoverPanel.activeSelf && !teteto.gamedonePanel.activeSelf)
        {
            teteto.Invoke("gameOver_setters", 0.5f);
        }
    }
}
