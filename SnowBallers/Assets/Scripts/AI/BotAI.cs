using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Tutorial used: https://www.youtube.com/watch?v=UjkSFoLxesw&ab_channel=Dave%2FGameDevelopment
//

public class BotAI : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    //Dodging
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //States
    public float sightRange;
    public bool playerInSightRange;

    //IK_Character Model Stuff
    public GameObject model;
    public bool lookRotation = true;
    private Transform modelTransform;
    private Animator modelAnimator;

    void Start()
    {
        modelTransform = model.transform;
        modelAnimator = model.GetComponent<Animator>();
    }

    //*issue* - want to grab players prefab
    private void Awake()
    {
        //player = GameObject.GetComponent<NetworkPlayer>().transform;
        agent = GetComponent<NavMeshAgent>();
    }

    // Check if player is in Sight
    void Update()
    {
        playerInSightRange  = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);

        if (!playerInSightRange)
            Dodging();
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
        if(distanceToWalkPoint.magnitude < 0.1f)
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

    //visualize bot sight range
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
