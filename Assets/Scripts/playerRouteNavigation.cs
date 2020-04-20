using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]

public class playerRouteNavigation : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject[] routePoints;

    private int routeDestinationIndex;

    private Rigidbody rb;
    private NavMeshPath path;

    // Start is called before the first frame update
    void Start()
    {
        routeDestinationIndex = 0;
        rb = gameObject.GetComponent<Rigidbody>();
        path = new NavMeshPath();
    }

    // Update is called once per frame
    void Update()
    {
        int closestPointIndex = getClosestPointIndex();
        if (closestPointIndex > routeDestinationIndex && closestPointIndex != routePoints.Length - 1) //if there is a point closer to you in your route than where you're going and that point is also further along in your route then go there
        {
            routeDestinationIndex = closestPointIndex;
        }
        if (Vector3.Distance(transform.position, routePoints[routeDestinationIndex].transform.position) < 1)//if you have arrived at your destinatation
        {
            if (routeDestinationIndex == routePoints.Length - 1) //if this is the final destination
            {
                GameObject.FindGameObjectWithTag("UIManager").GetComponent<GameStateManger>().LoadWinScreen();
            }
            else //go to the next point in your route
            {
                routeDestinationIndex++;
            }
        }
        if(Vector3.Distance(agent.destination, routePoints[routeDestinationIndex].transform.position) > 3) //if the distance from where your agent is going and where they're supposed to be going is greater than 3
        {
            bool isAccesable = NavMesh.CalculatePath(transform.position, routePoints[routeDestinationIndex].transform.position,NavMesh.AllAreas,path);
            if (isAccesable)
            {
                agent.SetPath(path);
            }
            else
            {
                if (routeDestinationIndex == routePoints.Length - 1)
                {
                    Debug.Log("Player can't reach goal");
                }
                else
                {
                    routeDestinationIndex++;
                }
            }
        }
    }
    private int getClosestPointIndex()
    {
        int closestPointIndex = 0;
        float closestPointDistance = Vector3.Distance(routePoints[0].transform.position, transform.position);
        for (int i = 0; i < routePoints.Length; i++)
        {
            float dist = Vector3.Distance(transform.position, routePoints[i].transform.position);
            if (dist < closestPointDistance)
            {
                closestPointIndex = i;
                closestPointDistance = dist;
            }
        }
        return closestPointIndex;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 11) //player can't collide with the ground or the roads or things like that 
        {
            GameObject.FindGameObjectWithTag("UIManager").GetComponent<GameStateManger>().LoadLoseScreen(other.gameObject);
            gameObject.GetComponent<NavMeshAgent>().enabled = false;
            rb.isKinematic = false;
        }
    }
}
