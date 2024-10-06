using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    public int enemyHealth = 150;
    [Header("Enemy Movement(For NavMeshAgent)")]
    private NavMeshAgent enemyAgent;
    public Transform playerTransform;
    public LayerMask enemyGroundLayer;
    public LayerMask playerLayer;
    [Header("Enemy Patrolling")]
    public Vector3 patrolPoint;
    public float patrolPointRange;
    public bool isPatrolling;
    [Header("Enemy Sight and Attack Range")]
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    [Header("Enemy Attack")]
    public float attackDelay = 3f;
    public bool isAttacking;
    public Transform attackPoint;
    public GameObject projectile;
    public float projectileSpeed = 20f;
    private Animator enemyAnimator;
    [Header("Particle Effect")]
    public ParticleSystem deathEffect;
    private void Start()
    {
        enemyAgent = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();
    }
    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);
        if (!playerInSightRange && !playerInAttackRange)
        {
            Patrolling();
            enemyAnimator.SetBool("Patrolling", true);
            enemyAnimator.SetBool("Chasing", false);
            enemyAnimator.SetBool("Attacking", false);
        }
        if (playerInSightRange && !playerInAttackRange)
        {
            ChasePlayer();
            enemyAnimator.SetBool("Patrolling", false);
            enemyAnimator.SetBool("Chasing", true);
            enemyAnimator.SetBool("Attacking", false);
        }
        if (playerInSightRange && playerInAttackRange)
        {
            AttackPlayer();
            enemyAnimator.SetBool("Patrolling", false);
            enemyAnimator.SetBool("Chasing", false);
            enemyAnimator.SetBool("Attacking", true);
        }

    }

    private void Patrolling()
    {
        if (!isPatrolling)
        {
            float randomZpos = Random.Range(-patrolPointRange, patrolPointRange);
            float randomXpos = Random.Range(-patrolPointRange, patrolPointRange);

            patrolPoint = new Vector3(transform.position.x + randomXpos, transform.position.y, transform.position.z + randomZpos);

            if (Physics.Raycast(patrolPoint, -transform.up, 2f, enemyGroundLayer))
            {
                isPatrolling = true;
            }
        }

        if (isPatrolling)
        {
            enemyAgent.SetDestination(patrolPoint);
        }

        Vector3 distanceToPatrolPoint = transform.position - patrolPoint;

        if (distanceToPatrolPoint.magnitude < 1f)
        {
            isPatrolling = false;
        }
    }
    private void ChasePlayer()
    {
        enemyAgent.SetDestination(playerTransform.position);
        Vector3 lookAtPosition = new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z);
        transform.LookAt(lookAtPosition);
    }
    private void AttackPlayer()
    {
        enemyAgent.SetDestination(transform.position);
        Vector3 lookAtPosition = new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z);
        transform.LookAt(lookAtPosition);

        if (!isAttacking)
        {
            StartCoroutine(ProjectileCoroutine());

            isAttacking = true;
            Invoke("ResetAttack", attackDelay);
        }
    }
    private IEnumerator ProjectileCoroutine()
    {
        yield return new WaitForSeconds(1f);
        Rigidbody rb = Instantiate(projectile, attackPoint.position, Quaternion.identity).GetComponent<Rigidbody>();
        rb.useGravity = false;
        yield return new WaitForSeconds(0.6f);
        rb.useGravity = true;
        rb.AddForce(transform.forward * projectileSpeed, ForceMode.Impulse);
    }

    private void ResetAttack()
    {
        isAttacking = false;
    }

    public void EnemyTakeDamage(int damage)
    {
        enemyHealth -= damage;
        if (enemyHealth <= 0)
        {
            EnemyDeath();
        }
        Debug.Log("Enemy take damage: " + damage);
    }

    private void EnemyDeath()
    {
        GameManager.Instance.AddKill();
        Destroy(gameObject);
        Instantiate(deathEffect, transform.position, Quaternion.identity);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
