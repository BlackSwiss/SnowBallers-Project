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
    public Vector3 walkPoint, destination;
    bool walkPointSet;
    public float walkPointRange;
    NavMeshHit hit;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject snowball;
    public float attackHorizontalVelocity = 10;
    public float attackVerticalVelocity = 5;
    public float attackAnimationDelay = 1.85f;

    //Hiding
    public LayerMask HidableLayers;
    public EnemyLineOfSightChecker LineOfSightChecker;
    [Range(-1, 1)]
    public float HideSensitivity = 0;
    private Collider[] Colliders = new Collider[5];
    //Hiding test
    public int ammoCount = 3;
    public Vector3 distanceToHidePoint, distanceToHidePoint2;
    bool hidePointSet, hidePointSet2;

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
    public Transform modelRightHandTransform;

    //Rotation
    float speed = 0.01f;
    float timeCount = 0.0f;
    bool isChaseOrAttack = false;
    
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
        syncModelTransform();

        if (players.Count > 0)
        {
            player = players[0].transform;
            //Debug.Log("Player found" + player.transform.position.x + player.transform.position.z);
        }

        playerInSightRange  = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if(isChaseOrAttack)
            lookAtPlayer();

        if(alreadyAttacked)
        {
            //Debug.Log("Bot Dodging");
            modelAnimator.SetBool("Throw", false);
            modelAnimator.SetBool("Move", false);
            Dodging();
            isChaseOrAttack = false;
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
                isChaseOrAttack = true;
            }
            else if(playerInSightRange)
            {
                Debug.Log("Bot Chasing");
                modelAnimator.SetBool("Move", true);
                ChasePlayer();
                isChaseOrAttack = true;
            }
            else
            {
                Debug.Log("Bot Dodging");
                modelAnimator.SetBool("Move", false);
                Dodging();
                isChaseOrAttack = false;
            }
        }
    }

    private void Dodging()
    {
        if (!walkPointSet)
            SearchWalkPoint(); 

        if (walkPointSet)
            {
            agent.SetDestination(walkPoint);
            Debug.DrawRay(walkPoint, Vector3.up, Color.blue, 1.0f);
            }
        
        //Calculate distance to walkpoint
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //walkpoint reached
        //#update increase to 0.5, issue with y-axis changing slightly higher 
        // making walkpoint impossible to reach (might need to increase)
        if(distanceToWalkPoint.magnitude < 0.5f)
        {
            walkPointSet = false;
            //Vector3 walkPointNoY = new Vector3(transform.position.x, modelTransform.position.y, transform.position.z);
            //modelTransform.position = walkPointNoY;
        }  
    }

    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);


        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        //Search for random point on map within bounds 
        //#update 4/20/23 - changed to using samplePosition to confirm a walkpoint on the navmesh
        // had an issue with getting stuck when walkpoint was out of bounds.
        if (NavMesh.SamplePosition(walkPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            walkPoint = new Vector3(hit.position.x, transform.position.y, hit.position.z);
            walkPointSet = true;

            //Make bot look at player
            //transform.LookAt(new Vector3(walkPoint.x, transform.position.y, walkPoint.z));
            //transform.LookAt(walkPoint);
        }
    }

    private void ChasePlayer()
    {
        //transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        //Checks if agent is on nav mesh
        if(!agent.isOnNavMesh)
            return;

        //Make sure bot doesn't move
        agent.SetDestination(transform.position);

        //Make bot look at player
        //transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
        //transform.LookAt(player);

        if (!alreadyAttacked)
        {
            alreadyAttacked = true;

            //Attack code ***need to write AIs own snowball code
            GameObject newSnowball = Instantiate(snowball, modelRightHandTransform.position, Quaternion.identity);
            Rigidbody rb = newSnowball.GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * attackHorizontalVelocity, ForceMode.Impulse);
            rb.AddForce(transform.up * attackVerticalVelocity, ForceMode.Impulse);
            Destroy(newSnowball, 5);
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
            Invoke(nameof(ResetIKTracking), attackAnimationDelay);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void ResetIKTracking()
    {
        modelRigBuilder.enabled = true;
        isChaseOrAttack = false;
    }

    private void syncModelTransform()
    {
        //Get parent transform without Y component since model is positioned lower than parent
        Vector3 transformNoY = new Vector3(transform.position.x, modelTransform.position.y, transform.position.z);
        //Sync parent transform and model transform
        modelTransform.position = transformNoY;
        modelTransform.rotation = transform.rotation;
    }

    private void lookAtPlayer()
    {
        transform.LookAt(player);
    }

    private void lookAtTarget(Vector3 target)
    {
        Quaternion lookRotation = Quaternion.LookRotation(target, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, timeCount * speed);
        timeCount = timeCount + Time.deltaTime;
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

        if ((hidePointSet || hidePointSet2) && ((distanceToHidePoint.magnitude < 0.1f) || (distanceToHidePoint2.magnitude < 0.1f)))
        {
            Debug.Log("bot reloaded");
            ammoCount = 3;
            hidePointSet = false;
            hidePointSet2 = false;
            ResetAttack();
        }
        else
        {    
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
                        hidePointSet = true;

                        //Calculate distance to walkpoint
                        Vector3 distanceToHidePoint = transform.position - hit.position;

                        //walkpoint reached
                        //if(distanceToHidePoint.magnitude < 0.1f)
                        //{
                        //    Debug.Log("bot reloaded");
                        //    ammoCount = 3;
                        //    ResetAttack();
                        //}  
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
                                hidePointSet2 = true;

                                //Calculate distance to walkpoint
                                Vector3 distanceToHidePoint2 = transform.position - hit2.position;

                                //walkpoint reached
                                //if(distanceToHidePoint2.magnitude < 0.1f)
                                // {
                                //    Debug.Log("bot reloaded2");
                                //    ammoCount = 3;
                                //   ResetAttack();
                                //}
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
}
