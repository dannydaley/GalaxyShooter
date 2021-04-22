using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreScreen : MonoBehaviour
{

    public GameObject HighScore;
    // Start is called before the first frame update
    void Start()
    {
        HighScore.transform.GetComponent<TMPro.TextMeshProUGUI>().text = PlayerPrefs.GetInt("HighScore", 0).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
