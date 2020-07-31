using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HiredSoldierPopUp : UnitySingleton<HiredSoldierPopUp>
{
    [SerializeField] private Button _reSizeButton;
    [SerializeField] private Button _closeButton;
    [SerializeField] private Text _hiredSoldierPopUpText;
    [SerializeField] private Text _levelText;
    [SerializeField] private Text _goldText;
    [SerializeField] private Text _diamondText;

    [HideInInspector] public int[] hiredSoldierLevel = new int[4];
    public bool isButton = false;

    private Text[] _hiredSoldierText = new Text[4];
    private Text[] _hiredSoldierLevelUpGoldText = new Text[4];

    private Image _hiredSoldierPopUp;

    private Vector3 _buttonUp = new Vector3(0, 80);

    private float[] _addHiredSoldierAttackPower = new float[4];
    private int[] _hiredSoldierAddAttackPower = new int[4];
    public int[] _hiredSoldierlevelUpGold = new int[4];
    private int[] _hiredSoldierlevelUpGoldIncrease = new int[4];

    private bool _isResize = false;

    protected override void Awake()
    {
        base.Awake();

        for (int i = 0; i < 4; ++i)
        {
            _addHiredSoldierAttackPower[i] = 10;
            _hiredSoldierAddAttackPower[i] = 10;
            _hiredSoldierlevelUpGold[i] = 1000;
            _hiredSoldierlevelUpGoldIncrease[i] = 1000;
        }
    }

    private void Start()
    {
        _hiredSoldierPopUp = GameObject.Find("HiredSoldierPopUp").GetComponent<Image>();

        for (int i = 0; i < 4; ++i)
        {
            _hiredSoldierLevelUpGoldText[i] = GameObject.Find("MLevelUpGoldText" + i.ToString()).GetComponent<Text>();
            _hiredSoldierText[i] = GameObject.Find("HiredSoliderInfo" + i.ToString()).GetComponent<Text>();
            AchievementPopUp.Instance.sumHiredSoldierLevel += hiredSoldierLevel[i];
        }

        for (int i = 0; i < 4; ++i)
        {
            if (hiredSoldierLevel[i] > 0)
            {
                GameController.Instance.SpawnHiredSoldier(i);
            }
            else
            {
                continue;
            }

            for (int j = 1; j < hiredSoldierLevel[i]; ++j)
            {
                GameController.Instance.CurrentHiredSoldiers[i].attackPower += _hiredSoldierAddAttackPower[i];
                _hiredSoldierlevelUpGold[i] += _hiredSoldierlevelUpGoldIncrease[i];
                SetHiredSoldierLevelUpText(i, _hiredSoldierlevelUpGold[i]);

                _hiredSoldierText[i].text = "레벨 +1, 공격력 +" + _addHiredSoldierAttackPower[i].ToString();
            }
        }

        ScreenSliderUI.Instance.SetHiredSoldiersPowerText(GameController.Instance.CalculateHiredSoldiersPower());
        _hiredSoldierPopUp.gameObject.SetActive(false);
    }

    public void SetHiredSoldierText(int hiredSoldierIndex, int soldierLevel, float soldierAttackPower)
    {
        _hiredSoldierText[hiredSoldierIndex].text = "레벨: " + soldierLevel.ToString() + ", 공격력: " + soldierAttackPower.ToString();
    }

    public void SetHiredSoldierLevelUpText(int hiredSoldierIndex, int gold)
    {
        _hiredSoldierLevelUpGoldText[hiredSoldierIndex].text = gold.ToString();
    }

    public void HiredSoldierButton()
    {
        if (GameController.Instance.CurrentHiredSoldiers[0] != null)
        {
            _hiredSoldierPopUp.gameObject.SetActive(true);
        }
        else
        {
            _hiredSoldierPopUp.gameObject.SetActive(true);
        }
        if (isButton == true)
        {
            CloseButton();
        }
        else
        {
            AchievementPopUp.Instance.CloseButton();
            UpGradePopUp.Instance.CloseButton();
            PassivePopUp.Instance.CloseButton();
            this.transform.position += _buttonUp;
            isButton = true;
        }
        SetPlayerInfo(UpGradePopUp.Instance.playerLevel, GameController.Instance.gold, GameInstance.Instance.CurrentDiamond);
    }


    public void HiredSoldierPurchaseButton(int hiredSoliderIndex)
    {
        if (GameController.Instance.gold < _hiredSoldierlevelUpGold[hiredSoliderIndex])
            return;

        GameController.Instance.gold -= _hiredSoldierlevelUpGold[hiredSoliderIndex];
        GameInfoTextUI.Instance.SetGoldText(GameController.Instance.gold);

        if (hiredSoldierLevel[hiredSoliderIndex] == 0)
        {
            GameController.Instance.SpawnHiredSoldier(hiredSoliderIndex);
            ++hiredSoldierLevel[hiredSoliderIndex];
            return;
        }

        GameController.Instance.CurrentHiredSoldiers[hiredSoliderIndex].attackPower += _hiredSoldierAddAttackPower[hiredSoliderIndex];
        _hiredSoldierlevelUpGold[hiredSoliderIndex] += _hiredSoldierlevelUpGoldIncrease[hiredSoliderIndex];
        SetHiredSoldierLevelUpText(hiredSoliderIndex, _hiredSoldierlevelUpGold[hiredSoliderIndex]);

        _hiredSoldierText[hiredSoliderIndex].text = "레벨 +1, 공격력 +" + _addHiredSoldierAttackPower[hiredSoliderIndex].ToString();
        ++hiredSoldierLevel[hiredSoliderIndex];

        SetPlayerInfo(UpGradePopUp.Instance.playerLevel, GameController.Instance.gold, GameInstance.Instance.CurrentDiamond);

        ScreenSliderUI.Instance.SetHiredSoldiersPowerText(GameController.Instance.CalculateHiredSoldiersPower());
    }

    public void CloseButton()
    {
        _hiredSoldierPopUp.gameObject.SetActive(false);
        if (this.transform.position.y > -10)
        {
            this.transform.position -= _buttonUp;
        }
        isButton = false;
    }

    public void ReSizeButton()
    {
        if (_isResize == false)
        {
            _hiredSoldierPopUp.GetComponent<RectTransform>().sizeDelta = new Vector2(700, 1000);
            _hiredSoldierPopUp.transform.position += new Vector3(0, 350);
            _reSizeButton.transform.position += new Vector3(0, 300);
            _closeButton.transform.position += new Vector3(0, 300);
            _hiredSoldierPopUpText.transform.position += new Vector3(0, 300);
            _isResize = true;
        }
        else
        {
            _hiredSoldierPopUp.GetComponent<RectTransform>().sizeDelta = new Vector2(700, 400);
            _hiredSoldierPopUp.transform.position -= new Vector3(0, 350);
            _reSizeButton.transform.position -= new Vector3(0, 300);
            _closeButton.transform.position -= new Vector3(0, 300);
            _hiredSoldierPopUpText.transform.position -= new Vector3(0, 300);
            _isResize = false;
        }
    }

    public void SetPlayerInfo(int level, int gold, int diamond)
    {
        _levelText.text = "LV " + level.ToString();
        _goldText.text = gold.ToString();
        _diamondText.text = diamond.ToString();
    }
}
