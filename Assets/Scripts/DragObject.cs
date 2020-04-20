using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class DragObject : MonoBehaviour
{
    //these public variables should be deleted later
    private bool mouseOverObj = false;
    private bool pickedUp = false;
    private bool readyToFire = false;
    private bool leftClick = false;
    private bool rightClick = false;

    public int throwPowerMultiplier = 400; //maybe this can be dependant on the object being thrown
    public int minimumYPosition = 10;

    private Rigidbody rb;

    private float cameraOffset;
    private bool escape = false;
    private bool objectBeingHeld; //is an object being held
    private Component stateManager;
    private bool wasPickedUp = false;
    private Transform cam;
    private Vector3 offset;
    private bool beingLookedAt = false;

    public float countDownCount = 0.0f;
    private int countDownTime = 3;

    private void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
        rb = gameObject.GetComponent<Rigidbody>();
        stateManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<GameStateManger>();
        if(gameObject.GetComponent<pickUpOffset>() != null)
        {
            offset = gameObject.GetComponent<pickUpOffset>().getOffset();
        }
        else
        {
            offset = Vector3.zero;
        }
    }

    public bool ableToBePickedUp = true;

    private void Update()
    {
        leftClick = Input.GetMouseButton(0);
        rightClick = Input.GetMouseButton(1);

        objectBeingHeld = cam.gameObject.GetComponent<isHoldingObject>().holdingObject();

        if (!ableToBePickedUp)
        {
            countDownCount -= Time.deltaTime;
            if (countDownCount <= 0.0f)
            {
                ableToBePickedUp = true;
                countDownCount = 0.0f;
            }    
        }

        if (!escape && (pickedUp || !objectBeingHeld) && ableToBePickedUp)
        {
            if (!rightClick)
                stateManager.GetComponent<GameStateManger>().setPowerBarActive(false);
            if(mouseOverObj && Input.GetMouseButtonDown(0) && !pickedUp)
            {
                pickedUp = true;
                wasPickedUp = true;
                cam.gameObject.GetComponent<isHoldingObject>().holdingObject(true);
                cameraOffset = Vector3.Distance(cam.transform.position, gameObject.transform.position);
                stateManager.GetComponent<GameStateManger>().startHeldTimer(gameObject);
            } //if you are able to be picked up and clicked on then get picked up
            if (pickedUp || readyToFire)
            {
                if (pickedUp)
                {
                    rb.useGravity = false;
                    rb.isKinematic = false;
                    if (rightClick)
                    {
                        readyToFire = true;
                        stateManager.GetComponent<GameStateManger>().increasePowerBar();
                    }
                    Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraOffset); //6.33 is offset from camera 
                    Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
                    objPosition = applyOffset(objPosition);
                    transform.position = clampY(objPosition,minimumYPosition);

                    
                }
                if (!rightClick)
                {
                    if (readyToFire)//if the right click is released when object is in firing position then fire object
                    {
                        //fire object
                        rb.AddForce(cam.forward * stateManager.GetComponent<GameStateManger>().getPowerBarValue() * throwPowerMultiplier);
                        Debug.Log("Fire!");
                        pickedUp = false;
                        dropped();
                    }
                    readyToFire = false;
                    stateManager.GetComponent<GameStateManger>().ResetPowerBar();
                }
                if (!leftClick && !rightClick) //if not left click and not right click 
                {
                    dropped();
                    readyToFire = false;
                    pickedUp = false;
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
        if (mouseOverObj && !cam.GetComponent<isHoldingObject>().holdingObject()) //if the mouse is over this object and an object is not being held 
        {
            stateManager.GetComponent<GameStateManger>().mouseOverObject(countDownCount/countDownTime,gameObject);
        }
        if (!mouseOverObj && beingLookedAt)
        {
            stateManager.GetComponent<GameStateManger>().mouseOverObject(0,gameObject);
        }

    }
    public void dropped()
    {
        Debug.Log("dropped");
        pickedUp = false;
        ableToBePickedUp = false;
        countDownCount = countDownTime;
        objectBeingHeld = false;
        rb.useGravity = true;
        cam.GetComponent<isHoldingObject>().holdingObject(false);
        stateManager.GetComponent<GameStateManger>().stopHeldTimer();
        stateManager.GetComponent<GameStateManger>().ResetPowerBar();
    }
    private void OnMouseEnter()
    {
        mouseOverObj = true;
    }
    private void OnMouseExit()
    {
        mouseOverObj = false;
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
    private Vector3 applyOffset(Vector3 pos)
    {
        Vector3 ret = new Vector3(pos.x + offset.x, pos.y + offset.y, pos.z + offset.z);
        return ret;
    }
    public void SetBeingLookedAt(bool b)
    {
        beingLookedAt = b;
    }
}
