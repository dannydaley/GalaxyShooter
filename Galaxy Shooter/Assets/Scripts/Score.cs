using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Score : MonoBehaviour
{
    public int score = 0;

    public GameObject player;

    public GameObject scoreText;


    private void FixedUpdate()
    {
        score += ((int)player.transform.GetComponent<Movement>().forwardMovement / 10);
        scoreText.transform.GetComponent<TMPro.TextMeshProUGUI>().text = score.ToString();
           
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
