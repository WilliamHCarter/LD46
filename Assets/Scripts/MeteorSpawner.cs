using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{

    private Vector3 playerLocation;

    public GameObject meteorPrefab;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Fire!");
            fireMeteors();
        }
            
    }
    // Start is called before the first frame update
    void Start()
    {
        playerLocation = GameObject.FindGameObjectWithTag("Player").transform.position;
        fireMeteors();
        
    }
    private void fireMeteors()
    {
        int numberOfMeteorsToSpawn = Random.Range(3,5);
        //numberOfMeteorsToSpawn = 1;
        Vector3 spawnArea = new Vector3(playerLocation.x + 20, playerLocation.y + 50, playerLocation.z - 20);
        Vector3 directionToPlayer = (playerLocation - spawnArea).normalized;

        for (int i = 0; i < numberOfMeteorsToSpawn; i++)
        {
            int spawnOffset = Random.RandomRange(0, 6);
            Vector3 spawnLocation = new Vector3(spawnArea.x+spawnOffset+i*2, spawnArea.y + spawnOffset+i*2, spawnArea.z + spawnOffset-i*2);
            float scaleOffset = Random.Range(-0.5f,0.5f);
            GameObject obj = Instantiate(meteorPrefab,spawnLocation,Quaternion.identity);
            obj.transform.localScale += new Vector3(scaleOffset,scaleOffset,scaleOffset);
            obj.transform.rotation = Quaternion.LookRotation(directionToPlayer,Vector3.up);
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.AddForce(obj.transform.forward * 2000);
        }
    }
}
