using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawnerTest : MonoBehaviour
{
    public GameObject objectToSpawn;
    public int numberToSpawn;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numberToSpawn; i++)
        {
            Instantiate(objectToSpawn,gameObject.transform.position,Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
