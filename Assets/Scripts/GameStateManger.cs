using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStateManger : MonoBehaviour
{
    private GameObject Reticle;
    private GameObject PowerBar;
    private GameObject radialTimer;

    private GameObject WinScreen;
    private GameObject LoseScreen;
    private GameObject PauseMenu;
    private GameObject ControlsMenu;

    private GameObject player;

    public TextMeshProUGUI textMeshProText;
    public int powerBarMax = 3;

    private string state;
    private bool powerBarIsOn = false;
    private float power;
    private int radialTimerValue = 0;
    private bool timerEnabled = false;
    private float timePassed = 0.0f;
    private int timerMaxSeconds = 5;
    private GameObject objThatStartedTimer;

    //unity methods
    void Start()
    {
        Reticle = GameObject.FindGameObjectWithTag("Reticle");
        PowerBar = GameObject.FindGameObjectWithTag("PowerBar");
        PowerBar.GetComponent<PowerBar>().setMaxValue(powerBarMax * 10);
        radialTimer = GameObject.FindGameObjectWithTag("RadialTimer");
        radialTimer.SetActive(false);

        WinScreen = GameObject.FindGameObjectWithTag("WinScreen");
        PauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
        LoseScreen = GameObject.FindGameObjectWithTag("LoseScreen");
        ControlsMenu = GameObject.FindGameObjectWithTag("ControlsMenu");

        player = GameObject.FindGameObjectWithTag("Player");

        LoadGame();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (state == "PauseMenu")
            {
                LoadGame();
            }
            else if (state == "Game")
            {
                LoadPauseMenu();
            }
            else
            {
                //can't load pause menu from anywhere other than game
            }
        }
        if (timerEnabled)
        {
            timePassed += Time.deltaTime;
            float fill = timePassed / timerMaxSeconds;
            if (fill <= 1)
            {
                Image img = radialTimer.GetComponent<Image>();
                img.fillAmount = fill;
                Color color = Color.green;
                if (fill > 0.4f)
                    color = new Color(255,165,0);
                if (fill > 0.8)
                    color = Color.red;
                img.color = color;
            }
            else
            {
                //time's up
                Destroy(objThatStartedTimer.GetComponent<DragObject>());
                objThatStartedTimer.GetComponent<Rigidbody>().useGravity = true;
                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<isHoldingObject>().holdingObject(false);
                stopHeldTimer();
            }
        }
    }

    private void UnloadEverything()
    {
        Reticle.SetActive(false);
        PowerBar.SetActive(false);

        WinScreen.SetActive(false);
        PauseMenu.SetActive(false);
        LoseScreen.SetActive(false);
        ControlsMenu.SetActive(false);
    }
    private string realObjName(GameObject obj)
    {
        string ret = "Unknown Object";
        if (obj.layer == 12)
            ret = "Building";
        if (obj.layer == 13)
            ret = "Tree";
        if (obj.layer == 10)
            ret = "Car";
        if (obj.layer == 14)
            ret = "Broken Meteor Shrapnel";
        return ret;
    }
    
    //timer stuff
    public void startHeldTimer(GameObject obj)
    {
        timerEnabled = true;
        radialTimer.SetActive(true);
        objThatStartedTimer = obj;
    }
    public void stopHeldTimer()
    {
        timerEnabled = false;
        radialTimer.SetActive(false);
        timePassed = 0.0f;
    }

    //loading different game states
    public void LoadWinScreen()
    {
        Time.timeScale = 1f;
        state = "WinScreen";
        UnloadEverything();
        WinScreen.SetActive(true);

    }
    public void LoadLoseScreen(GameObject obj)
    {
        Time.timeScale = 1f;
        state = "WinScreen";
        UnloadEverything();
        LoseScreen.SetActive(true);
        textMeshProText.SetText("You were hit by a "+realObjName(obj));

    }
    public void LoadPauseMenu()
    {
        Time.timeScale = 0f;
        state = "PauseMenu";
        UnloadEverything();
        PauseMenu.SetActive(true);

    }
    public void LoadControlsMenu()
    {
        Time.timeScale = 0f;
        state = "ControlsMenu";
        UnloadEverything();
        ControlsMenu.SetActive(true);

    }
    public void LoadGame()
    {
        Time.timeScale = 1f;
        state = "Game";
        UnloadEverything();
        Reticle.SetActive(true);
        PowerBar.SetActive(true);
        player.SetActive(true);

    }
    public void LoadTitleScreen()
    {
        SceneManager.LoadScene("TitleScreen", LoadSceneMode.Single);
    }

    //power bar stuff
    public void increasePowerBar()
    {
        if (powerBarIsOn && power < powerBarMax)
        {
            power += Time.deltaTime;
            float powerNormalized = power / powerBarMax;
            PowerBar.GetComponent<PowerBar>().SetValue((int)(powerNormalized*100));
        }
        else
        {
            //PowerBar.GetComponent<PowerBar>().SetValue(powerBarMax*30);
        }
    }
    public void setPowerBarActive(bool value)
    {
        powerBarIsOn = value;
        PowerBar.SetActive(value);
    }
    public bool getPowerBarActive()
    {
        return powerBarIsOn;
    }
    public void ResetPowerBar()
    {
        power = 0;
        PowerBar.GetComponent<PowerBar>().SetValue(0);
    }
    public float getPowerBarValue()
    {
        return power;
    }
   
}
