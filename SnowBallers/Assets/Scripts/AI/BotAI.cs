using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

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
    public float attackHorizontalVelocity = 10;
    public float attackVerticalVelocity = 5;
    public float attackAnimationDelay = 1.85f;
    public float enableIKDelay = 1;

    //Hiding
    public LayerMask HidableLayers;
    public EnemyLineOfSightChecker LineOfSightChecker;
    [Range(-1, 1)]
    public float HideSensitivity = 0;
    private Collider[] Colliders = new Collider[5];
    //Hiding test
    public int ammoCount = 3;

    //States
    public float sightRange;
    public float attackRange;
    public bool playerInSightRange;
    public bool playerInAttackRange;

    //IK_Character Model Stuff
    public GameObject model;
    private Transform modelTransform;
    private Animator modelAnimator;
    private RigBuilder modelRigBuilder;

    void Start()
    {
        modelTransform = model.transform;
        modelAnimator = model.GetComponent<Animator>();
        players = networkPlayerSpawner.getPlayers();
        modelRigBuilder = GetComponentInChildren<RigBuilder>();
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
            //Debug.Log("Player found" + player.transform.position.x + player.transform.position.z);
        }

        playerInSightRange  = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        

        if(alreadyAttacked)
        {
            //Debug.Log("Bot Dodging");
            modelAnimator.SetBool("Throw", false);
            modelAnimator.SetBool("Move", false);
            Dodging();
        }
        else
        {
            if (ammoCount == 0)
            {
                Debug.Log("Bot Hiding");
                Hide();
            }
            else if(playerInAttackRange)
            {
                Debug.Log("Bot Attacking");
                modelRigBuilder.enabled = false;
                modelAnimator.SetBool("Throw", true);
                Invoke(nameof(AttackPlayer), attackAnimationDelay);
                ammoCount -= 1;
                Invoke(nameof(ResetIKTracking), enableIKDelay);
            }
            else if(playerInSightRange)
            {
                Debug.Log("Bot Chasing");
                modelAnimator.SetBool("Move", true);
                ChasePlayer();
            }
            else
            {
                //Debug.Log("Bot Dodging");
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
            //transform.LookAt(new Vector3(walkPoint.x, transform.position.y, walkPoint.z));
            transform.LookAt(walkPoint);
        }
    }

    private void ChasePlayer()
    {
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        //Checks if agent is on nav mesh
        if(!agent.isOnNavMesh)
            return;

        //Store old position and rotation to reset after attack
        Vector3 oldPosition = transform.position;
        Quaternion oldRotation = transform.rotation;
        
        //Make sure bot doesn't move
        agent.SetDestination(transform.position);

        //Make bot look at player
        //transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            alreadyAttacked = true;

            //Attack code ***need to write AIs own snowball code
            GameObject newSnowball = Instantiate(snowball, transform.position, Quaternion.identity);
            Rigidbody rb = newSnowball.GetComponent<Rigidbody>();
            rb.AddForce(modelTransform.forward * attackHorizontalVelocity, ForceMode.Impulse);
            rb.AddForce(modelTransform.up * attackVerticalVelocity, ForceMode.Impulse);
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

    private void ResetIKTracking()
    {
        modelRigBuilder.enabled = true;
    }

    //visualize bot sight range
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

    private void Hide()
    {

        
            int hits = Physics.OverlapSphereNonAlloc(agent.transform.position, LineOfSightChecker.Collider.radius, Colliders, HidableLayers);
            Debug.Log($"number of hiding spots: {hits}");

            for (int i = 0; i < hits; i++)
            {
                if (NavMesh.SamplePosition(Colliders[i].transform.position, out NavMeshHit hit, 2f, agent.areaMask))
                {
                    if (!NavMesh.FindClosestEdge(hit.position, out hit, agent.areaMask))
                    {
                        Debug.LogError($"Unable to find edge close to {hit.position}");
                    }

                    if (Vector3.Dot(hit.normal, (player.position - hit.position).normalized)  < HideSensitivity)
                    {
                        Debug.Log("Hiding spot found");
                        agent.SetDestination(hit.position);

                        //Calculate distance to walkpoint
                        Vector3 distanceToHidePoint = transform.position - hit.position;

                        //walkpoint reached
                        if(distanceToHidePoint.magnitude < 0.1f)
                        {
                            Debug.Log("bot reloaded");
                            ammoCount = 3;
                            ResetAttack();
                        }  
                        break;
                    }
                    else
                    {
                        //Since previous spot wasn't facing "away" from the player, we'll try the other side of the object
                        if (NavMesh.SamplePosition(Colliders[i].transform.position - (player.position - hit.position).normalized * 2, out NavMeshHit hit2, 2f, agent.areaMask))
                        {
                            if (!NavMesh.FindClosestEdge(hit2.position, out hit2, agent.areaMask))
                            {
                                Debug.LogError($"Unable to find edge close to {hit2.position} (second attempt)");
                            }

                            if (Vector3.Dot(hit2.normal, (player.position - hit2.position).normalized)  < HideSensitivity)
                            {
                                Debug.Log("Hiding spot2 found");
                                agent.SetDestination(hit2.position);

                                //Calculate distance to walkpoint
                                Vector3 distanceToHidePoint2 = transform.position - hit2.position;

                                //walkpoint reached
                                if(distanceToHidePoint2.magnitude < 0.1f)
                                {
                                    Debug.Log("bot reloaded2");
                                    ammoCount = 3;
                                    ResetAttack();
                                }
                                break;
                            }
                        }
                    }
                }
                else
                {
                    Debug.LogError($"Unable to find NavMesh near object {Colliders[i].name} at {Colliders[i].transform.position}");
                }
            }
    }
}
