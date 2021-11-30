
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public float health;

    public Animator animator;

    [SerializeField] private float damage;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    //public GameObject projectile;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private bool dead = false;

    public TraumaInducer inducer;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void FixedUpdate()
    {
        if (dead)
        {
            agent.SetDestination(transform.position);
            return;
        }

        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);


        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();
    }

    private void changeAnim(string state, bool tOrF)
    {
        animator.SetBool(state, tOrF);
    }

    private void Patroling()
    {
        changeAnim("isChasing", false);

        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        if (alreadyAttacked)
        {
            return;
        }


        changeAnim("isChasing", true);
        agent.SetDestination(player.position);

    }

    private bool AnimatorIsFinished()
    {
        return animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1;
    }


    private float GetAnimationTime(string clipName)
    {
        float cTime = 0f;

        AnimationClip[] arrclip = GetComponent<Animator>().runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in arrclip)
        {
            if (clip.name.Contains(clipName))
            {
                cTime = clip.length;
            }
        }

        return cTime;
    }

    private void AttackDirectionChange()
    {
        Transform cam = GameObject.FindWithTag("MainCamera").transform;
        Quaternion from = cam.rotation;
        Quaternion to = new Quaternion(from.x + 2, from.y, from.z, from.w);
        float speed = 0.1f;
        cam.rotation = Quaternion.Lerp(from, to, Time.time * speed);
    }

    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);


        transform.LookAt(player);

        changeAnim("isAttacking", false);
        changeAnim("isChasing", false);

        if (!alreadyAttacked)
        {
            alreadyAttacked = true;
            animator.Play("Attack");
            StartCoroutine(DealDamange());
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private IEnumerator DealDamange()
    {
        float attackDelay = GetAnimationTime("Attack");
        if (transform.name.Contains("Zombie"))
        {
            attackDelay = attackDelay * 0.5f;
        }
        else
        {
            attackDelay = attackDelay * 0.65f;
        }
        //else if (transform.name.Contains("Wolf"))
        //{
        //    attackDelay = attackDelay * 0.65f;
        //}
        //else if (transform.name.Contains("Werewolf"))
        //{
        //    attackDelay = attackDelay * 0.65f;
        //}
        //else if (transform.name.Contains("Yeti"))
        //{
        //    attackDelay = attackDelay * 0.65f;
        //}
        yield return new WaitForSeconds(attackDelay);
        float distance = Vector3.Distance(player.position, transform.position);
        if (distance <= attackRange + 1 && !dead)
        {
            if (inducer != null)
            {
                StartCoroutine(inducer.Explode());
                //AttackDirectionChange();
            }
            player.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(float damage)
    {
        if (dead)
        {
            return;
        }

        health -= damage;

        if (health <= 0)
        {
            dead = true;
            animator.Play("Death");
            
            if (transform.name.Contains("Yeti"))
            {
                GetComponent<NavMeshAgent>().baseOffset = -0.2f;
            }
            GetComponent<Rigidbody>().detectCollisions = false;

            //Invoke(nameof(DestroyEnemy), 3f);
        }
    }
    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
