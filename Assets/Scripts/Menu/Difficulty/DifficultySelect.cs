using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DifficultySelect : MonoBehaviour
{

    public static readonly string difficulty = "Difficulty";
    [SerializeField]
    private int difficultyVal = 3;
    [SerializeField]
    private TextMeshProUGUI text;
    [SerializeField]
    private Slider slider;

    private void Start()
    {
        if(slider == null || text == null)
        {
            Debug.LogError(gameObject + " difficulty select text is missing a text or slider.");
        }
        if (!PlayerPrefs.HasKey(difficulty))
        {
            slider.value = difficultyVal;
        }
        else
        {
            slider.value = PlayerPrefs.GetInt(difficulty);
        }
    }

    public void ChangeDifficulty()
    {
        difficultyVal = (int) slider.value;
        PlayerPrefs.SetInt(difficulty, difficultyVal);
        text.text = difficultyVal.ToString();
    }

    public void SaveDifficulty()
    {
        PlayerPrefs.Save();
    }
}
