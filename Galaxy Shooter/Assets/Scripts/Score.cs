using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Score : MonoBehaviour
{
    /// <summary>
    /// this script handles the score over distance travlled by the player GameObject,
    /// a reducer is applied to avoid massive numbers
    /// </summary>
    /// 

    //current score
    public int score = 0;

    //Player gameobject
    public GameObject player;

    //Text slot for score on screen
    public Text scoreText;

    //Score reducer
    public int scoreReducer = 1000;
    
    private void FixedUpdate()
    {
        //increment the score by (distance travelled along the z axis / the scoreReducer)
        score += ((int)player.transform.GetComponent<Movement>().forwardMovement / scoreReducer);

        //update the onscreen score with the new score
        scoreText.transform.GetComponent<Text>().text = score.ToString();           
    }
}
