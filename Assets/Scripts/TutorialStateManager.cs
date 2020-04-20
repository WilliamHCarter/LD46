using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialStateManager : MonoBehaviour
{

    private GameObject Reticle;
    private GameObject PowerBar;
    private GameObject radialTimer;

    private GameObject WinScreen;
    private GameObject LoseScreen;
    private GameObject PauseMenu;
    private GameObject ControlsMenu;
    private GameObject Tutorial1;
    private GameObject Tutorial2;
    private GameObject Tutorial3;
    private GameObject Tutorial4;
    float startingTime = 10f;
    float currentTime = 0f;
    private int tutStepper;



    void Start()
    {
        Time.timeScale = 0.2f;
        Tutorial1 = GameObject.FindGameObjectWithTag("Tutorial1");
        Tutorial2 = GameObject.FindGameObjectWithTag("Tutorial2");
        Tutorial3 = GameObject.FindGameObjectWithTag("Tutorial3");
        Tutorial4 = GameObject.FindGameObjectWithTag("Tutorial4");
        LoadTutorial1();
        currentTime = startingTime;
    }

    void Update()
    {
        currentTime -= 1 * Time.deltaTime;
        if (System.Math.Abs(currentTime) < Mathf.Epsilon)
        {
            tutStepper++;
        }
    }

    void LoadTutorial1()
    {
        Tutorial1.SetActive(true);
        if (tutStepper==1)
        {
            LoadTutorial2();
        }

    }
    void LoadTutorial2()
    {
        Tutorial1.SetActive(false);
        Tutorial2.SetActive(true);
        if (tutStepper == 2)
        {
            LoadTutorial3();
        }
    }
    void LoadTutorial3()
    {
        Tutorial2.SetActive(false);
        Tutorial3.SetActive(true);
        if (tutStepper == 3)
        {
            LoadTutorial4();
        }
    }
    void LoadTutorial4()
    {
        Tutorial3.SetActive(false);
        Tutorial4.SetActive(true);
        if (tutStepper == 3)
        {
            SceneManager.LoadScene("Level1", LoadSceneMode.Single);
        }
    }
}
