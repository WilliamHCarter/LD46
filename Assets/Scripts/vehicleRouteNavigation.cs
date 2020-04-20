using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]

public class vehicleRouteNavigation : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject[] routePoints;

    private int routeDestinationIndex;

    private Rigidbody rb;
    private NavMeshPath path;

    // Start is called before the first frame update
    void Start()
    {
        routeDestinationIndex = getStartPointIndex();
        Debug.Log("Route Destination Index: "+routeDestinationIndex);
        rb = gameObject.GetComponent<Rigidbody>();
        path = new NavMeshPath();
        agent.destination = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(agent.destination, routePoints[routeDestinationIndex].transform.position) >= 1) //if you are not currently going to where you're supposed to be going
        {
            bool isAccesable = NavMesh.CalculatePath(transform.position, routePoints[routeDestinationIndex].transform.position, NavMesh.AllAreas, path);
            if (isAccesable) //if it is possible to go to where you're supposed to be going
            {
                agent.SetPath(path);
            }
            else
            {
                routeDestinationIndex++;
                if (routeDestinationIndex > routePoints.Length - 1)
                    routeDestinationIndex = 0;
            }
        }
        if (Vector3.Distance(transform.position, routePoints[routeDestinationIndex].transform.position) <= 1)//if you have arrived at your destinatation
        {
            routeDestinationIndex++;
            if (routeDestinationIndex > routePoints.Length - 1)
                routeDestinationIndex = 0;
        }
        //if the car is flipped over then disable it
        if (Mathf.Abs(transform.eulerAngles.z) > 30)
            Destroy(GetComponent<vehicleRouteNavigation>());
    }
    private int getStartPointIndex()
    {
        //calculates the two closest points to the car's position and sets the starting point to be the one further along in the route

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

        int nextClosestPointIndex = 0;
        float nextClosestPointDistance = Vector3.Distance(routePoints[0].transform.position, transform.position);

        for (int i = 0; i < routePoints.Length; i++)
        {
            if (i != closestPointIndex)
            {
                float dist = Vector3.Distance(transform.position, routePoints[i].transform.position);
                if (dist < nextClosestPointDistance)
                {
                    nextClosestPointIndex = i;
                    nextClosestPointDistance = dist;
                }
            }
        }
        Debug.Log("Closest Point: "+closestPointIndex);
        Debug.Log("Next Closest Point: " + nextClosestPointIndex);
        if (nextClosestPointIndex > closestPointIndex)
            return nextClosestPointIndex;
        if(nextClosestPointIndex==1)
            return nextClosestPointIndex;
        return closestPointIndex;

    }
    
}
