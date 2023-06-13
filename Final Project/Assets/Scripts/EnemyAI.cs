using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    Transform target;

    [HideInInspector]
    public NavMeshAgent agent;
    Animator anim;
    bool isDead = false;
    public bool canAttack = true;
    [SerializeField]
    float chaseDistance = 2f;

    [SerializeField]
    float turnSpeed = 5f;

    public float damageAmount = 35f;

    [SerializeField]
    float attackTime = 2f;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        if(distance > chaseDistance && !isDead)
        {
            ChasePlayer();
        }
        else if(canAttack && !PlayerHealth.singleton.isDead)
        {
            AttackPlayer();
        }
        else if(PlayerHealth.singleton.isDead)
        {
            DisableEnemy();
        }
    }

    public void EnemyDeathAnim()
    {
        isDead = true;
        canAttack = false;
        anim.SetTrigger("isDead");
    }

    void ChasePlayer()
    {
        agent.updateRotation = true;
        agent.SetDestination(target.position);
        anim.SetBool("isWalking", true);
        anim.SetBool("isAttacking", false);
    }

    void AttackPlayer()
    {
        agent.updateRotation = false;
        Vector3 direction = target.position - transform.position;
        direction.y = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), turnSpeed * Time.deltaTime);
        agent.updatePosition = false;
        anim.SetBool("isWalking", false);
        anim.SetBool("isAttacking", true);
        StartCoroutine(AttackTime());
    }

    void DisableEnemy()
    {
        canAttack = false;
        anim.SetBool("isWalking", false);
        anim.SetBool("isAttacking", false);
    }

    IEnumerator AttackTime()
    {
        canAttack = false;
        yield return new WaitForSeconds(0.5f);
        PlayerHealth.singleton.DamagePlayer(damageAmount);
        yield return new WaitForSeconds(attackTime);
        canAttack = true;
    }
}
