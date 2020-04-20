using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeteorSpawner : MonoBehaviour
{
    GameObject player;
    private Vector3 playerLocation;
    private float playerSpeed;
    private float distanceToFireAt;
    private float headsUpDistance;
    private Transform targetArea;
    private GameObject meteorUI;

    public GameObject meteorPrefab;
    public Transform[] targetPositions;
    public int headsUpTime;

    private bool meteorsHaveFired = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerSpeed = player.GetComponent<NavMeshAgent>().speed;
        //1.6 seconds is how long the meteors will be in the air
        //so the player needs to be 1.6 seconds away from the targetArea when the meteors need to fire
        distanceToFireAt = 1.6f * playerSpeed;
        headsUpDistance = distanceToFireAt + headsUpTime * playerSpeed;
        meteorUI = GameObject.FindGameObjectWithTag("MeteorWarning");
        meteorUI.SetActive(false);
    }
    private void Update()
    {
        playerLocation = player.transform.position;
        /*
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Fire!");
            fireMeteors();
        }
        */
        targetArea = getClosestArea();
        
        float dist = Vector3.Distance(targetArea.position, playerLocation);

        if (dist <= distanceToFireAt)
        {
            if(!meteorsHaveFired)
                fireMeteors();
        }
        else if (dist <= headsUpDistance)
        {
            meteorUI.SetActive(true);
        }
        else
        {
            meteorUI.SetActive(false);
        }
        if (dist > headsUpDistance+1 && meteorsHaveFired) //reset meteors if player is far away
            meteorsHaveFired = false;

    }
    // Start is called before the first frame update
    
    private void fireMeteors()
    {
        meteorsHaveFired = true;
        int numberOfMeteorsToSpawn = Random.Range(3,5);
        //numberOfMeteorsToSpawn = 1;
        Vector3 spawnArea = new Vector3(targetArea.position.x + 20, targetArea.position.y + 50, targetArea.position.z - 20);
        Vector3 directionToTarget = (targetArea.position - spawnArea).normalized;

        for (int i = 0; i < numberOfMeteorsToSpawn; i++)
        {
            int spawnOffset = Random.RandomRange(0, 6);
            Vector3 spawnLocation = new Vector3(spawnArea.x+spawnOffset+i*2, spawnArea.y + spawnOffset+i*2, spawnArea.z + spawnOffset-i*2);
            float scaleOffset = Random.Range(-0.5f,0.5f);
            GameObject obj = Instantiate(meteorPrefab,spawnLocation,Quaternion.identity);
            obj.transform.localScale += new Vector3(scaleOffset,scaleOffset,scaleOffset);
            obj.transform.rotation = Quaternion.LookRotation(directionToTarget,Vector3.up);
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.AddForce(obj.transform.forward * 2000);
        }
    }

    private Transform getClosestArea()
    {
        Transform ret = targetPositions[0];
        int minDist = (int)Vector3.Distance(playerLocation, targetPositions[0].position);
        for (int i = 0; i < targetPositions.Length; i++)
        {
            int dist = (int)Vector3.Distance(playerLocation, targetPositions[i].position);
            if (dist<minDist)
            {
                minDist = dist;
                ret = targetPositions[i];
            }
        }
        return ret;
    }
}
