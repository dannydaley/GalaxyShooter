using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Uduino;
using UnityEngine.SceneManagement;


public class Movement : MonoBehaviour
{
    //Space background
    public GameObject spacePlanes;

    public int spacePlanesDistance;
    //public Transform distancePlane;
    public GameObject ship;

    //Text for Score counter
    public GameObject HiScore;

    //Global HP variable
    public int hp = 100;

    public GameObject DeathExposion;

    public GameObject HitExplosion;

    //Text for Hp
    public GameObject HPText;


    //forward movement
    public float forwardMovement = 0;

    //Speed that movement along the Z axis travels
    public float speed = 10f;

    //sensitivity
    public float sensitivity = 200f;

    //if true control for movement will look for uduino input
    public bool UseUduino;



    public float XYSmoothing;


    //data coming from uduino sketch
    float d = 0;

    //Right sensor reading
    float R;
    //left sensor reading
    float L;

    
    float smoothL;

    float smoothR;

    float Lvelocity;

    float Rvelocity;

    public float smoothTime;

    public bool enableSmooth = false;
    public bool hideWhenTooFar = false;

    //Observed Min and Max reading from sensors (for quick reference in editor only)
    public float MinReading = 0;
    public float MaxReading = 160;

    //Value for distance to trigger ascent
    public float Ascent;

    //value for distance to trigger descent
    public float Descent;

    //value for tolerance of difference in L and R values
    public float Tolerance;

    //value for difference in values to trigger banking
    public float Bank;

    //Left and Right potentiometer values
    public int leftPotValue = 0;
    public int rightPotValue = 0;

    public float maxDistance = 200;




    void Start()
    {
        UduinoManager.Instance.OnDataReceived += DataReceived;

    }


    /// <summary>
    /// This method pulls raw data from the arduino in the format of "L100", "R60", "L90",
    /// it checks the starting letter and seperates the values in to independant L and R variables
    /// </summary>
    /// <param name="data"></param>
    /// <param name="baord"></param>    
    void DataReceived(string data, UduinoDevice baord)
    {
        if (data[0].ToString() == "R") //check if first letter of data is "R"
        {           
            R = float.Parse(data.Substring(1)); // removes first letter from the STRING of data and parses it to float
            //Debug.Log(R);   //print value of R to console

            smoothR = Mathf.SmoothDamp(smoothR, R, ref Rvelocity, smoothTime);
        }
        else if (data[0].ToString() == "L")    //check if first letter of data is "L"
        {
            L = float.Parse(data.Substring(1)); // removes first letter from the STRING of data and parses it to float
                                                //Debug.Log(L);   //print value of L to console
            smoothL = Mathf.SmoothDamp(smoothL, L, ref Lvelocity, smoothTime);
        }
    }

    /// <summary>
    /// This Method controls ascent and descent, if both values are under the 'Ascent' variable, ascent will be triggered,
    /// The opposite for descent, alternatively arro keys can be pressed
    /// </summary>
    void YPositionUpdate()
    {
        if (Input.GetButton("Up") || (UseUduino && smoothL <= Ascent && smoothR <= Ascent))  // if up arrow key is pressed
        {
            if (ship.transform.position.y > 10)
            {

                return;
            }
            else
            {                              
                //Add force to ship along the Y Axis
                ship.transform.GetComponent<Rigidbody>().AddForce(0, sensitivity, 0, ForceMode.Acceleration);

            }
        }
        else if (Input.GetButton("Down")  || (UseUduino && smoothL >= Descent && smoothR >= Descent))   // if down arrow key is pressed 
        {
           // Debug.Log("DOWN");
            if (ship.transform.position.y < -10)
            {
                return;
            }
            else
            {
                //Add force to ship along the Y Axis
                ship.transform.GetComponent<Rigidbody>().AddForce(0, -sensitivity, 0, ForceMode.Acceleration);
            }
            
        }
        
    }

    /// <summary>
    /// This method controls the banking left and right, if the left sensor reads the lower value AND clears the tolerance, it will bank left
    /// similarly, right reading the lower value will trigger right banking
    /// </summary>
    void XPositionUpdate()
    {
        if (Input.GetButton("Left") || (UseUduino && smoothL < smoothR - Tolerance))    // if left arrow key is pressed OR left sensor reads a lower value
        {

            //Add force to ship along the X Axis
             ship.transform.GetComponent<Rigidbody>().AddForce(-sensitivity, 0, 0, ForceMode.Acceleration);
            

        }
        else if (Input.GetButton("Right") || (UseUduino && smoothR < smoothL -  Tolerance ))  //if right arrow key is pressed OR right sensor reads a lower value
        {

            //Add force to ship along the X Axis
            ship.transform.GetComponent<Rigidbody>().AddForce(sensitivity, 0, 0, ForceMode.Acceleration);
        }
    }

    //Fixed update to handle hp
    private void FixedUpdate()
    {
        HPText.transform.GetComponent<TMPro.TextMeshProUGUI>().text = hp.ToString();
        spacePlanes.transform.position = new Vector3(0, 0,ship.transform.position.z + spacePlanesDistance);
    }

    void Update()
    {
        if (hp < 1)
        {
            StartCoroutine(die());
        }
        // increment forward movement each update
        forwardMovement += speed * Time.deltaTime;

        //update ship position
       ship.transform.position = new Vector3(ship.transform.position.x, ship.transform.position.y, forwardMovement);

        // run the x position update method
        XPositionUpdate();

        //run the y position update method
        YPositionUpdate();       


    }

    IEnumerator die()
    {
        if (this.gameObject.transform.GetComponent<Score>().score > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", this.gameObject.transform.GetComponent<Score>().score);
        }
        if(speed > 0)
        {
            speed -= 0.5f;
        }
        else
        {
            SceneManager.LoadScene(3);
             
        }
        Instantiate(DeathExposion, this.gameObject.transform.position, this.gameObject.transform.rotation);
        yield return new WaitForSeconds(1f);
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "Enemy" && hp > 0)
        {
            Instantiate(HitExplosion, this.gameObject.transform.position, this.gameObject.transform.rotation);
            hp -= 10;
        }
    }

}
