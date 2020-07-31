using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpGradePopUp : UnitySingleton<UpGradePopUp>
{
    [SerializeField] private Text _levelText;
    [SerializeField] private Text _goldText;
    [SerializeField] private Text _diamondText;
    [SerializeField] private Text _playerPopUpText;
    [SerializeField] private Button _reSizeButton;
    [SerializeField] private Button _closeButton;

    public float hiredSoldierAttackSpeedIncrease = 0.1f;
    public int[] skillLevel = new int[5];
    public int playerLevel = 1;
    public Button[] skillPurchaseButton = new Button[5];

    private Text[] _skillLevelUpText = new Text[5];
    private Text[] _skillLevelUpGoldText = new Text[5];
    private int[] _skillLevelUpGold = new int[5];

    private Text _playerLevelUpGoldText;
    private Text _levelUpText;
    private Text _obtainDiamondText;
    private Text _explanationText;

    private Image _upGradePopUp;
    private Image _reincarnationPopUp;

    private float _addAttackPower = 10;
    private float _addAttackPowerIncrease = 5;
    public int _playerLevelUpGold = 1000;
    private int _levelUpGoldIncrease = 1000;

    public bool isButton = false;

    private bool _isResize = false;
    private Vector3 _buttonUp = new Vector3(0, 80);

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < 5; ++i)
        {
            _skillLevelUpText[i] = GameObject.Find("LevelUpText" + i.ToString()).GetComponent<Text>();
            _skillLevelUpGoldText[i] = GameObject.Find("LevelUpGoldText" + i.ToString()).GetComponent<Text>();
            skillLevel[i] = 1;
            _skillLevelUpGold[i] = 1000;

            skillPurchaseButton[i] = GameObject.Find("SkillPurchaseButton" + i.ToString()).GetComponent<Button>();
            if (SkillUI.Instance.isAvailableSkill[i] == false)
            {
                skillPurchaseButton[i].interactable = false;
            }
        }

        _playerLevelUpGoldText = GameObject.Find("LevelUpGoldText").GetComponent<Text>();
        _levelUpText = GameObject.Find("LevelUpText").GetComponent<Text>();
        _obtainDiamondText = GameObject.Find("ObtainDiamondText").GetComponent<Text>();
        _explanationText = GameObject.Find("ExplanationText").GetComponent<Text>();

        _upGradePopUp = GameObject.Find("UpgradePopUp").GetComponent<Image>();
        _reincarnationPopUp = GameObject.Find("ReincarnationPopUp").GetComponent<Image>();

        _upGradePopUp.gameObject.SetActive(false);
        _reincarnationPopUp.gameObject.SetActive(false);

        for (int i = 0; i < 5; ++i)
        {
            switch (i)
            {
                case 0:
                    for (int j = 0; j < skillLevel[i]; ++j)
                    {
                        PlayerController.Instance.skillPower += 0.1f;
                    }
                    break;
                case 1:
                    for (int j = 0; j < skillLevel[i]; ++j)
                    {
                        PlayerController.Instance.criticalPercent += 0.5f;
                    }
                    break;
                case 2:
                    for (int j = 0; j < skillLevel[i]; ++j)
                    {
                        hiredSoldierAttackSpeedIncrease += 0.1f;
                    }
                    break;
                case 3:
                    for (int j = 0; j < skillLevel[i]; ++j)
                    {
                        PlayerController.Instance.tapGold += 10;
                    }
                    break;
                case 4:
                    break;
            }

            SetLevelUpText(i);
        }

        for(int i=1;i<playerLevel;++i)
        {
            PlayerController.Instance.attackPower += _addAttackPower;

            _addAttackPower += _addAttackPowerIncrease;
            _playerLevelUpGold += _levelUpGoldIncrease;

            GameInfoTextUI.Instance.SetLevelText(playerLevel);
            GameInfoTextUI.Instance.SetAttackPowerText(PlayerController.Instance.attackPower);
            SetLevelUpText(_addAttackPower);
            SetPlayerLevelUpGoldText(_playerLevelUpGold);
            SetPlayerInfo(playerLevel, GameController.Instance.gold, GameInstance.Instance.CurrentDiamond);
        }
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void SetLevelUpText(int num)
    {
        switch (num)
        {
            case 0:
                _skillLevelUpText[num].text = "치명적 강타\n" +
                    "LV." + skillLevel[num] + " 기본 공격력의 " + PlayerController.Instance.skillPower + "배 데미지를 입힘\n" +
                    "LV." + (skillLevel[num] + 1) + " 기본 공격력의 " + (PlayerController.Instance.skillPower + 0.1f) + "배 데미지를 입힘";
                break;
            case 1:
                _skillLevelUpText[num].text = "잔인한 타격\n" +
                    "Lv." + skillLevel[num] + " 5초동안 크리티컬 확률 " + (PlayerController.Instance.criticalPercent * 10) + "% 증가\n" +
                    "Lv." + (skillLevel[num] + 1) + " 5초동안 크리티컬 확률 " + (PlayerController.Instance.criticalPercent * 10 + 5) + "% 증가";
                break;
            case 2:
                _skillLevelUpText[num].text = "용병 포션\n" +
                    "Lv." + skillLevel[num] + " 10초 동안 용병들의 공격속도 " + (hiredSoldierAttackSpeedIncrease * 100) + "% 증가\n" +
                    "Lv." + (skillLevel[num] + 1) + " 10초 동안 용병들의 공격속도 " + (hiredSoldierAttackSpeedIncrease * 100 + 10) + "% 증가";
                break;
            case 3:
                _skillLevelUpText[num].text = "미다스의 손\n" +
                    "Lv." + skillLevel[num] + " 6초동안 텝당 " + PlayerController.Instance.tapGold + "골드 획득\n" +
                    "Lv." + (skillLevel[num] + 1) + " 6초동안 텝당 " + (PlayerController.Instance.tapGold + 10) + "골드 획득";
                break;
            case 4:
                _skillLevelUpText[num].text = "강력한 궁극기\n" +
                    "몬스터 피통 반을 깎음";
                break;
        }
    }

    public void SetPlayerLevelUpGoldText(int levelUpGoldText)
    {
        _playerLevelUpGoldText.text = levelUpGoldText.ToString();
    }

    public void SetSkillLevelUpGoldText(int[] skillLevelUpGold)
    {
        for (int i = 0; i < 5; ++i)
        {
            _skillLevelUpGoldText[i].text = skillLevelUpGold[i].ToString();
        }
    }

    public void SetLevelUpText(float addAttackPower)
    {
        _levelUpText.text = "레벨 +1 증가, 공격력 + " + addAttackPower.ToString() + "증가";
    }

    public void SetPlayerInfo(int level, int gold, int diamond)
    {
        _levelText.text = "LV " + level.ToString();
        _goldText.text = gold.ToString();
        _diamondText.text = diamond.ToString();
    }

    public void UpgradeBotton()
    {
        _upGradePopUp.gameObject.SetActive(true);
        SetPlayerLevelUpGoldText(_playerLevelUpGold);
        SetSkillLevelUpGoldText(_skillLevelUpGold);
        SetLevelUpText(_addAttackPower);
        // SetPlayerInfo(GameInfoTextUI.Instance.)
        for (int i = 0; i < 5; ++i)
        {
            UpGradePopUp.Instance.SetLevelUpText(i);
        }
        if (isButton == true)
        {
            CloseButton();
        }
        else
        {
            AchievementPopUp.Instance.CloseButton();
            HiredSoldierPopUp.Instance.CloseButton();
            PassivePopUp.Instance.CloseButton();
            this.transform.position += _buttonUp;
            isButton = true;
        }
        SetPlayerInfo(playerLevel, GameController.Instance.gold, GameInstance.Instance.CurrentDiamond);

    }

    public void UpGradePurchaseButton()
    {
        if (GameController.Instance.gold >= _playerLevelUpGold)
        {
            playerLevel += 1;
            PlayerController.Instance.attackPower += _addAttackPower;

            GameController.Instance.gold -= _playerLevelUpGold;
            _addAttackPower += _addAttackPowerIncrease;
            _playerLevelUpGold += _levelUpGoldIncrease;

            GameInfoTextUI.Instance.SetLevelText(playerLevel);
            GameInfoTextUI.Instance.SetGoldText(GameController.Instance.gold);
            GameInfoTextUI.Instance.SetAttackPowerText(PlayerController.Instance.attackPower);
            SetLevelUpText(_addAttackPower);
            SetPlayerLevelUpGoldText(_playerLevelUpGold);
            SetPlayerInfo(playerLevel, GameController.Instance.gold, GameInstance.Instance.CurrentDiamond);
        }
    }

    public void FatalBlowPerChaseButton()
    {
        if (GameController.Instance.gold >= 100)
        {
            PlayerController.Instance.skillPower += 0.1f;
            GameController.Instance.gold -= 100;
            SetLevelUpText(0);
            GameInfoTextUI.Instance.SetGoldText(GameController.Instance.gold);
            ++skillLevel[0];
        }
    }

    public void ViciousBlowPurchaseButton()
    {
        if (GameController.Instance.gold >= 100)
        {
            PlayerController.Instance.criticalPercent += 0.5f;
            GameController.Instance.gold -= 100;
            SetLevelUpText(1);
            GameInfoTextUI.Instance.SetGoldText(GameController.Instance.gold);
            ++skillLevel[1];
        }
    }

    public void HiredSoldierPotionButton()
    {
        if (GameController.Instance.gold >= 100)
        {
            hiredSoldierAttackSpeedIncrease += 0.1f;
            GameController.Instance.gold -= 100;
            SetLevelUpText(2);
            GameInfoTextUI.Instance.SetGoldText(GameController.Instance.gold);
            ++skillLevel[2];
        }
    }

    public void MidasTouchPurchaseButton()
    {
        if (GameController.Instance.gold >= 100)
        {
            PlayerController.Instance.tapGold += 10;
            GameController.Instance.gold -= 100;
            SetLevelUpText(3);
            GameInfoTextUI.Instance.SetGoldText(GameController.Instance.gold);
            ++skillLevel[3];
        }
    }

    public void CloseButton()
    {
        _upGradePopUp.gameObject.SetActive(false);
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
            _upGradePopUp.GetComponent<RectTransform>().sizeDelta = new Vector2(700, 1000);
            _upGradePopUp.transform.position += new Vector3(0, 350);
            _reSizeButton.transform.position += new Vector3(0, 300);
            _closeButton.transform.position += new Vector3(0, 300);
            _playerPopUpText.transform.position += new Vector3(0, 300);
            _isResize = true;
        }
        else
        {
            _upGradePopUp.GetComponent<RectTransform>().sizeDelta = new Vector2(700, 400);
            _upGradePopUp.transform.position -= new Vector3(0, 350);
            _reSizeButton.transform.position -= new Vector3(0, 300);
            _closeButton.transform.position -= new Vector3(0, 300);
            _playerPopUpText.transform.position -= new Vector3(0, 300);
            _isResize = false;
        }
    }

    public void Reincarnation()
    {
        if (GameController.Instance.CurrentStage >= 3)
        {
            GameInstance.Instance.CurrentDiamond += (int)PassivePopUp.Instance.passiveValue[9] + GameController.Instance.CurrentStage - 2;
            GameInstance.Instance.isReincarnation = true;
            SceneController.Instance.ResetScene();
        }
    }

    public void ReincarnationPopUpButton()
    {
        _reincarnationPopUp.gameObject.SetActive(true);

        if (GameController.Instance.CurrentStage >= 3)
        {
            _obtainDiamondText.gameObject.SetActive(true);
            _obtainDiamondText.text = "지금 환생 시 다이아 " + 
                ((int)PassivePopUp.Instance.passiveValue[9] + GameController.Instance.CurrentStage - 2).ToString()
                + "개를 얻습니다.";
            _explanationText.text = "환생 시 패시브를 제외한 모든 것을 잃게 됩니다.\n그래도 환생하시겠습니까?";
        }
        else
        {
            _obtainDiamondText.gameObject.SetActive(false);
            _explanationText.text = "현재는 환생할 수 없습니다.\n" + "스테이지 3이상 클리어 후 가능합니다."; 
        }
    }

    public void ReincarnationPopUpCloseButton()
    {
        _reincarnationPopUp.gameObject.SetActive(false);
    }
}