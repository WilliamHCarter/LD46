using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    public GameObject brokenVersion;

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Meteor")
        {
            Debug.Log(Time.time);
            Destroy(GetComponent<Rigidbody>());
            Destroy(GetComponent<MeshCollider>());
            Destroy(GetComponent<MeshRenderer>());
            Instantiate(brokenVersion, gameObject.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        
    }
}
