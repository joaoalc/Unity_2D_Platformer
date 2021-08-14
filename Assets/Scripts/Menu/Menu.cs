using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    public void Play()
    {
        SceneManager.LoadScene("Game");
    }

    public void Exit()
    {
        Application.Quit();
        return;
    }

    public void OptionsMenu(GameObject mainMenu, GameObject optionsMenu, bool active)
    {
        mainMenu.SetActive(!active);
        mainMenu.SetActive(active);
    }
    public void OptionsMenu( bool active)
    {
        return;
    }
}
