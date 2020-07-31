using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class HiredSoldier : MonoBehaviour
{
    [SerializeField] private ParticleSystem _attackParticle;

    public float attackSpeed = 3.0f;
    public float attackPower = 1.0f;

    private float _timeAfterAtatck = 0.0f;
    private Animator _myAnimator;

    private void Awake()
    {
        _myAnimator = GetComponent<Animator>();
        _myAnimator.speed = 1.5f;
    }

    private void Update()
    {
        if (_timeAfterAtatck > attackSpeed)
        {
            Attack();
            _myAnimator.SetTrigger("Attack");
        }

        _timeAfterAtatck += Time.deltaTime;
    }

    public void Attack()
    {
        Enemy enemyObject = GameController.Instance.CurrentEnemy;
        if (enemyObject && enemyObject.gameObject.activeSelf == true)
        {
            enemyObject.SufferDamage(attackPower);

            Vector3 enemyPos = enemyObject.transform.position + new Vector3(0.0f, 0.5f, -0.2f);
            Instantiate(_attackParticle, enemyPos, Quaternion.identity);
        }

        _timeAfterAtatck = 0.0f;
    }
}
