using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenButtonUI : UnitySingleton<ScreenButtonUI>
{
    [HideInInspector] public Button bossAppearanceButton;
    private Image _soundImage;
    private Image _SettingPopUp;
    private void Start()
    {
        bossAppearanceButton = GameObject.Find("BossAppearanceButton").GetComponent<Button>();
        bossAppearanceButton.gameObject.SetActive(false);

        _soundImage = GameObject.Find("SoundButton").GetComponent<Image>();
        _SettingPopUp = GameObject.Find("SettingPopUp").GetComponent<Image>();
        _SettingPopUp.gameObject.SetActive(false);
    }

    public void DataSave()
    {
        JsonHelper.Save();
    }

    public void Quit()
    {
        Application.Quit();

        // UnityEditor.EditorApplication.isPlaying = false;
    }

    public void Sound()
    {
        if (AudioListener.pause == false)
        {
            AudioListener.pause = true;
            _soundImage.sprite = Resources.Load<Sprite>("UI/shh");
        }
        else
        {
            AudioListener.pause = false;
            _soundImage.sprite = Resources.Load<Sprite>("UI/sound");
        }
    }

    public void SettingPopUp()
    {
        _SettingPopUp.gameObject.SetActive(true);
    }

    public void CloseButton()
    {
        _SettingPopUp.gameObject.SetActive(false);
    }

    public void DataReset()
    {
        JsonHelper.Reset();
    }
}
