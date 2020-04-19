using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenStateManager : MonoBehaviour
{
    // Start is called before the first frame update
    public void LoadGame()
    {
        SceneManager.LoadScene("Level1",LoadSceneMode.Single);
    }
    public void ExitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

}
