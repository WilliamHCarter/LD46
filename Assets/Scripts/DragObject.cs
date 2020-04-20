using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class DragObject : MonoBehaviour
{
    //these public variables should be deleted later
    public bool mouseOverObj = false;
    public bool pickedUp = false;
    public bool readyToFire = false;
    public bool leftClick = false;
    public bool rightClick = false;

    public Transform cam;
    public int throwPowerMultiplier = 400; //maybe this can be dependant on the object being thrown
    public int minimumYPosition = 10;

    private Rigidbody rb;

    private float cameraOffset;
    private bool escape = false;
    private bool objectBeingHeld;
    private Component stateManager;
    private bool wasPickedUp = false;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        stateManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<GameStateManger>();
    }

    private void Update()
    {
        leftClick = Input.GetMouseButton(0);
        rightClick = Input.GetMouseButton(1);

        objectBeingHeld = cam.gameObject.GetComponent<isHoldingObject>().holdingObject();

        if (!escape && (pickedUp || !objectBeingHeld))
        {
            if (!rightClick)
                stateManager.GetComponent<GameStateManger>().setPowerBarActive(false);
            if (mouseOverObj || pickedUp || readyToFire)
            {
                if (leftClick && !pickedUp) //if left click and pickedUp is false 
                {
                    pickedUp = true;
                    wasPickedUp = true;
                    cam.gameObject.GetComponent<isHoldingObject>().holdingObject(true);
                    cameraOffset = Vector3.Distance(cam.transform.position, gameObject.transform.position);
                    stateManager.GetComponent<GameStateManger>().startHeldTimer(gameObject);
                }
                if (!leftClick && !rightClick) //if not left click and not right click 
                {
                    if (readyToFire) //if both buttons are released when in firing position then fire
                    {
                        //fire object
                    }
                    else
                    {
                        readyToFire = false;
                        pickedUp = false;
                    }
                }
                if (pickedUp)
                {
                    rb.useGravity = false;
                    if (rightClick)
                    {
                        readyToFire = true;
                        stateManager.GetComponent<GameStateManger>().increasePowerBar();
                    }
                    Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraOffset); //6.33 is offset from camera 
                    Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
                    transform.position = clampY(objPosition,minimumYPosition);

                    if (!rightClick && !leftClick)
                        pickedUp = false;
                    
                }
                if (!rightClick)
                {
                    if (readyToFire)//if the left click is released when object is in firing position then fire object
                    {
                        //fire object
                        rb.AddForce(cam.forward * stateManager.GetComponent<GameStateManger>().getPowerBarValue() * throwPowerMultiplier);
                        Debug.Log("Fire!");
                        pickedUp = false;
                    }
                    readyToFire = false;
                    stateManager.GetComponent<GameStateManger>().ResetPowerBar();
                }

                if (!pickedUp && !readyToFire)
                    rb.useGravity = true;
            }
            if (readyToFire && !stateManager.GetComponent<GameStateManger>().getPowerBarActive())
                stateManager.GetComponent<GameStateManger>().setPowerBarActive(true);
            //Debug.Log("Picked up: " + pickedUp + "Mouse Button 0 down: "+ Input.GetMouseButtonDown(0) + " Mouse Button 1 down: " + Input.GetMouseButtonDown(1));
            if (!pickedUp)
                cam.gameObject.GetComponent<isHoldingObject>().holdingObject(false);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            escape = !escape;
            if (!escape)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
            }
        }
        if(wasPickedUp && !pickedUp)
        {
            //if you have already been picked up and you are not currently being held then you cannot be picked up anymore
            rb.useGravity = true;
            stateManager.GetComponent<GameStateManger>().stopHeldTimer();
            Destroy(this);
        }

    }
    private void OnMouseEnter()
    {
        mouseOverObj = true;
    }
    private void OnMouseExit()
    {
        mouseOverObj = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        //canBePickedUp = true; //objects cannot be used more than once
    }
    private Vector3 clampY(Vector3 pos, int minY)
    {
        float newY = pos.y;
        if (newY < minY)
        {
            newY = minY;
        }
        return new Vector3(pos.x,newY,pos.z);

    }
}
