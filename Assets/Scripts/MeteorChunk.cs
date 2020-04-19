using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorChunk : MonoBehaviour
{
    Rigidbody rb;
    Vector3 parentPos;
    // Start is called before the first frame update
    void Start()
    {
        parentPos = gameObject.transform.parent.transform.position;
        rb = GetComponent<Rigidbody>();
        rb.AddExplosionForce(2000,parentPos,5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
