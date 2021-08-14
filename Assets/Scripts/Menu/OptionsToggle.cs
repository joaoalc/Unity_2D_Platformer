using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsToggle : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject optionsMenu;

    public void ToggleOptionsMenu(bool newStatus)
    {
        mainMenu.SetActive(!newStatus);
        optionsMenu.SetActive(newStatus);
    }

}
