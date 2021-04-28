using System.Collections;
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

    //Player game object
    public GameObject ship;

    //Text for Score counter
    public GameObject HiScore;

    //Global HP variable
    public int hp = 100;

    //Slider for onscreen hp
    public Slider HPSlider;

    //Animation prefab for when player dies
    public GameObject DeathExposion;

    //Animation prefab for when player gets hit
    public GameObject HitExplosion;

    //forward movement
    public float forwardMovement = 0;

    //Speed that movement along the Z axis travels
    public float speed = 10f;

    //sensitivity
    public float sensitivity = 200f;

    //if true control for movement will look for uduino input
    public bool UseUduino;

    //float XY data smoothing to reduce anomoles
    public float XYSmoothing;

    //Right sensor reading
    float R;

    //left sensor reading
    float L;

    //smoothed left sensor value
    float smoothL;

    //smooth right sensor value
    float smoothR;

    public GameObject FadeOutPrefab;

    //screen that displays when pausing
    public GameObject PauseScreen;

    //game object that handles scene loading and pausing
    public GameObject SceneLoader;

    #region Smoothing variables
    //left velocity
    float Lvelocity;

    //right velocity
    float Rvelocity;

    //smooth time
    public float smoothTime;

    //enable smoothing
    public bool enableSmooth = false;

    //hide when too far
    public bool hideWhenTooFar = false;
    #endregion

    #region variables control distances and tolerances
    //Value for distance to trigger ascent (close hands to controller trigger ascent)
    public float Ascent;

    //value for distance to trigger descent (far hands from controller triggers descent
    public float Descent;

    
    //value for tolerance of difference in L and R values, this allows a threshold for straight flight without banking
    public float Tolerance;

    //value for difference in values to trigger banking
    public float Bank;

    //Left and Right potentiometer values
    public int leftPotValue = 0;
    public int rightPotValue = 0;


    //maximum distances hands can be from controller
    public float maxDistance = 200;

    #endregion



    void Start()
    {
        //Check data coming in from the control-deck
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
            
            //Smooths the value coming from the right sensor to reduce anomoles and twitchy flight
            smoothR = Mathf.SmoothDamp(smoothR, R, ref Rvelocity, smoothTime);
        }
        else if (data[0].ToString() == "L")    //check if first letter of data is "L"
        {
            L = float.Parse(data.Substring(1)); // removes first letter from the STRING of data and parses it to float
                       
            //Smooths the value coming from the left sensor to reduce anomoles and twitchy flight
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
        //background moves forward with ship
        spacePlanes.transform.position = new Vector3(0, 0,ship.transform.position.z + spacePlanesDistance);
    }

    void Update()
    {
        //update the hp bar with hp value
        HPSlider.value = hp;

        if (Input.GetButton("Pause"))
        {
            PauseScreen.SetActive(true);
            SceneLoader.transform.GetComponent<LoadScenes>().PauseGame();
        }

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
        //If the high score is greater than the existing hiscore
        if (this.gameObject.transform.GetComponent<Score>().score > PlayerPrefs.GetInt("HighScore", 0))
        {
            //set the new highscore as this score
            PlayerPrefs.SetInt("HighScore", this.gameObject.transform.GetComponent<Score>().score);
        }
        //if speed is greater than 0
        if(speed > 0)
        {
            //decrease speed
            speed -= 0.1f;
        }
        else
        {
            Instantiate(FadeOutPrefab, this.gameObject.transform.position, this.gameObject.transform.rotation);
            yield return new WaitForSecondsRealtime(3);
            //If speed is 1, load the death scene
            SceneManager.LoadScene(3);             
        }
        // spawn the death explosion animation
        Instantiate(DeathExposion, this.gameObject.transform.position, this.gameObject.transform.rotation);

        //wait for 1 second
        yield return new WaitForSeconds(1f);
    }

    //This method handles collisions
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "Enemy" && hp > 0)
        {
            //spawn the hit explosion animation
            Instantiate(HitExplosion, this.gameObject.transform.position, this.gameObject.transform.rotation);

            //reduce players hp
            hp -= 10;
        }
    }

}
