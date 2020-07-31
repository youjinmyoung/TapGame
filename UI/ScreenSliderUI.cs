using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenSliderUI : UnitySingleton<ScreenSliderUI>
{
    private Slider _enemyHpSlider;
    private Slider _bossCoolTimeSlider;
    private Slider _playerMpSlider;

    private Text _enemyHpText;
    private Text _enemyNameText;
    private Text _playerMpText;
    private Text _hiredSoldiersPowerText;

    private void Start()
    {
        _enemyHpSlider = GameObject.Find("EnemyHpSlider").GetComponent<Slider>();

        _bossCoolTimeSlider = GameObject.Find("BossCoolTimeSlider").GetComponent<Slider>();
        _bossCoolTimeSlider.gameObject.SetActive(false);

        _playerMpSlider = GameObject.Find("PlayerMpBar").GetComponent<Slider>();

        _enemyHpText = GameObject.Find("EnemyHpText").GetComponent<Text>();
        _enemyNameText = GameObject.Find("EnemyNameText").GetComponent<Text>();
        _playerMpText = GameObject.Find("PlayerMpText").GetComponent<Text>();
        _hiredSoldiersPowerText = GameObject.Find("HiredSoldiersPower").GetComponent<Text>();

        SetHiredSoldiersPowerText(GameController.Instance.CalculateHiredSoldiersPower());
    }

    public void SetEnemyHpSlider(float maxHp, float hp)
    {
        _enemyHpSlider.maxValue = maxHp;
        _enemyHpSlider.value = hp;
    }

    public void SetBossCoolTimeSlider(float maxTime, float time)
    {
        _bossCoolTimeSlider.maxValue = maxTime;
        _bossCoolTimeSlider.value = time;
    }

    public void SetActiveBossCoolTimeSlider(bool active)
    {
        _bossCoolTimeSlider.gameObject.SetActive(active);
    }

    public void SetPlayerMpSlider(float maxMp, float mp)
    {
        _playerMpSlider.maxValue = maxMp;
        _playerMpSlider.value = mp;
    }

    public void SetEnemyNameText(int enemyIndex)
    {
        switch (enemyIndex)
        {
            case 0:
                _enemyNameText.text = "Fanatic";
                break;
            case 1:
                _enemyNameText.text = "Uray";
                break;
        }
    }

    public void SetEnemyHpText(float maxhp, float hp)
    {
        _enemyHpText.text = hp.ToString() + " / " + maxhp.ToString();
    }

    public void SetPlayerMpText(float maxMp, float mp)
    {
        _playerMpText.text = mp.ToString() + " / " + maxMp.ToString();
    }

    public void SetHiredSoldiersPowerText(float hiredSolderisPower)
    {
        if (_hiredSoldiersPowerText != null)
        {
            _hiredSoldiersPowerText.text = hiredSolderisPower.ToString();
        }
    }
}
