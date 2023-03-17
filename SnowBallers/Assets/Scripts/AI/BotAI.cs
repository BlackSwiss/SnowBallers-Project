using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Tutorial used: https://www.youtube.com/watch?v=UjkSFoLxesw&ab_channel=Dave%2FGameDevelopment
//

public class BotAI : MonoBehaviour
{
    public NavMeshAgent agent;

    private Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public NetworkPlayerSpawner networkPlayerSpawner;
    public List<GameObject> players;

    //Dodging
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject snowball;

    //States
    public float sightRange;
    public float attackRange;
    public bool playerInSightRange;
    public bool playerInAttackRange;

    //IK_Character Model Stuff
    public GameObject model;
    public bool lookRotation = true;
    private Transform modelTransform;
    private Animator modelAnimator;

    void Start()
    {
        modelTransform = model.transform;
        modelAnimator = model.GetComponent<Animator>();
        players = networkPlayerSpawner.getPlayers();
        
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Check if player is in Sight
    private void Update()
    {
        if (players.Count > 0)
        {
            player = players[0].transform;
            Debug.Log("Player found" + player.transform.position.x + player.transform.position.z);
        }
        

        playerInSightRange  = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange)
        {
            Debug.Log("Bot Dodging");
            Dodging();
        }
        if (playerInSightRange && !playerInAttackRange)
        {
            Debug.Log("Bot Chasing");
            ChasePlayer();
        }
        if (playerInAttackRange && playerInSightRange)
        {
            Debug.Log("Bot Attacking");
            AttackPlayer();
        }
    }

    private void Dodging()
    {
        if (!walkPointSet)
            SearchWalkPoint(); 

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
            modelAnimator.SetBool("Move", true);
        }

        //Calculate distance to walkpoint
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //walkpoint reached
        if(distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
            modelAnimator.SetBool("Move", false);
            Vector3 walkPointNoY = new Vector3(transform.position.x, modelTransform.position.y, transform.position.z);
            modelTransform.position = walkPointNoY;
        }  
    }

    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        //Search for random point on map within bounds
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;

            if(lookRotation)
                transform.LookAt(new Vector3(walkPoint.x, transform.position.y, walkPoint.z));
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        //Make sure bot doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            alreadyAttacked = true;

            //Attack code ***need to write AIs own snowball code
            Rigidbody rb = Instantiate(snowball, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 16f, ForceMode.Impulse);
            rb.AddForce(transform.up * 16f, ForceMode.Impulse);

            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    //visualize bot sight range
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
