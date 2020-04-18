using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class RouteNavigation : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject[] routePoints;
    public bool loop = false;

    private bool wasInCollsision = false;
    private int routeDestinationIndex;

    private Renderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
        routeDestinationIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {

        if (!wasInCollsision) {
            int closestPointIndex = getClosestPointIndex();
            if (closestPointIndex > routeDestinationIndex && !loop) //if there is a point closer to you in your toute then where you're going and that point is also further along in your route then go there
            {
                routeDestinationIndex = closestPointIndex;
            }
            if (Vector3.Distance(transform.position, routePoints[routeDestinationIndex].transform.position) < 1) //if you have arrived at your destinatation
            {
                if (routeDestinationIndex == routePoints.Length - 1)//if this is your final destination
                {
                    Debug.Log("arrived at final destination");
                    if (!loop)
                    {
                        //eventually there will be some game over function that goes here but for now
                        inCollision();
                    }
                    else
                    {
                        routeDestinationIndex = 0; // go back to the begining of your route
                    }
                }
                else //go to the next point in the route
                {
                    routeDestinationIndex++;
                }
            }

            if (Vector3.Distance(agent.destination,routePoints[routeDestinationIndex].transform.position)>3)  //if you arn't going to your destination then go there
            {
                agent.destination = routePoints[routeDestinationIndex].transform.position;
                if(gameObject.name=="Player")
                    Debug.Log("destination is "+routeDestinationIndex);
                //Debug.Log(agent.destination);
            }            
        }
    }

    public void inCollision()
    {
        wasInCollsision = true;
        gameObject.GetComponent<NavMeshAgent>().enabled = false;  
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!wasInCollsision)
        {
            Debug.Log(gameObject.name + " was hit by " + other.gameObject.name);
            inCollision();
        }
    }
    private int getClosestPointIndex()
    {
        int closestPointIndex = 0;
        float closestPointDistance = Vector3.Distance(routePoints[0].transform.position,transform.position);
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
}
