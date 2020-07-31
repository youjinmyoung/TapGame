using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PassivePopUp : UnitySingleton<PassivePopUp>
{
    [SerializeField] private Button _reSizeButton;
    [SerializeField] private Button _closeButton;
    [SerializeField] private Text _PassivePopUpText;
    [SerializeField] private Text _levelText;
    [SerializeField] private Text _goldText;
    [SerializeField] private Text _diamondText;

    public int[] needDiamond;
    public int[] needDiamondIncrease;
    public float[] passiveValue;
    public float[] passiveValueIncrease;

    private Image _passivePopUpImage;

    private Text[] _needDiamondText = new Text[20];
    private Text[] _passiveLevelUpText = new Text[20];
    private Text[] _passiveLevelUpIncreaseText = new Text[20];

    public bool isButton = false;
    private Vector3 _buttonUp = new Vector3(0, 80);
    private bool _isResize = false;

    private void Start()
    {
        _passivePopUpImage = GameObject.Find("PassivePopUp").GetComponent<Image>();

        for (int i = 0; i < 20; ++i)
        {
            _needDiamondText[i] = GameObject.Find("NeedDiamondText" + i.ToString()).GetComponent<Text>();
            _passiveLevelUpText[i] = GameObject.Find("PassiveLevelUpText" + i.ToString()).GetComponent<Text>();
            _passiveLevelUpIncreaseText[i] = GameObject.Find("PassiveLevelUpIncreaseText" + i.ToString()).GetComponent<Text>();
        }

        for (int i = 0; i < GameInstance.Instance.CurrentPassiveLevel.Length; ++i)
        {
            for (int j = 0; j < GameInstance.Instance.CurrentPassiveLevel[i]; ++j)
            {
                PassiveLevelUp(i);
            }
        }

        if (GameInstance.Instance.isReincarnation)
        {
            GameController.Instance.gold += GameInstance.Instance.CurrentPassiveLevel[2] * (int)passiveValue[2];
            GameController.Instance.saveGold += GameInstance.Instance.CurrentPassiveLevel[2] * (int)passiveValue[2];
            GameInfoTextUI.Instance.SetGoldText(GameController.Instance.gold);
        }

        _passivePopUpImage.gameObject.SetActive(false);
    }

    public void PassivePopUpButton()
    {
        _passivePopUpImage.gameObject.SetActive(true);
        if (isButton == true)
        {
            CloseButton();
        }
        else
        {
            AchievementPopUp.Instance.CloseButton();
            HiredSoldierPopUp.Instance.CloseButton();
            UpGradePopUp.Instance.CloseButton();
            this.transform.position += _buttonUp;
            isButton = true;
        }
        SetPlayerInfo(UpGradePopUp.Instance.playerLevel, GameController.Instance.gold, GameInstance.Instance.CurrentDiamond);
    }

    public void CloseButton()
    {
        _passivePopUpImage.gameObject.SetActive(false);
        if (this.transform.position.y > -10)
        {
            this.transform.position -= _buttonUp;
        }
        isButton = false;
    }

    public void PurchaseButton(int popUpIndex)
    {
        if (needDiamond[popUpIndex] > GameInstance.Instance.CurrentDiamond)
            return;

        ++GameInstance.Instance.CurrentPassiveLevel[popUpIndex];
        GameInstance.Instance.CurrentDiamond -= needDiamond[popUpIndex];
        GameInfoTextUI.Instance.SetDiamondText(GameInstance.Instance.CurrentDiamond);
        PassiveLevelUp(popUpIndex);
        SetPlayerInfo(UpGradePopUp.Instance.playerLevel, GameController.Instance.gold, GameInstance.Instance.CurrentDiamond);
    }

    public void PassiveLevelUp(int popUpIndex)
    {
        passiveValue[popUpIndex] += passiveValueIncrease[popUpIndex];
        needDiamond[popUpIndex] += needDiamondIncrease[popUpIndex];
        _needDiamondText[popUpIndex].text = needDiamond[popUpIndex].ToString();

        switch (popUpIndex)
        {
            case 0:
                _passiveLevelUpText[popUpIndex].text = "공격력 " + passiveValue[popUpIndex].ToString() + " 증가";
                _passiveLevelUpIncreaseText[popUpIndex].text = "구매 시 공격력 " + passiveValueIncrease[popUpIndex].ToString() + "% 상승";

                PlayerController.Instance.attackPower += passiveValueIncrease[popUpIndex];
                GameInfoTextUI.Instance.SetAttackPowerText(PlayerController.Instance.attackPower);
                break;
            case 1:
                _passiveLevelUpText[popUpIndex].text = "보스 쿨타임 " + passiveValue[popUpIndex].ToString() + "초 증가";
                _passiveLevelUpIncreaseText[popUpIndex].text = "구매 시 보스 쿨타임 " + passiveValueIncrease[popUpIndex].ToString() + "초 상승";

                GameController.Instance.bossMaxCoolTime += passiveValueIncrease[popUpIndex];
                break;
            case 2:
                _passiveLevelUpText[popUpIndex].text = "골드 " + passiveValue[popUpIndex].ToString() + "로 시작";
                _passiveLevelUpIncreaseText[popUpIndex].text = "구매 시 시작골드 " + passiveValueIncrease[popUpIndex].ToString() + " 상승";
                break;
            case 3:
                _passiveLevelUpText[popUpIndex].text = "일반몹 처치 시 " + passiveValue[popUpIndex].ToString() + "골드 추가 드랍";
                _passiveLevelUpIncreaseText[popUpIndex].text = "구매 시 드랍골드 " + passiveValueIncrease[popUpIndex].ToString() + " 상승";
                break;
            case 4:
                _passiveLevelUpText[popUpIndex].text = "보스몹 처치 시 " + passiveValue[popUpIndex].ToString() + "골드 추가 드랍";
                _passiveLevelUpIncreaseText[popUpIndex].text = "구매 시 드랍골드 " + passiveValueIncrease[popUpIndex].ToString() + " 상승";
                break;
            case 5:
                _passiveLevelUpText[popUpIndex].text = "궁극기 쿨타임 " + passiveValue[popUpIndex].ToString() + "초 감소";
                _passiveLevelUpIncreaseText[popUpIndex].text = "구매 시 궁극기 쿨타임 " + passiveValueIncrease[popUpIndex].ToString() + "초 감소";

                if(GameInstance.Instance.CurrentPassiveLevel[popUpIndex] >= 30)
                {
                    GameObject.Find("PassivePurchaseButton" + popUpIndex.ToString()).GetComponent<Button>().interactable = false;
                }
                break;
            case 6:
                _passiveLevelUpText[popUpIndex].text = "전체 마나 " + passiveValue[popUpIndex].ToString() + " 증가";
                _passiveLevelUpIncreaseText[popUpIndex].text = "구매 시 전체 마나 " + passiveValueIncrease[popUpIndex].ToString() + " 상승";

                PlayerController.Instance.maxMp += passiveValueIncrease[popUpIndex];
                break;
            case 7:
                _passiveLevelUpText[popUpIndex].text = "치명적 강타 " + passiveValue[popUpIndex].ToString() + "배수 증가";
                _passiveLevelUpIncreaseText[popUpIndex].text = "구매 시 치명적 강타 " + passiveValueIncrease[popUpIndex].ToString() + "배수 상승";

                PlayerController.Instance.skillPower += passiveValueIncrease[popUpIndex];
                break;
            case 8:
                _passiveLevelUpText[popUpIndex].text = "마나 회복 속도 " + passiveValue[popUpIndex].ToString() + " 증가";
                _passiveLevelUpIncreaseText[popUpIndex].text = "구매 시 마나 회복 속도 " + passiveValueIncrease[popUpIndex].ToString() + " 상승";

                PlayerController.Instance.addMpPerSec += passiveValueIncrease[popUpIndex];

                if (GameInstance.Instance.CurrentPassiveLevel[popUpIndex] >= 100)
                {
                    GameObject.Find("PassivePurchaseButton" + popUpIndex.ToString()).GetComponent<Button>().interactable = false;
                }
                break;
            case 9:
                _passiveLevelUpText[popUpIndex].text = "환생 시 다이아 " + passiveValue[popUpIndex].ToString() + "개 증가";
                _passiveLevelUpIncreaseText[popUpIndex].text = "구매 시 환생 다이아 " + passiveValueIncrease[popUpIndex].ToString() + "개 상승";
                break;
            case 10:
                _passiveLevelUpText[popUpIndex].text = "모든 용병 데미지 " + passiveValue[popUpIndex].ToString() + " 증가";
                _passiveLevelUpIncreaseText[popUpIndex].text = "구매 시 모든 용병 데미지 " + passiveValueIncrease[popUpIndex].ToString() + " 상승";

                for (int i=0;i< GameController.Instance.CurrentHiredSoldiers.Length;++i)
                {
                    if(GameController.Instance.CurrentHiredSoldiers[i] != null)
                    {
                        GameController.Instance.CurrentHiredSoldiers[i].attackPower += passiveValueIncrease[popUpIndex];
                    }
                }

                ScreenSliderUI.Instance.SetHiredSoldiersPowerText(GameController.Instance.CalculateHiredSoldiersPower());
                break;
            case 11:
                _passiveLevelUpText[popUpIndex].text = "모든 용병 공격속도 " + passiveValue[popUpIndex].ToString() + "초 증가";
                _passiveLevelUpIncreaseText[popUpIndex].text = "구매 시 모든 용병 공격속도 " + passiveValueIncrease[popUpIndex].ToString() + "초 상승";

                for (int i = 0; i < GameController.Instance.CurrentHiredSoldiers.Length; ++i)
                {
                    if (GameController.Instance.CurrentHiredSoldiers[i] != null)
                    {
                        GameController.Instance.CurrentHiredSoldiers[i].attackSpeed -= passiveValueIncrease[popUpIndex];
                    }
                }

                if (GameInstance.Instance.CurrentPassiveLevel[popUpIndex] >= 30)
                {
                    GameObject.Find("PassivePurchaseButton" + popUpIndex.ToString()).GetComponent<Button>().interactable = false;
                }
                break;
            case 12:
                _passiveLevelUpText[popUpIndex].text = "활성화 스킬 지속시간 " + passiveValue[popUpIndex].ToString() + "초 증가";
                _passiveLevelUpIncreaseText[popUpIndex].text = "구매 시 활성화 스킬 지속시간" + passiveValueIncrease[popUpIndex].ToString() + "초 상승";

                if (GameInstance.Instance.CurrentPassiveLevel[popUpIndex] >= 30)
                {
                    GameObject.Find("PassivePurchaseButton" + popUpIndex.ToString()).GetComponent<Button>().interactable = false;
                }
                break;
            case 13:
                _passiveLevelUpText[popUpIndex].text = "마나 회복 탭 수치" + passiveValue[popUpIndex].ToString() + " 감소";
                _passiveLevelUpIncreaseText[popUpIndex].text = "구매 시 마나 회복 탭 수치" + passiveValueIncrease[popUpIndex].ToString() + " 감소";

                PlayerController.Instance.tapMaxCount -= (int)passiveValueIncrease[popUpIndex];
                if (GameInstance.Instance.CurrentPassiveLevel[popUpIndex] >= 10)
                {
                    GameObject.Find("PassivePurchaseButton" + popUpIndex.ToString()).GetComponent<Button>().interactable = false;
                }
                break;
            case 14:
                _passiveLevelUpText[popUpIndex].text = "용병 레벨업 비용 " + passiveValue[popUpIndex].ToString() + " 감소";
                _passiveLevelUpIncreaseText[popUpIndex].text = "구매 시 용병 레벨업 비용 " + passiveValueIncrease[popUpIndex].ToString() + " 감소";

                for (int i = 0; i < 4; ++i)
                {
                    HiredSoldierPopUp.Instance._hiredSoldierlevelUpGold[popUpIndex] -= (int)passiveValueIncrease[popUpIndex];
                    HiredSoldierPopUp.Instance.SetHiredSoldierLevelUpText(popUpIndex, HiredSoldierPopUp.Instance._hiredSoldierlevelUpGold[popUpIndex]);
                }
                break;
            case 15:
                _passiveLevelUpText[popUpIndex].text = "플레이어 레벨업 비용 " + passiveValue[popUpIndex].ToString() + " 감소";
                _passiveLevelUpIncreaseText[popUpIndex].text = "구매 시 플레이어 레벨업 비용 " + passiveValueIncrease[popUpIndex].ToString() + " 감소";

                UpGradePopUp.Instance._playerLevelUpGold -= (int)passiveValueIncrease[popUpIndex];
                UpGradePopUp.Instance.SetPlayerLevelUpGoldText(UpGradePopUp.Instance. _playerLevelUpGold);
                break;
            case 16:
                _passiveLevelUpText[popUpIndex].text = "몬스터 체력 " + passiveValue[popUpIndex].ToString() + " 감소";
                _passiveLevelUpIncreaseText[popUpIndex].text = "구매 시 몬스터 체력 " + passiveValueIncrease[popUpIndex].ToString() + " 감소";
                break;
            case 17:
                _passiveLevelUpText[popUpIndex].text = "용병 포션 스킬 효율 " + passiveValue[popUpIndex].ToString() + " 증가";
                _passiveLevelUpIncreaseText[popUpIndex].text = "구매 시 용병 포션 스킬 효율 " + passiveValueIncrease[popUpIndex].ToString() + " 증가";

                if (GameInstance.Instance.CurrentPassiveLevel[popUpIndex] >= 15)
                {
                    GameObject.Find("PassivePurchaseButton" + popUpIndex.ToString()).GetComponent<Button>().interactable = false;
                }
                break;
            case 18:
                _passiveLevelUpText[popUpIndex].text = "미다스의 손 스킬 효율 " + passiveValue[popUpIndex].ToString() + " 증가";
                _passiveLevelUpIncreaseText[popUpIndex].text = "구매 시 미다스의 손 스킬 효율 " + passiveValueIncrease[popUpIndex].ToString() + " 증가";

                PlayerController.Instance.tapGold += (int)passiveValueIncrease[popUpIndex];

                if (GameInstance.Instance.CurrentPassiveLevel[popUpIndex] >= 10)
                {
                    GameObject.Find("PassivePurchaseButton" + popUpIndex.ToString()).GetComponent<Button>().interactable = false;
                }
                break;
            case 19:
                _passiveLevelUpText[popUpIndex].text = "잔인한 타격 크리티컬 확률 " + passiveValue[popUpIndex].ToString() + " 증가";
                _passiveLevelUpIncreaseText[popUpIndex].text = "구매 시 잔인한 타격 크리티컬 확률" + passiveValueIncrease[popUpIndex].ToString() + " 증가";

                PlayerController.Instance.criticalPercent += passiveValueIncrease[popUpIndex];

                if (GameInstance.Instance.CurrentPassiveLevel[popUpIndex] >= 30)
                {
                    GameObject.Find("PassivePurchaseButton" + popUpIndex.ToString()).GetComponent<Button>().interactable = false;
                }
                break;
        }
    }

    public void ReSizeButton()
    {
        if (_isResize == false)
        {
            _passivePopUpImage.GetComponent<RectTransform>().sizeDelta = new Vector2(700, 1000);
            _passivePopUpImage.transform.position += new Vector3(0, 350);
            _reSizeButton.transform.position += new Vector3(0, 300);
            _closeButton.transform.position += new Vector3(0, 300);
            _PassivePopUpText.transform.position += new Vector3(0, 300);
            _isResize = true;
        }
        else
        {
            _passivePopUpImage.GetComponent<RectTransform>().sizeDelta = new Vector2(700, 400);
            _passivePopUpImage.transform.position -= new Vector3(0, 350);
            _reSizeButton.transform.position -= new Vector3(0, 300);
            _closeButton.transform.position -= new Vector3(0, 300);
            _PassivePopUpText.transform.position -= new Vector3(0, 300);
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
