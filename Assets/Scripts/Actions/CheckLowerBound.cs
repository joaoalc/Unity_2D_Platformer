using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CheckLowerBound : MonoBehaviour
{
    [SerializeField]
    float minYPlayer = -27.5f;
    bool gameOver = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.transform.position.y < minYPlayer && !gameOver)
        {
            float time = float.Parse(GameObject.FindGameObjectWithTag("TimeText").GetComponent<TextMeshProUGUI>().text);

            GameEnd.EndGame(gameObject, time);
            gameOver = true;
        }
    }

}
