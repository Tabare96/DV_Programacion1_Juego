using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // en el Menu
    public void GoToLevel()
    {
        SceneManager.LoadScene("Nivel_1Tab");
    }
    
    public void ExitGame()
    {
        Application.Quit();
    }

    // en el GameOver
    public void GoToMenu()
    {
        SceneManager.LoadScene("MenuTab");
    }

}
