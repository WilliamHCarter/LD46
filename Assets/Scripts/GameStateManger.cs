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

    private GameObject player;

    public TextMeshProUGUI textMeshProText;

    private string state;

    // Start is called before the first frame update
    void Start()
    {
        Reticle = GameObject.FindGameObjectWithTag("Reticle");
        PowerBar = GameObject.FindGameObjectWithTag("PowerBar");

        WinScreen = GameObject.FindGameObjectWithTag("WinScreen");
        PauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
        LoseScreen = GameObject.FindGameObjectWithTag("LoseScreen");

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
    }
    public void LoadWinScreen()
    {
        state = "WinScreen";
        UnloadEverything();
        WinScreen.SetActive(true);

    }
    public void LoadLoseScreen(string loseText)
    {
        state = "WinScreen";
        UnloadEverything();
        LoseScreen.SetActive(true);
        textMeshProText.SetText(loseText);

    }
    public void LoadPauseMenu()
    {
        state = "PauseMenu";
        UnloadEverything();
        PauseMenu.SetActive(true);

    }
    public void LoadGame()
    {
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
