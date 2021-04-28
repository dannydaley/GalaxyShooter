using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreScreen : MonoBehaviour
{

    /// <summary>
    /// This scripts gets the existing high score and displays it to the player on the DeathScene UI.
    /// 
    /// if the player has just created a new highscore, that would have been applied in the previous scene and the player will be shown the correct score
    /// </summary>
    public GameObject HighScore;
    // Start is called before the first frame update
    void Start()
    {
        HighScore.transform.GetComponent<Text>().text = PlayerPrefs.GetInt("HighScore", 0).ToString();
    }

}
