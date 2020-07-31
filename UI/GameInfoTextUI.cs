using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameInfoTextUI : UnitySingleton<GameInfoTextUI>
{
    [SerializeField] private Text _attackPowerText;
    [SerializeField] private Text _stageText;
    [SerializeField] private Text _beforeStageText;
    [SerializeField] private Text _nextStageText;
    [SerializeField] private Text _levelText;
    [SerializeField] private Text _goldText;
    [SerializeField] private Text _diamondText;
    [SerializeField] public Text _deadEnemyInStageText;

    private void Start()
    {
        SetAttackPowerText(PlayerController.Instance.attackPower);
        SetLevelText(UpGradePopUp.Instance.playerLevel);
        SetGoldText(GameController.Instance.gold);
        SetStageText(GameController.Instance.CurrentStage);
        SetDiamondText(GameInstance.Instance.CurrentDiamond);
    }

    public void SetAttackPowerText(float attackPower)
    {
        _attackPowerText.text = attackPower.ToString() + "\n탭 대미지";
    }

    public void SetStageText(int stage)
    {
        _stageText.text = stage.ToString();
        _beforeStageText.text = (stage - 1).ToString();
        _nextStageText.text = (stage + 1).ToString();
    }
    public void SetLevelText(int level)
    {
        _levelText.text = "Lv " + level.ToString();
    }

    public void SetGoldText(int gold)
    {
        _goldText.text = gold.ToString();
    }

    public void SetDiamondText(int diamond)
    {
        _diamondText.text = diamond.ToString();
    }

    public void SetDeadEnemyInStage(int deadEnemyInStage)
    {
        _deadEnemyInStageText.text = deadEnemyInStage.ToString() + "/5";
    }
}
