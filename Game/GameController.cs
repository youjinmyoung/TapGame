using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : UnitySingleton<GameController>
{
    [SerializeField] private ParticleSystem _reincarnationParticle;
    [SerializeField] private ParticleSystem _deadParticle;
    [SerializeField] private ParticleSystem _bossDeadParticle;
    [SerializeField] private Enemy[] _enemyDefaults;
    [SerializeField] private HiredSoldier[] _hiredSoldierDefaults;

    public Enemy CurrentEnemy { get; set; }
    public HiredSoldier[] CurrentHiredSoldiers { get; set; }
    public int CurrentDeadEnemyCount { get; set; }
    public int CurrentDeadBossCount { get; set; }
    public int CurrentStage { get; set; }

    public float bossMaxCoolTime = 10.0f;
    public int gold = 0;
    public int saveGold = 0;

    private int _deadEnemyForNextStage = 5;
    private int _deadEnemyInStage = 0;

    private Transform _hiredSoldierSpawnTransforms;
    private float bossProgressTime = 0.0f;
    private bool _isBossSpawn = false;


    public HiredSoldier[] CurrentHiredSoldierDefaults
    {
        get
        {
            return _hiredSoldierDefaults;
        }
    }

    GameController()
    {
        CurrentHiredSoldiers = new HiredSoldier[4];

        CurrentStage = 1;
    }

    protected override void Awake()
    {
        base.Awake();

        JsonHelper.Load();
       

        _hiredSoldierSpawnTransforms = transform.GetChild(0);

        // 해상도 조정(되는지 확인해야댐)
        Screen.SetResolution(720, 1280, true);
    }

    private void Start()
    {
        if(GameInstance.Instance.isReincarnation)
        {
            Instantiate(_reincarnationParticle, new Vector3(0.0f, 0.0f, -6.5f), Quaternion.identity);
        }
    }

    private void Update()
    {
        if (CurrentEnemy == null)
            SpawnRandomEnemy();
    }

    void SpawnEnemy(int enemyIndex)
    {
        float enemyHp;
        int enemyGold;

        if (_isBossSpawn)
        {
            enemyHp = 500.0f;
            enemyGold = 300;

            enemyHp += 1000.0f * (CurrentStage - 1);
            enemyGold *= CurrentStage;

            SpawnEnemy(enemyIndex, enemyHp, enemyGold);
            ScreenSliderUI.Instance.SetActiveBossCoolTimeSlider(true);
            bossProgressTime = 0.0f;
            CurrentEnemy.CurrentIsBoss = true;
            CurrentEnemy.transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
            _isBossSpawn = false;

            StartCoroutine(BossCoolTime());
        }
        else
        {
            enemyHp = 100.0f;
            enemyGold = 50;

            enemyHp += 50.0f * (CurrentStage - 1)  - PassivePopUp.Instance.passiveValue[16];
            enemyGold *= CurrentStage;

            SpawnEnemy(enemyIndex, enemyHp, enemyGold);
        }
    }

    void SpawnEnemy(int enemyIndex, float hp, int gold)
    {
        CurrentEnemy = Instantiate(_enemyDefaults[enemyIndex], transform.position, _enemyDefaults[enemyIndex].transform.rotation);
        CurrentEnemy.CurrentMaxHp = hp;
        CurrentEnemy.CurrentGold = gold;
        ScreenSliderUI.Instance.SetEnemyNameText(enemyIndex);
    }

    void SpawnRandomEnemy()
    {
        int randomIndex = Random.Range(0, _enemyDefaults.Length);
        SpawnEnemy(randomIndex);
    }

    public void KillEnemy(int enemyGold)
    {
        if (CurrentEnemy.CurrentIsBoss)
        {
            gold += enemyGold + (int)PassivePopUp.Instance.passiveValue[4];
            saveGold += enemyGold + (int)PassivePopUp.Instance.passiveValue[4];
            ++CurrentDeadBossCount;
            bossProgressTime = 0.0f;
            IncreaseStage();
            ScreenSliderUI.Instance.SetActiveBossCoolTimeSlider(false);
            Instantiate(_bossDeadParticle, transform.position, _deadParticle.transform.rotation);

            StopAllCoroutines();
        }
        else
        {
            gold += enemyGold + (int)PassivePopUp.Instance.passiveValue[3];
            saveGold += enemyGold + (int)PassivePopUp.Instance.passiveValue[3];
            ++CurrentDeadEnemyCount;
            ++_deadEnemyInStage;
            if (_deadEnemyInStage == _deadEnemyForNextStage)
            {
                _isBossSpawn = true;
            }
            Instantiate(_deadParticle, transform.position, _deadParticle.transform.rotation);
        }

        GameInfoTextUI.Instance.SetGoldText(gold);
        GameInfoTextUI.Instance.SetDeadEnemyInStage(_deadEnemyInStage);
        UpGradePopUp.Instance.SetPlayerInfo(UpGradePopUp.Instance.playerLevel, gold, GameInstance.Instance.CurrentDiamond);
        HiredSoldierPopUp.Instance.SetPlayerInfo(UpGradePopUp.Instance.playerLevel, gold, GameInstance.Instance.CurrentDiamond);
        AchievementPopUp.Instance.SetPlayerInfo(UpGradePopUp.Instance.playerLevel, gold, GameInstance.Instance.CurrentDiamond);
        PassivePopUp.Instance.SetPlayerInfo(UpGradePopUp.Instance.playerLevel, gold, GameInstance.Instance.CurrentDiamond);

        Destroy(CurrentEnemy.gameObject);
    }

    public void SpawnHiredSoldier(int soldierIndex)
    {
        Transform hiredSoldierSpawnTransform = _hiredSoldierSpawnTransforms.GetChild(soldierIndex);
        CurrentHiredSoldiers[soldierIndex] = Instantiate(_hiredSoldierDefaults[soldierIndex], hiredSoldierSpawnTransform.position, hiredSoldierSpawnTransform.rotation) as HiredSoldier;
        CurrentHiredSoldiers[soldierIndex].attackPower += PassivePopUp.Instance.passiveValue[10];
        CurrentHiredSoldiers[soldierIndex].attackSpeed -= PassivePopUp.Instance.passiveValue[11];
    }

    public void IncreaseStage()
    {
        ++CurrentStage;
        _deadEnemyInStage = 0;
        SceneController.Instance.NextStage(CurrentStage);
        GetSkill();

        GameInfoTextUI.Instance.SetStageText(CurrentStage);
        ScreenButtonUI.Instance.bossAppearanceButton.gameObject.SetActive(false);
        GameInfoTextUI.Instance._deadEnemyInStageText.gameObject.SetActive(true);
    }

    public void AppearanceBoss()
    {
        Destroy(CurrentEnemy.gameObject);
        _isBossSpawn = true;
        _deadEnemyInStage = 0;
    }

    public void GetSkill()
    {
        if (CurrentStage > 1)
        {
            int skillIndex = CurrentStage - 2;
            if (skillIndex > 4)
                return;

            SkillUI.Instance.skillButton[skillIndex].interactable = true;
            SkillUI.Instance.isAvailableSkill[skillIndex] = true;
            SkillUI.Instance.skillImage[skillIndex].sprite = Resources.Load<Sprite>("UI/skill" + (skillIndex).ToString());
            UpGradePopUp.Instance.skillPurchaseButton[skillIndex].interactable = true;
        }
    }

    public float CalculateHiredSoldiersPower()
    {
        float power = 0.0f;

        for (int i = 0; i < 4; ++i)
        {
            if (CurrentHiredSoldiers[i] != null)
            {
                power += CurrentHiredSoldiers[i].attackPower;
            }
        }

        return power;
    }

    IEnumerator BossCoolTime()
    {
        while (bossProgressTime < bossMaxCoolTime)
        {
            bossProgressTime += Time.deltaTime;
            ScreenSliderUI.Instance.SetBossCoolTimeSlider(bossMaxCoolTime, bossMaxCoolTime - bossProgressTime);

            yield return new WaitForFixedUpdate();
        }

        ScreenSliderUI.Instance.SetActiveBossCoolTimeSlider(false);
        ScreenButtonUI.Instance.bossAppearanceButton.gameObject.SetActive(true);
        GameInfoTextUI.Instance._deadEnemyInStageText.gameObject.SetActive(false);
        Destroy(CurrentEnemy.gameObject);
        bossProgressTime = 0.0f;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        StopAllCoroutines();
    }
}
