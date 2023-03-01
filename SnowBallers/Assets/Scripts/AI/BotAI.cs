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
    public float RandomX, RandomZ;

    //States
    public float sightRange;
    public bool playerInSightRange;

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
            Debug.Log("start dodging");
            Dodging();
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
        if(distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + RandomX, transform.position.y, transform.position.z + RandomZ);

        //Search for random point on map within bounds
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }
}
