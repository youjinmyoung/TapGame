using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementPopUp : UnitySingleton<AchievementPopUp>
{
    [SerializeField] private Button _reSizeButton;
    [SerializeField] private Button _closeButton;
    [SerializeField] private Text _achievementPopUpText;
    [SerializeField] private Text _levelText;
    [SerializeField] private Text _goldText;
    [SerializeField] private Text _diamondText;
    [SerializeField] private Text _goalEnemyText;
    [SerializeField] private Text _goalBossText;
    [SerializeField] private Text _goalGoldRewardText;
    [SerializeField] private Text _goalStageText;
    [SerializeField] private Text _goalLevelText;
    [SerializeField] private Text _goalSumLevelText;

    public bool[] isAchivementClear = new bool[12];

    private Image _achievementsPopUp;

    public int goalEnemyCount = 10;
    public int goalBossCount = 5;
    public int goalGold = 5000;
    public int goalStage = 2;
    public int goalLevel = 3;
    public int goalSumLevel = 5;
    public int sumHiredSoldierLevel = 0;

    public bool isButton = false;
    private Vector3 _buttonUp = new Vector3(0, 80);

    private bool _isResize = false;

    private void Start()
    {
        GameObject achievementsPopUpObject = GameObject.Find("AchievementsPopUp");

        _achievementsPopUp = achievementsPopUpObject.GetComponent<Image>();
        _achievementsPopUp.gameObject.SetActive(false);
    }

    public void AchievementsButton()
    {
        int totalLevel = 0;
        _achievementsPopUp.gameObject.SetActive(true);
        if (isButton == true)
        {
            CloseButton();
        }
        else
        {
            UpGradePopUp.Instance.CloseButton();
            HiredSoldierPopUp.Instance.CloseButton();
            PassivePopUp.Instance.CloseButton();
            this.transform.position += _buttonUp;
            isButton = true;
        }
        SetPlayerInfo(UpGradePopUp.Instance.playerLevel, GameController.Instance.gold, GameInstance.Instance.CurrentDiamond);
        for (int i = 0; i < 4; ++i)
        {
            totalLevel += HiredSoldierPopUp.Instance.hiredSoldierLevel[i];
        }
        sumHiredSoldierLevel = totalLevel;
        SetText();
    }

    public void SetText()
    {
        _goalEnemyText.text = "일반 몬스터 " + goalEnemyCount.ToString() + "마리 처치하기";
        _goalBossText.text = "보스 몬스터 " + goalBossCount.ToString() + "마리 처치하기";
        _goalGoldRewardText.text = "골드 " + goalGold.ToString() + " 모으기";
        _goalStageText.text = goalStage.ToString() + " 스테이지 도달";
        _goalLevelText.text = goalLevel.ToString() + " 레벨 달성";
        _goalSumLevelText.text = "용병 합산 레벨" + goalSumLevel.ToString() + " 달성";
    }

    public void CloseButton()
    {
        _achievementsPopUp.gameObject.SetActive(false);
        if (this.transform.position.y > -10)
        {
            this.transform.position -= _buttonUp;
        }
        isButton = false;
    }

    public void KillEnemyReward()
    {
        if (GameController.Instance.CurrentDeadEnemyCount >= goalEnemyCount)
        {
            GameController.Instance.gold += 1000;
            GameController.Instance.saveGold += 1000;
            GameController.Instance.CurrentDeadEnemyCount = 0;
            GameInfoTextUI.Instance.SetGoldText(GameController.Instance.gold);
            goalEnemyCount += 10;
            _goalEnemyText.text = "일반 몬스터 " + goalEnemyCount.ToString() + "마리 처치하기";
        }
        SetPlayerInfo(UpGradePopUp.Instance.playerLevel, GameController.Instance.gold, GameInstance.Instance.CurrentDiamond);
    }

    public void KillBossReward()
    {
        if (GameController.Instance.CurrentDeadBossCount >= goalBossCount)
        {
            GameController.Instance.gold += 1500;
            GameController.Instance.saveGold += 1500;
            GameController.Instance.CurrentDeadBossCount = 0;
            GameInfoTextUI.Instance.SetGoldText(GameController.Instance.gold);
            goalBossCount += 5;
            _goalBossText.text = "보스 몬스터 " + goalBossCount.ToString() + "마리 처치하기";
        }
        SetPlayerInfo(UpGradePopUp.Instance.playerLevel, GameController.Instance.gold, GameInstance.Instance.CurrentDiamond);
    }

    public void GatherGoldReward()
    {
        if (GameController.Instance.gold >= goalGold)
        {
            GameController.Instance.gold += 1000;
            GameInfoTextUI.Instance.SetGoldText(GameController.Instance.gold);
            goalGold += 5000;
            _goalGoldRewardText.text = "골드 " + goalGold.ToString() + " 모으기";
        }
        SetPlayerInfo(UpGradePopUp.Instance.playerLevel, GameController.Instance.gold, GameInstance.Instance.CurrentDiamond);
    }

    public void ReachStageReward()
    {
        if (GameController.Instance.CurrentStage == goalStage)
        {
            GameController.Instance.gold += 1000;
            GameInfoTextUI.Instance.SetGoldText(GameController.Instance.gold);
            goalStage += 2;
            _goalStageText.text = goalStage.ToString() + " 스테이지 도달";
        }
        SetPlayerInfo(UpGradePopUp.Instance.playerLevel, GameController.Instance.gold, GameInstance.Instance.CurrentDiamond);
    }

    public void ReachLevelReward()
    {
        if (UpGradePopUp.Instance.playerLevel >= goalLevel)
        {
            GameController.Instance.gold += 1000;
            GameInfoTextUI.Instance.SetGoldText(GameController.Instance.gold);
            goalLevel += 3;
            _goalLevelText.text = goalLevel.ToString() + " 레벨 달성";
        }
        SetPlayerInfo(UpGradePopUp.Instance.playerLevel, GameController.Instance.gold, GameInstance.Instance.CurrentDiamond);
    }

    public void SumHiredSoldierLevelReward()
    {
        if (sumHiredSoldierLevel >= goalSumLevel)
        {
            GameController.Instance.gold += 1500;
            GameInfoTextUI.Instance.SetGoldText(GameController.Instance.gold);
            goalSumLevel += 5;
            _goalSumLevelText.text = "용병 합산 레벨" + goalSumLevel.ToString() + " 달성";
        }
        SetPlayerInfo(UpGradePopUp.Instance.playerLevel, GameController.Instance.gold, GameInstance.Instance.CurrentDiamond);
    }



    public void ReSizeButton()
    {
        if (_isResize == false)
        {
            _achievementsPopUp.GetComponent<RectTransform>().sizeDelta = new Vector2(700, 1000);
            _achievementsPopUp.transform.position += new Vector3(0, 350);
            _reSizeButton.transform.position += new Vector3(0, 300);
            _closeButton.transform.position += new Vector3(0, 300);
            _achievementPopUpText.transform.position += new Vector3(0, 300);
            _isResize = true;
        }
        else
        {
            _achievementsPopUp.GetComponent<RectTransform>().sizeDelta = new Vector2(700, 400);
            _achievementsPopUp.transform.position -= new Vector3(0, 350);
            _reSizeButton.transform.position -= new Vector3(0, 300);
            _closeButton.transform.position -= new Vector3(0, 300);
            _achievementPopUpText.transform.position -= new Vector3(0, 300);
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
