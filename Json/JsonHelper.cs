using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameInfo
{
    // GameControllerInfo
    public float bossMaxCoolTime;
    public int gold;
    public int deadEnemyCount;
    public int deadBossCount;
    public int stage;
    public int spawnedHiredSoldier;

    // GameInstanceInfo
    public int diamond;
    public int[] passiveLevel = new int[20];
    public float coolDownTime;

    // UIInfo
    public int[] skillLevel = new int[5];
    public int playerLevel;
    public int[] hiredSoldierLevel = new int[4];
    public bool[] isAchivementClear = new bool[12];
    public bool[] isAvailableSkill = new bool[5];
    public int goalEnemyCount;
    public int goalBossCount;
    public int goalGold;
    public int saveGold;
    public int goalStage;
    public int goalLevel;
    public int goalSumLevel;
    public int sumHiredSoldierLevel;

    // SceneInfo
    public int backIndex;
}


public static class JsonHelper 
{
    static string filePath = Path.Combine(Application.persistentDataPath, "data.json");
    // static string filePath = Application.dataPath + "/data.json";

    public static void Save()
    {
        GameInfo gameInfo = new GameInfo();

        gameInfo.bossMaxCoolTime = GameController.Instance.bossMaxCoolTime;
        gameInfo.gold = GameController.Instance.gold;
        gameInfo.saveGold = GameController.Instance.saveGold;
        gameInfo.deadEnemyCount = GameController.Instance.CurrentDeadEnemyCount;
        gameInfo.deadBossCount = GameController.Instance.CurrentDeadBossCount;
        gameInfo.stage = GameController.Instance.CurrentStage;

        gameInfo.diamond = GameInstance.Instance.CurrentDiamond;
        gameInfo.passiveLevel = GameInstance.Instance.CurrentPassiveLevel;
        gameInfo.coolDownTime = GameInstance.Instance.coolDownTime;

        gameInfo.skillLevel = UpGradePopUp.Instance.skillLevel;
        gameInfo.playerLevel = UpGradePopUp.Instance.playerLevel;
        gameInfo.hiredSoldierLevel = HiredSoldierPopUp.Instance.hiredSoldierLevel;
        gameInfo.isAchivementClear = AchievementPopUp.Instance.isAchivementClear;
        gameInfo.isAvailableSkill = SkillUI.Instance.isAvailableSkill;

        gameInfo.goalEnemyCount = AchievementPopUp.Instance.goalEnemyCount;
        gameInfo.goalBossCount = AchievementPopUp.Instance.goalBossCount;
        gameInfo.goalGold = AchievementPopUp.Instance.goalGold;
        gameInfo.goalStage = AchievementPopUp.Instance.goalStage;
        gameInfo.goalLevel = AchievementPopUp.Instance.goalLevel;
        gameInfo.goalSumLevel = AchievementPopUp.Instance.goalSumLevel;
        gameInfo.sumHiredSoldierLevel = AchievementPopUp.Instance.sumHiredSoldierLevel;

        gameInfo.backIndex = SceneController.Instance.backIndex;
        
        string saveInfo = JsonUtility.ToJson(gameInfo);

        File.WriteAllText(filePath, saveInfo);
    }

    public static void Load()
    {
        if(File.Exists(filePath))
        {
            string savedInfo = File.ReadAllText(filePath);
            GameInfo gameInfo = JsonUtility.FromJson<GameInfo>(savedInfo);

            GameController.Instance.bossMaxCoolTime = gameInfo.bossMaxCoolTime;
            GameController.Instance.gold = gameInfo.gold;
            GameController.Instance.saveGold = gameInfo.saveGold;
            GameController.Instance.CurrentDeadEnemyCount = gameInfo.deadEnemyCount;
            GameController.Instance.CurrentDeadBossCount = gameInfo.deadBossCount;
            GameController.Instance.CurrentStage = gameInfo.stage;

            GameInstance.Instance.CurrentDiamond = gameInfo.diamond;
            GameInstance.Instance.CurrentPassiveLevel = gameInfo.passiveLevel;
            GameInstance.Instance.coolDownTime = gameInfo.coolDownTime;

            UpGradePopUp.Instance.skillLevel = gameInfo.skillLevel;
            UpGradePopUp.Instance.playerLevel = gameInfo.playerLevel;
            HiredSoldierPopUp.Instance.hiredSoldierLevel = gameInfo.hiredSoldierLevel;
            AchievementPopUp.Instance.isAchivementClear = gameInfo.isAchivementClear;
            SkillUI.Instance.isAvailableSkill = gameInfo.isAvailableSkill;

            AchievementPopUp.Instance.goalEnemyCount = gameInfo.goalEnemyCount;
            AchievementPopUp.Instance.goalBossCount = gameInfo.goalBossCount;
            AchievementPopUp.Instance.goalGold = gameInfo.goalGold;
            AchievementPopUp.Instance.goalStage = gameInfo.goalStage;
            AchievementPopUp.Instance.goalLevel = gameInfo.goalLevel;
            AchievementPopUp.Instance.goalSumLevel = gameInfo.goalSumLevel;
            AchievementPopUp.Instance.sumHiredSoldierLevel = gameInfo.sumHiredSoldierLevel;

            SceneController.Instance.backIndex = gameInfo.backIndex;
        }
    }

    public static void Reincarnation()
    {
        GameInfo gameInfo = new GameInfo();

        gameInfo.bossMaxCoolTime = 10.0f;
        gameInfo.gold = 0;
        gameInfo.saveGold = GameController.Instance.saveGold;
        gameInfo.deadEnemyCount = 0;
        gameInfo.deadBossCount = 0;
        gameInfo.stage = 1;

        gameInfo.diamond = GameInstance.Instance.CurrentDiamond;
        gameInfo.passiveLevel = GameInstance.Instance.CurrentPassiveLevel;
        gameInfo.coolDownTime = GameInstance.Instance.coolDownTime;


        gameInfo.skillLevel = new int[5];
        gameInfo.playerLevel = 1;
        gameInfo.hiredSoldierLevel = new int[4];
        gameInfo.isAchivementClear = new bool[12];
        gameInfo.isAvailableSkill = new bool[5];

        gameInfo.goalEnemyCount = AchievementPopUp.Instance.goalEnemyCount;
        gameInfo.goalBossCount = AchievementPopUp.Instance.goalBossCount;
        gameInfo.goalGold = AchievementPopUp.Instance.goalGold;
        gameInfo.goalStage = AchievementPopUp.Instance.goalStage;
        gameInfo.goalLevel = AchievementPopUp.Instance.goalLevel;
        gameInfo.goalSumLevel = AchievementPopUp.Instance.goalSumLevel;
        gameInfo.sumHiredSoldierLevel = AchievementPopUp.Instance.sumHiredSoldierLevel;

        gameInfo.backIndex = 0;

        string saveInfo = JsonUtility.ToJson(gameInfo);

        File.WriteAllText(filePath, saveInfo);
    }

    public static void Reset()
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            File.Delete(filePath + ".meta");

            GameInstance.Instance.CurrentDiamond = 0;
            GameInstance.Instance.CurrentPassiveLevel = new int[3];

            SceneController.Instance.ResetScene();
        }
    }
}
