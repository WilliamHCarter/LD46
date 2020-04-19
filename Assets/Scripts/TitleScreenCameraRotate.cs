using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenCameraRotate : MonoBehaviour
{
    public Camera cam;
    public int rotateSpeed = 2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0,rotateSpeed*Time.deltaTime,0);
        cam.transform.LookAt(gameObject.transform.position, Vector3.up);
    }
}
