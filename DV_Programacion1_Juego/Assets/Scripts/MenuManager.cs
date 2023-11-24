using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void GoToLevel()
    {
        SceneManager.LoadScene("Nivel_1");
    }
    
    public void ExitGame()
    {
        Application.Quit();
    }
}
