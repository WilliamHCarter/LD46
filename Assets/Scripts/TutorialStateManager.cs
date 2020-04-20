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
    float startingTime = 0f;
    float currentTime = 3f;
    private int tutStepper;



    void Start()
    {
        Time.timeScale = 1f;
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

        if (currentTime<-startingTime)
        {
            tutStepper++;
			currentTime = 3f;
        }
		if (tutStepper == 1)
		{
			LoadTutorial2();
		}
		if (tutStepper == 2)
		{
			LoadTutorial3();
		}
		if (tutStepper == 3)
		{
			LoadTutorial4();
		}
		if (tutStepper == 3)
		{
			SceneManager.LoadScene("Level1", LoadSceneMode.Single);
		}
	}

    void LoadTutorial1()
    {
        Tutorial1.SetActive(true);
		Tutorial2.SetActive(false);
		Tutorial3.SetActive(false);
		Tutorial4.SetActive(false);

	}
    void LoadTutorial2()
    {
        Tutorial1.SetActive(false);
        Tutorial2.SetActive(true);
    }
    void LoadTutorial3()
    {
        Tutorial2.SetActive(false);
        Tutorial3.SetActive(true);
    }
    void LoadTutorial4()
    {
        Tutorial3.SetActive(false);
        Tutorial4.SetActive(true);
    }
}
