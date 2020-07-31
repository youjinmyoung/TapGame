using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private Material _defaultMaterial;
    [SerializeField] private Material _fadeMaterial;
    [SerializeField] private Material _defaultWeaponMaterial;
    [SerializeField] private Material _fadeWeaponMaterial;

    // 외부에서 수정되는 변수
    public float CurrentMaxHp { get; set; }
    public float CurrentHp { get; set; }
    public int CurrentGold { get; set; }
    public bool CurrentIsBoss { get; set; }
    public bool CurrentIsDead { get; private set; }

    private Animator _myAnimator;
    private SkinnedMeshRenderer[] _mySkinnedMeshRenderers;
    private float _alpha = 1.0f;

    private void Awake()
    {
        _myAnimator = GetComponent<Animator>();
        _myAnimator.speed = 2; // 임시 처리

        _mySkinnedMeshRenderers = transform.GetComponentsInChildren<SkinnedMeshRenderer>();
        _mySkinnedMeshRenderers[0].material = _defaultMaterial;
        if (_defaultWeaponMaterial != null)
        {
            if (_mySkinnedMeshRenderers[1] != null)
            {
                _mySkinnedMeshRenderers[1].material = _defaultWeaponMaterial;
            }
        }

        CurrentIsDead = false;
    }

    private void Start()
    {
        CurrentHp = CurrentMaxHp;
        SetEnemyHpUI();
    }

    public void SufferDamage(float damage)
    {
        if (CurrentHp > 0.0f)
        {
            CurrentHp -= damage;
            SetEnemyHpUI();
            Beaten();
            if (CurrentHp <= 0.0f)
            {
                CurrentHp = 0.0f;
                SetEnemyHpUI();
                _myAnimator.SetBool("Dead", true);
                CurrentIsDead = true;

                _mySkinnedMeshRenderers[0].material = _fadeMaterial;
                if (_fadeWeaponMaterial != null)
                {
                    if (_mySkinnedMeshRenderers[1] != null)
                    {
                        _mySkinnedMeshRenderers[1].material = _fadeWeaponMaterial;
                    }
                }

                StartCoroutine(DeadFadeOut());
            }
        }
        GameInfoTextUI.Instance.SetGoldText(GameController.Instance.gold);
    }

    public void Beaten()
    {
        _myAnimator.SetTrigger("Beaten");
    }

    void SetEnemyHpUI()
    {
        ScreenSliderUI.Instance.SetEnemyHpSlider(CurrentMaxHp, CurrentHp);
        ScreenSliderUI.Instance.SetEnemyHpText(CurrentMaxHp, CurrentHp);
    }

    IEnumerator DeadFadeOut()
    {
        while (_alpha > 0)
        {
            _alpha -= Time.deltaTime * 3.0f;

            foreach (SkinnedMeshRenderer skinnedMeshRenderer in _mySkinnedMeshRenderers)
            {
                foreach (Material meshMaterial in skinnedMeshRenderer.materials)
                {
                    Color meshColor = meshMaterial.color;
                    meshMaterial.color = new Color(meshColor.r, meshColor.g, meshColor.b, _alpha);
                }
            }

            yield return new WaitForFixedUpdate();
        }

        GameController.Instance.KillEnemy(CurrentGold);
    }
}
