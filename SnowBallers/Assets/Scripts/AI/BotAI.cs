using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Tutorial used: https://www.youtube.com/watch?v=UjkSFoLxesw&ab_channel=Dave%2FGameDevelopment
//

public class BotAI : MonoBehaviour
{
    public NavMeshAgent agent;

    [SerializeField]
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
    private Transform modelTransform;
    private Animator modelAnimator;
    public float AttackAnimationDelay = 2;

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

        if(alreadyAttacked)
        {
            Debug.Log("Bot Dodging");
            modelAnimator.SetBool("Throw", false);
            modelAnimator.SetBool("Move", false);
            Dodging();
        }
        else
        {
            if(playerInAttackRange)
            {
                Debug.Log("Bot Attacking");
                modelAnimator.SetBool("Throw", true);
                Invoke(nameof(AttackPlayer), AttackAnimationDelay);
            }
            else if(playerInSightRange)
            {
                Debug.Log("Bot Chasing");
                modelAnimator.SetBool("Move", true);
                ChasePlayer();
            }
            else
            {
                Debug.Log("Bot Dodging");
                modelAnimator.SetBool("Move", false);
                Dodging();
            }
        }
    }

    private void Dodging()
    {
        if (!walkPointSet)
            SearchWalkPoint(); 

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        //Calculate distance to walkpoint
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //walkpoint reached
        if(distanceToWalkPoint.magnitude < 0.1f)
        {
            walkPointSet = false;
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

            //Make bot look at player
            transform.LookAt(new Vector3(walkPoint.x, transform.position.y, walkPoint.z));
        }
    }

    private void ChasePlayer()
    {
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        //Store old position and rotation to reset after attack
        Vector3 oldPosition = transform.position;
        Quaternion oldRotation = transform.rotation;

        //Make sure bot doesn't move
        //agent.SetDestination(transform.position);

        //Make bot look at player
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));

        if (!alreadyAttacked)
        {
            alreadyAttacked = true;

            //Attack code ***need to write AIs own snowball code
            GameObject newSnowball = Instantiate(snowball, transform.position, Quaternion.identity);
            Rigidbody rb = newSnowball.GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 16f, ForceMode.Impulse);
            rb.AddForce(transform.up * 16f, ForceMode.Impulse);
            Destroy(newSnowball, 5);
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }

        //Resets position and rotation once attack is complete
        transform.position =  oldPosition;
        transform.rotation = oldRotation;
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
