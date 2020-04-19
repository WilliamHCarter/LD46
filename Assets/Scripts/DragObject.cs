using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class DragObject : MonoBehaviour
{
    public bool mouseOverObj = false;
    public bool pickedUp = false;
    public bool readyToFire = false;
    public bool leftClick = false;
    public bool rightClick = false;
    private Rigidbody rb;
    public GameObject powerBarObj;
    private PowerBar powerBar;
    private int maxPowerValue = 3;
    private float power = 0;
    public Transform cam;
    private bool canBePickedUp = true;
    public int throwPowerMultiplier = 400;
    private float cameraOffset;
    private bool escape = false;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        powerBar = powerBarObj.GetComponent<PowerBar>();
        powerBar.setMaxValue(maxPowerValue*10);
    }

    public Transform guide;

    private void Update()
    {
        leftClick = Input.GetMouseButton(0);
        rightClick = Input.GetMouseButton(1);

        if (!escape)
        {
            if (!rightClick)
                powerBarObj.SetActive(false);
            if ((mouseOverObj || pickedUp || readyToFire) && canBePickedUp)
            {
                if (leftClick && !pickedUp) //if left click and pickedUp is false
                {
                    pickedUp = true;
                    cameraOffset = Vector3.Distance(cam.transform.position, gameObject.transform.position);
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
                        //go to ready position
                        //transform.position = guide.transform.position;
                        //start charging up shot 
                        IncreasePower();
                    }
                    else if (leftClick)
                    {
                        //drag around 
                        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraOffset); //6.33 is offset from camera 
                        Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
                        transform.position = objPosition;
                    }
                    else
                        pickedUp = false;
                }
                if (!rightClick)
                {
                    if (readyToFire)//if the left click is released when object is in firing position then fire object
                    {
                        //fire object
                        rb.AddForce(cam.forward * power * throwPowerMultiplier);
                        resetPowerBar();
                        Debug.Log("Fire!");
                        pickedUp = false;
                        canBePickedUp = false;
                    }
                    readyToFire = false;
                    power = 0.0f;
                }

                if (!pickedUp && !readyToFire)
                    rb.useGravity = true;
            }
            if (readyToFire && !powerBarObj.activeSelf)
                powerBarObj.SetActive(true);
            //Debug.Log("Picked up: " + pickedUp + "Mouse Button 0 down: "+ Input.GetMouseButtonDown(0) + " Mouse Button 1 down: " + Input.GetMouseButtonDown(1));
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
    }
    private void OnMouseEnter()
    {
        mouseOverObj = true;
    }
    private void OnMouseExit()
    {
        mouseOverObj = false;
    }
    private void IncreasePower()
    {
        //increments power once every second
        if (!powerBarObj.active)
        {
            powerBarObj.SetActive(true);
            //powerBar.SetValue(0);
        }
        if (power < maxPowerValue)
        {
            power+=Time.deltaTime;
            powerBar.SetValue((int)(power*10));
        }   
    }
    private void resetPowerBar()
    {
        power = 0;
        powerBar.SetValue(0);
    }
    private void OnCollisionEnter(Collision collision)
    {
        canBePickedUp = true;
    }
}
