using UnityEngine;

public class Setting : MonoBehaviour
{
    [SerializeField] private GameObject settingObject;
    bool isSettingOpened;

    private void Awake()
    {
        settingObject.SetActive(false);
        isSettingOpened = false;
    }

    public void OnClicked()
    {
        if(isSettingOpened)
        {
            CloseSetting();
            Time.timeScale = 1;
        }
        else
        {
            OpenSetting();
            Time.timeScale = 0;
        }
    }

    private void OpenSetting()
    {
        isSettingOpened = true;
        settingObject.SetActive(true);
    }

    private void CloseSetting()
    {
        isSettingOpened = false;
        settingObject.SetActive(false);

    }
}
