using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Animator))]
public class PlayerController : UnitySingleton<PlayerController>
{
    enum ParticlePosition { ENEMY, PLAYERBOTTOM, PLAYERMIDDLE, HIREDSOLDIERS }

    [SerializeField] private ParticleSystem _attackParticle;
    [SerializeField] private ParticleSystem _criticalParticle;
    [SerializeField] private ParticleSystem[] _skillParticles;
    [SerializeField] private ParticleSystem _goldParticle;

    public float maxMp = 100.0f;
    public float mp = 0.0f;
    public float attackPower = 20.0f;
    public float skillPower = 1.5f;
    public float criticalPower;
    public float criticalPercent = 2.0f;
    public float addMpPerSec = 1.0f;
    public float addMpPerTap = 1.0f;
    public bool isCritical = false;
    public bool isAddGold = false;
    public int tapGold = 50;
    public int tapCount = 0;
    public int tapMaxCount = 10;

    private float _criticalMultiple = 1.5f;
    private float _attackEndTime = 0.25f;

    private Animator _myAnimator;
    private float _attackAfterTime = 0.0f;
    private float _delayTime = 1.0f;
    private float _mpAddTime = 0.0f;
    private bool _isAttack, _isAttackContinue;

    PlayerController()
    {
        criticalPower = attackPower * _criticalMultiple;
    }

    protected override void Awake()
    {
        base.Awake();

        _myAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        Click();
        AddMpPerSec();
    }

    void Click()
    {
        if (EventSystem.current.IsPointerOverGameObject() == false)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            {
                Attack();

                if (!_isAttack)
                {
                    SetIsAttack(true);
                }
                else
                {
                    _attackAfterTime = 0.0f;

                    if (!_isAttackContinue)
                    {
                        SetIsAttackContinue(true);
                    }
                }
            }
        }

        if(Input.GetKeyUp(KeyCode.Escape))
        {
            ScreenButtonUI.Instance.Quit();
        }

        _attackAfterTime += Time.deltaTime;
        if (_attackAfterTime > _attackEndTime)
        {
            SetIsAttack(false);
            SetIsAttackContinue(false);
            _attackAfterTime = 0.0f;
        }
    }

    void SetIsAttack(bool isAttack)
    {
        _isAttack = isAttack;
        _myAnimator.SetBool("Attack", isAttack);
    }

    void SetIsAttackContinue(bool isAttackContinue)
    {
        _isAttackContinue = isAttackContinue;
        _myAnimator.SetBool("AttackContinue", isAttackContinue);
    }

    void DamageToEnemy(float damage)
    {
        if (isAddGold)
        {
            GameController.Instance.gold += tapGold;
            GameController.Instance.saveGold += tapGold;
            Instantiate(_goldParticle, GameController.Instance.transform.position, _goldParticle.transform.rotation);
        }

        Enemy enemy = GameController.Instance.CurrentEnemy;
        if (enemy != null && enemy.gameObject.activeSelf == true && enemy.CurrentIsDead == false)
        {
            enemy.SufferDamage(damage);
            SpawnParticleToEnemy(_attackParticle, ParticlePosition.ENEMY);
        }
    }

    public void Attack()
    {
        if (isCritical)
        {
            int random = (int)Random.Range(0.0f, 10.0f);
            if (random <= criticalPercent)
            {
                DamageToEnemy(criticalPower);
            }
            else
            {
                DamageToEnemy(attackPower);
            }
        }
        else
        {
            DamageToEnemy(attackPower);
        }

        ++tapCount;
        if(tapCount > tapMaxCount)
        {
            AddMp(addMpPerTap);
            tapCount = 0;
        }
    }

    public void UseSkill(int skillIndex)
    {
        if (mp >= 20)
        {
            switch (skillIndex)
            {
                case 0:
                    MinusMp(20);
                    _myAnimator.SetTrigger("Skill0");
                    DamageToEnemy(attackPower * skillPower);
                    SpawnParticleToEnemy(_skillParticles[skillIndex], ParticlePosition.ENEMY);
                    StartCoroutine(SkillUI.Instance.SkillCoolDownTime(skillIndex, 10));
                    break;
                case 1:
                    MinusMp(20);
                    SpawnParticleToEnemy(_skillParticles[skillIndex], ParticlePosition.PLAYERBOTTOM);
                    StartCoroutine(SkillUI.Instance.AddCriticalPercent(10 + (int)PassivePopUp.Instance.passiveValue[12]));
                    StartCoroutine(SkillUI.Instance.SkillCoolDownTime(skillIndex, 10));
                    break;
                case 2:
                    if (GameController.Instance.CurrentHiredSoldiers[0] == null)
                        break;

                    MinusMp(20);
                    SpawnParticleToEnemy(_skillParticles[skillIndex], ParticlePosition.HIREDSOLDIERS);
                    StartCoroutine(SkillUI.Instance.AddHiredSolderSpeedUp(10 + (int)PassivePopUp.Instance.passiveValue[12]));
                    StartCoroutine(SkillUI.Instance.SkillCoolDownTime(skillIndex, 10));
                    break;
                case 3:
                    MinusMp(20);
                    SpawnParticleToEnemy(_skillParticles[skillIndex], ParticlePosition.PLAYERMIDDLE);
                    StartCoroutine(SkillUI.Instance.AddGold(10 + (int)PassivePopUp.Instance.passiveValue[12]));
                    StartCoroutine(SkillUI.Instance.SkillCoolDownTime(skillIndex, 10));
                    break;
                case 4:
                    MinusMp(maxMp);
                    DamageToEnemy(GameController.Instance.CurrentEnemy.CurrentMaxHp / 2.0f);
                    ScreenSliderUI.Instance.SetEnemyHpText(GameController.Instance.CurrentEnemy.CurrentMaxHp, GameController.Instance.CurrentEnemy.CurrentHp);
                    ScreenSliderUI.Instance.SetEnemyHpSlider(GameController.Instance.CurrentEnemy.CurrentMaxHp, GameController.Instance.CurrentEnemy.CurrentHp);
                    SpawnParticleToEnemy(_skillParticles[skillIndex], ParticlePosition.ENEMY);
                    StartCoroutine(SkillUI.Instance.SkillCoolDownTime(skillIndex, 3600.0f - PassivePopUp.Instance.passiveValue[5] * 60.0f));
                    break;
            }
        }
    }

    public void AddMpPerSec()
    {
        if (Time.time > _mpAddTime)
        {
            _mpAddTime = Time.time + _delayTime;
            AddMp(addMpPerSec);
        }
    }

    void SpawnParticleToEnemy(ParticleSystem particle, ParticlePosition particlePosition)
    {
        switch (particlePosition)
        {
            case ParticlePosition.ENEMY:
                {
                    Enemy enemy = GameController.Instance.CurrentEnemy;
                    if (enemy != null && enemy.gameObject.activeSelf == true)
                    {
                        Vector3 particlePos = enemy.transform.position + new Vector3(0.0f, 0.8f, -0.5f);
                        Instantiate(particle, particlePos, particle.transform.rotation);
                    }
                    break;
                }
            case ParticlePosition.PLAYERBOTTOM:
                {
                    Vector3 particlePos = transform.position;
                    Instantiate(particle, particlePos, particle.transform.rotation);
                    break;
                }
            case ParticlePosition.PLAYERMIDDLE:
                {
                    Vector3 particlePos = transform.position + new Vector3(0.0f, 0.5f, -0.3f);
                    Instantiate(particle, particlePos, particle.transform.rotation);
                    break;
                }
            case ParticlePosition.HIREDSOLDIERS:
                {
                    for (int i = 0; i < GameController.Instance.CurrentHiredSoldiers.Length; ++i)
                    {
                        Vector3 particlePos = GameController.Instance.CurrentHiredSoldiers[i].transform.position;
                        Instantiate(particle, particlePos, particle.transform.rotation);
                    }
                    break;
                }
        }
    }

    void SetPlayerMpUI()
    {
        ScreenSliderUI.Instance.SetPlayerMpSlider(maxMp, mp);
        ScreenSliderUI.Instance.SetPlayerMpText(maxMp, mp);
    }

    void AddMp(float addMp)
    {
        mp += addMp;
        if (mp >= maxMp)
        {
            mp = maxMp;
        }
        SetPlayerMpUI();
    }

    void MinusMp(float consumption)
    {
        mp -= consumption;
        if (mp <= 0)
        {
            mp = 0;
        }
        SetPlayerMpUI();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        StopAllCoroutines();
    }

    public void SetSkillParticleDuration(int skillIndex, float duration)
    {
        _skillParticles[skillIndex].Stop();
        var particleMain = _skillParticles[skillIndex].main;
        particleMain.duration = duration;
    }
}
