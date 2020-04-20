using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]

public class RouteNavigation : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject[] routePoints;
    public bool loop = false;

    private bool wasInCollsision = false;
    private int routeDestinationIndex;

    private Renderer renderer;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
        routeDestinationIndex = 0;
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        if (!wasInCollsision) {
            int closestPointIndex = getClosestPointIndex();
            if (closestPointIndex > routeDestinationIndex && closestPointIndex!=routePoints.Length-1) //if there is a point closer to you in your route than where you're going and that point is also further along in your route then go there
            {
                routeDestinationIndex = closestPointIndex;
            }
            if (Vector3.Distance(transform.position, routePoints[routeDestinationIndex].transform.position) < 1) //if you have arrived at your destinatation
            {
                if (routeDestinationIndex == routePoints.Length - 1)//if this is your final destination
                {
                    //Debug.Log("arrived at final destination");
                    if (!loop)
                    {
                        //eventually there will be some game over function that goes here but for now
                        GameObject.FindGameObjectWithTag("UIManager").GetComponent<GameStateManger>().LoadWinScreen();
                    }
                    else
                    {
                        routeDestinationIndex = 0; // go back to the begining of your route
                        Debug.Log("going back to the begining");
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
                //if(gameObject.name!="Player")
                    //Debug.Log("destination is "+routeDestinationIndex);
                //Debug.Log(agent.destination);
            }            
        }
    }
    private void inCollision() //called when car collides
    {
        wasInCollsision = true;
        gameObject.GetComponent<NavMeshAgent>().enabled = false;
        rb.isKinematic = false;
    }
    private void inCollision(GameObject obj) //called when player collides
    {
        if (obj.layer!=11) { //player can't collide with the ground or the roads or things like that 
            GameObject.FindGameObjectWithTag("UIManager").GetComponent<GameStateManger>().LoadLoseScreen(obj);

            wasInCollsision = true;
            gameObject.GetComponent<NavMeshAgent>().enabled = false;
            rb.isKinematic = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!wasInCollsision)
        {
            if (gameObject.tag == "Player")
            {
                inCollision(other.gameObject);
            }
            else
            {
                if(!(gameObject.layer==10 && other.gameObject.layer==10)) //cars can't collide with other cars
                    inCollision();
            }
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
