using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameStateManger : MonoBehaviour
{
    private GameObject Reticle;
    private GameObject PowerBar;

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

    // Start is called before the first frame update
    void Start()
    {
        Reticle = GameObject.FindGameObjectWithTag("Reticle");
        PowerBar = GameObject.FindGameObjectWithTag("PowerBar");
        PowerBar.GetComponent<PowerBar>().setMaxValue(powerBarMax * 10);

        WinScreen = GameObject.FindGameObjectWithTag("WinScreen");
        PauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
        LoseScreen = GameObject.FindGameObjectWithTag("LoseScreen");
        ControlsMenu = GameObject.FindGameObjectWithTag("ControlsMenu");

        player = GameObject.FindGameObjectWithTag("Player");

        LoadGame();
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
    public void increasePowerBar()
    {
        if (powerBarIsOn && power < powerBarMax)
        {
            power += Time.deltaTime;
            PowerBar.GetComponent<PowerBar>().SetValue((int)(power * 10));
        }
    }

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
    // Update is called once per frame
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
    }
}
