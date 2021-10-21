
using UnityEngine;
using UnityEngine.AI;

public class YetiAI : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public float health;

    public Animator animator;

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

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void FixedUpdate()
    {
        // Vector3 fwd = transform.TransformDirection(Vector3.forward);

        //if (Physics.Raycast(transform.position, fwd, sightRange))
        //{
        //    playerInSightRange = true;
        //}
        //else
        //{
        //    playerInSightRange = false;
        //}

        //if (Physics.Raycast(transform.position, fwd, attackRange))
        //{
        //    playerInAttackRange = true;
        //}
        //else
        //{
        //    playerInAttackRange = false;
        //}

        //Check for sight and attack range
        //playerInCloseSightRange = Physics.CheckSphere(transform.position, (sightRange - (sightRange * 0.8f)), whatIsPlayer);
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        //if ((playerInSightRange || playerInCloseSightRange) && !playerInAttackRange) ChasePlayer();
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
        changeAnim("isAttacking", false);

        
        // Debug.Log(animator.IsInTransition(0)); check if animation is transitioning


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
        changeAnim("isChasing", true);
        changeAnim("isAttacking", false);
        agent.SetDestination(player.position);
        
    }

    bool AnimatorIsPlaying()
    {
        Debug.Log("Playing? " + (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1));
        return animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1;
    }

    bool AnimatorIsPlaying(string stateName)
    {
        return AnimatorIsPlaying() && animator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }

    private void IdleForAnimation()
    {
        if (!AnimatorIsPlaying("Base Layer.YetiAttack"))
        {
            //changeAnim("isChasing", false);
            //changeAnim("isAttacking", false);
        }
    }

    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            ///Attack code here
            //Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            //rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            //rb.AddForce(transform.up * 8f, ForceMode.Impulse);
            ///End of attack code

            alreadyAttacked = true;
            changeAnim("isChasing", false);
            changeAnim("isAttacking", true);
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
        else
        {
            IdleForAnimation();
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
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
