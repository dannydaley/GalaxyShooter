using System.Collections;
using UnityEngine;
using Uduino;
using UnityEngine.UI;



public class Shooting : MonoBehaviour
{
    #region Variables

    public GameObject ship;

    
    //Firepoint for left lazer
    public Transform firePointL;

    //firepoint for right lazer
    public Transform firePointR;

    //prefab of bullet to fire
    public GameObject bulletPrefab;

    //Text for left power output gauge
    public Text outputL;

    //Slider for left power output gauge
    public Slider powerOutputL;

    //power gauge for left lazer
    public Slider leftGuage;

    //Text for right power output gauge
    public Text outputR;

    //slider for right power output gauge
    public Slider powerOutputR;

    //power gauge for left lazer
    public Slider rightGuage;
    //Text for left charge gauge
    public Text ChargeL;

    //Text for right charge gauge
    public Text ChargeR;

    //maximum charge for power gauges
    public float maxCharge = 1024;

    //power charge refill speed
    public float fillSpeed = 1.0f;

    //Left potentiometer value
    public float leftPotValue;

    //right potentiometer value
    public float rightPotValue;



    //Left reversed potentiometer value 
    float remappedLeftPotValue;

    //right reversed potentiometer value
    float remappedRightPotValue;

    //Control-Deck LED pin set up
    const int redLedL = 4;
    const int orangeLedL = 3;
    const int greenLedL = 2;
    const int redLedR = 13;
    const int orangeLedR = 12;
    const int greenLedR = 11;

    //Color thresholds
    int redThreshold = 200;
    int amberThreshold = 400;

    #endregion

    /// <summary>
    /// This Remap Method reverses the values coming in from the potentiometers,
    /// so that turning them clockwise increases the value
    /// </summary>
    /// <param name="inpVal"></param>
    /// <param name="fromMin"></param>
    /// <param name="fromMax"></param>
    /// <param name="toMin"></param>
    /// <param name="toMax"></param>
    /// <returns></returns>
    float Remap(float inpVal, float fromMin, float fromMax, float toMin, float toMax)
    {
        float i = ((inpVal - fromMin) / (fromMax - fromMin)) * (toMax - toMin) + toMin;
        return Mathf.Clamp(i, toMin, toMax);
    }


   /// <summary>
   /// Set up Controller potentiometer and LED pins
   /// </summary>
    void Start()
    {
        //initialise Control Deck analog pins 
        UduinoManager.Instance.pinMode(AnalogPin.A2, PinMode.Input); //Left potentiometer
        UduinoManager.Instance.pinMode(AnalogPin.A3, PinMode.Input); // Right potentiometer

        //Add incoming data to the control-deck datastream
        UduinoManager.Instance.OnDataReceived += DataReceived;

        //initialise Control-Deck LED pins
        UduinoManager.Instance.pinMode(redLedL, PinMode.Output);
        UduinoManager.Instance.pinMode(orangeLedL, PinMode.Output);
        UduinoManager.Instance.pinMode(greenLedL, PinMode.Output);
        UduinoManager.Instance.pinMode(redLedR, PinMode.Output);
        UduinoManager.Instance.pinMode(orangeLedR, PinMode.Output);
        UduinoManager.Instance.pinMode(greenLedR, PinMode.Output);
         
        //Start co-routine for LEDs to react with potentiometer values
        StartCoroutine(BlinkLoopL());
    }


    /// <summary>
    /// This co-routine lights up leds according to potentiometer values
    /// if a value is below the red threshold it will light red, 
    /// between the red and amber thresholds will light amber,
    /// and green if over the amber threshold
    /// </summary>
    /// <returns></returns>
    IEnumerator BlinkLoopL()
    {
        //always run
        while (true)
        {
            if (leftPotValue >= amberThreshold)
            {
                UduinoManager.Instance.digitalWrite(orangeLedL, State.LOW);
                UduinoManager.Instance.digitalWrite(redLedL, State.LOW);
                UduinoManager.Instance.digitalWrite(greenLedL, State.HIGH);
            }
            else if (leftPotValue < amberThreshold && leftPotValue > redThreshold)
            {
                UduinoManager.Instance.digitalWrite(greenLedL, State.LOW);
                UduinoManager.Instance.digitalWrite(redLedL, State.LOW);
                UduinoManager.Instance.digitalWrite(orangeLedL, State.HIGH);
            }
            else if (leftPotValue <= redThreshold)
            {
                UduinoManager.Instance.digitalWrite(orangeLedL, State.LOW);
                UduinoManager.Instance.digitalWrite(greenLedL, State.LOW);
                UduinoManager.Instance.digitalWrite(redLedL, State.HIGH);
            }

            if (rightPotValue > amberThreshold)
            {
                UduinoManager.Instance.digitalWrite(orangeLedR, State.LOW);
                UduinoManager.Instance.digitalWrite(redLedR, State.LOW);
                UduinoManager.Instance.digitalWrite(greenLedR, State.HIGH);
            }
            else if (rightPotValue < amberThreshold && rightPotValue > redThreshold)
            {
                UduinoManager.Instance.digitalWrite(greenLedR, State.LOW);
                UduinoManager.Instance.digitalWrite(redLedR, State.LOW);
                UduinoManager.Instance.digitalWrite(orangeLedR, State.HIGH);
            }
            else if (rightPotValue < redThreshold)
            {
                UduinoManager.Instance.digitalWrite(orangeLedR, State.LOW);
                UduinoManager.Instance.digitalWrite(greenLedR, State.LOW);
                UduinoManager.Instance.digitalWrite(redLedR, State.HIGH);
            }

            yield return new WaitForSeconds(0.01f);
        }
    }

    /// <summary>
    /// This Method constantly check from serial prints coming in from the arduino,
    /// if the data coming in is "BL" or "BR" (triggered by the left and right buttons on the Control-Deck)
    /// it will invoke the ShootL and ShootR methods
    /// </summary>
    /// <param name="data"></param>
    /// <param name="baord"></param>
    void DataReceived(string data, UduinoDevice baord)
    {
        if (data == "BL")
        {
            ShootL();
        }
        if (data == "BR")
        {
            ShootR();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // charge values if use uduino is active
        if (ship.transform.GetComponent<Movement>().UseUduino)
        {                    
        //Get the left potentiometer value from the Control-Deck analog pin
        leftPotValue = UduinoManager.Instance.analogRead(AnalogPin.A2);

        //Get the right potentiometer value from the Control-Deck analog pin
        rightPotValue = UduinoManager.Instance.analogRead(AnalogPin.A3);

        //update left power gauge with left potentiometer value
        powerOutputL.value = (int)Remap(leftPotValue, 1024, 0, 0, 1024);

        //update right power gauge with right potentiometer value
        powerOutputR.value = (int)Remap(rightPotValue, 1024, 0, 0, 1024);

        //update left output text with left potentiometer percentage
        outputL.gameObject.transform.GetComponent<Text>().text = (Remap(leftPotValue, 1024, 0, 0, 1024) / 10).ToString() + "%";

        //update right output text with right potentiometer percentage
        outputR.gameObject.transform.GetComponent<Text>().text = (Remap(rightPotValue, 1024, 0, 0, 1024) / 10).ToString() + "%";

        //update right charge text with right potentiometer percentage
        ChargeR.gameObject.GetComponent<Text>().text = (rightGuage.value / 10).ToString() + "%";

        //update right charge text with left potentiometer percentage
        ChargeL.gameObject.GetComponent<Text>().text = (leftGuage.value / 10).ToString() + "%";

        //recharge the left charge gauge over time
        leftGuage.value += (100 * Time.deltaTime);

        //recharge the right charge gauge over time
        rightGuage.value += (100 * Time.deltaTime);
    }
        //charge values if use keyboard is active
        else
        {
            // assign the remapped left pot value to the gauge value
            powerOutputL.value = remappedLeftPotValue;

            //assign the remapped right pot value to the gauge value
            powerOutputR.value = remappedRightPotValue;

            // add value to the gauges text label
            outputL.gameObject.transform.GetComponent<Text>().text = (remappedLeftPotValue / 10).ToString() + "%";

            // add value to the gauges text label
            outputR.gameObject.transform.GetComponent<Text>().text = (remappedRightPotValue / 10).ToString() + "%";

            //update charge percentage
            ChargeL.gameObject.GetComponent<Text>().text = (leftGuage.value / 10).ToString() + "%";

            //update right charge percentage
            ChargeR.gameObject.GetComponent<Text>().text = (rightGuage.value / 10).ToString() + "%";

            //recharge the left charge gauge over time
            leftGuage.value += (100 * Time.deltaTime);

            //recharge the right charge gauge over time
            rightGuage.value += (100 * Time.deltaTime);
        }
        //Lower potentiometer values through Keys
        if (Input.GetButton("minusLeft"))
        {
            if (remappedLeftPotValue > 10)
            {
                remappedLeftPotValue -= 10;
            }
        }

        //Lower potentiometer values through Keys
        if (Input.GetButton("plusLeft"))
        {
            if (remappedLeftPotValue < 1010)
            {
                remappedLeftPotValue += 10;
            }
        }

        //Lower potentiometer values through Keys
        if (Input.GetButton("minusRight"))
        {
            if (remappedRightPotValue > 10)
            {
                remappedRightPotValue -= 10;
            }
        }

        //Lower potentiometer values through Keys
        if (Input.GetButton("plusRight"))
        {
            if (remappedRightPotValue < 1010)
            {
                remappedRightPotValue += 10;
            }
        }

        //if right button is pressed shoot projectile
        if (Input.GetButton("FireR"))
        {
            ShootR();
        }
        //if left button is pressed shoot projectile
        if (Input.GetButton("FireL"))
        {
            ShootL();
        }
    }

    void ShootL()
    {
        if (ship.transform.GetComponent<Movement>().UseUduino)
        {
        //reverse the values coming from the left potentiometer
        remappedLeftPotValue = Remap(leftPotValue, 1024, 0, 0, 1024);

        }
        else
        {
            
        }


        //if the left charge gauge has enough power
        if (leftGuage.value >= (int)remappedLeftPotValue)
        {
            //spawn a bullet prefab and give it the power of the left potentiometer
            Instantiate(bulletPrefab, firePointL.position, firePointL.rotation).transform.GetComponent<Projectile>().Power = remappedLeftPotValue;

            //decrease the power gauge by that amount of power
            leftGuage.value -= (int)remappedLeftPotValue;
        } 
    }

    void ShootR()
    {
        if (ship.transform.GetComponent<Movement>().UseUduino)
        {
            //reverse the values coming from the right potentiometer
            remappedRightPotValue = Remap(rightPotValue, 1024, 0, 0, 1024);
        }
        //if the right charge gauge has enough power
        if (rightGuage.value >= (int)remappedRightPotValue)
        {
            //spawn a bullet prefab and give it the power of the right potentiometer
            Instantiate(bulletPrefab, firePointR.position, firePointR.rotation).transform.GetComponent<Projectile>().Power = remappedRightPotValue;

            //decrease the power gauge by that amount of power
            rightGuage.value -= (int)remappedRightPotValue;
        }
    }
}
