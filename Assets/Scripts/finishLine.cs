using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class finishLine : MonoBehaviour
{
    public int rotateSpeed = 4;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, Time.deltaTime * rotateSpeed, 0);
    }
}
