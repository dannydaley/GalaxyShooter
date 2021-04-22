using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Uduino;

public class UduinoInput : MonoBehaviour
{
    //PlayerMotor motor;
    public GameObject ship;
    public float direction;

    public int leftPotValue = 0;
    public int rightPotValue = 0;

    [Range(0, 1023)]

    public float calMin = 0f;

    [Range(0, 1023)]

    public float calMax = 1023f;
    const int rightButton = 13;
    const int leftButtonm = 1;
    /*
       
    const int orangeLedL = 3;
    const int greenLedL = 4;
    
    const int redLedR = 7;
    const int orangeLedR = 12;
    const int greenLedR = 8;
    */


    // Start is called before the first frame update
    void Start()
    {
        UduinoManager.Instance.pinMode(7, PinMode.Output);
        UduinoManager.Instance.pinMode(AnalogPin.A2, PinMode.Input); //Left potentiometer
        UduinoManager.Instance.pinMode(AnalogPin.A3, PinMode.Input); // Right potentiometer

       //UduinoManager.Instance.pinMode(rightButton, PinMode.Input);    //Left Button
        //UduinoManager.Instance.pinMode(AnalogPin.A1, PinMode.Input);    //Right button

        /*
        UduinoManager.Instance.pinMode(redLedL, PinMode.Output);
        UduinoManager.Instance.pinMode(orangeLedL, PinMode.Output);
        UduinoManager.Instance.pinMode(greenLedL, PinMode.Output);

        UduinoManager.Instance.pinMode(redLedR, PinMode.Output);
        UduinoManager.Instance.pinMode(orangeLedR, PinMode.Output);
        UduinoManager.Instance.pinMode(greenLedR, PinMode.Output);

        UduinoManager.Instance.pinMode(trigPinL, PinMode.Output); // Sets the L trigPin as an Output  
        UduinoManager.Instance.pinMode(echoPinL, PinMode.Output); //sets the L echoPin as an Input  
        */
       // StartCoroutine(BlinkLoopL());
        //StartCoroutine(BlinkLoopR());
    }

    /*
    IEnumerator BlinkLoopR()
    {
        while (true)
        {
            UduinoManager.Instance.digitalWrite(greenLedR, State.LOW);
            UduinoManager.Instance.digitalWrite(redLedR, State.HIGH);
            yield return new WaitForSeconds(0.5f);
            UduinoManager.Instance.digitalWrite(redLedR, State.LOW);
            UduinoManager.Instance.digitalWrite(orangeLedR, State.HIGH);
            yield return new WaitForSeconds(0.5f);
            UduinoManager.Instance.digitalWrite(orangeLedR, State.LOW);
            UduinoManager.Instance.digitalWrite(greenLedR, State.HIGH);
            yield return new WaitForSeconds(0.5f);
        }
    }
    IEnumerator BlinkLoopL()
    {
        while (true)
        {
           
            UduinoManager.Instance.digitalWrite(redLedR, State.LOW);
            UduinoManager.Instance.digitalWrite(redLedL, State.HIGH);
            yield return new WaitForSeconds(0.5f);

            UduinoManager.Instance.digitalWrite(redLedL, State.LOW);
            UduinoManager.Instance.digitalWrite(orangeLedL, State.HIGH);
            yield return new WaitForSeconds(0.5f);
            UduinoManager.Instance.digitalWrite(orangeLedL, State.LOW);
            UduinoManager.Instance.digitalWrite(greenLedL, State.HIGH);
            yield return new WaitForSeconds(0.5f);
            UduinoManager.Instance.digitalWrite(greenLedL, State.LOW);
            UduinoManager.Instance.digitalWrite(greenLedR, State.HIGH);
            yield return new WaitForSeconds(0.5f);
            UduinoManager.Instance.digitalWrite(greenLedR, State.LOW);
            UduinoManager.Instance.digitalWrite(orangeLedR, State.HIGH);
            yield return new WaitForSeconds(0.5f);
            UduinoManager.Instance.digitalWrite(orangeLedR, State.LOW);
            UduinoManager.Instance.digitalWrite(redLedR, State.HIGH);
            yield return new WaitForSeconds(0.5f);
        }
    }
    */
    // Update is called once per frame
    void Update()
    {
        
        bool shoot = false;
        /*
        if (UduinoManager.Instance.analogRead(AnalogPin.A4) > 100)
        {
            Debug.Log("Right - SSSSHHHHOOOOOTTTTTTT!!!");
            shoot = true;
        }
        /*
        if (UduinoManager.Instance.analogRead(AnalogPin.A1) > 100)
        {
            Debug.Log("Left - BBBOOOOOOOOOOOOOOOOOMMM!!");

        }
        */
        /*
        leftPotValue = UduinoManager.Instance.analogRead(AnalogPin.A2);
        rightPotValue = UduinoManager.Instance.analogRead(AnalogPin.A3);
        Debug.Log("Left Reading: " + leftPotValue);
        Debug.Log("Right Reading:" + rightPotValue);
        
        //Debug.Log("Button: " + UduinoManager.Instance.analogRead(AnalogPin.A5));

        // UduinoManager.Instance.digitalWrite(11, 1);
        //UduinoManager.Instance.digitalWrite(redLedL, 1);

        






        //direction = MapIntToFloat(analogValue, calMin, calMax, -1f, 1f);
        //Debug.Log(UduinoManager.Instance.analogRead(AnalogPin.A2));
        //bool shoot = Input.GetKeyDown(KeyCode.Space);

        //motor.SetInput(direction, shoot);
        //bool weaponReady = motor.WeaponReady();

        // State weaponReadyState = weaponReady ? State.HIGH : State.LOW;
        /*UduinoManager.Instance.digitalWrite(4, weaponReadyState);
        if (weaponReadyState == State.LOW)
        {
            UduinoManager.Instance.digitalWrite(3, State.HIGH);
        }
        else
        {
            UduinoManager.Instance.digitalWrite(3, State.LOW);
        }
        */

        // UduinoManager.Instance.digitalWrite(redLedL, State.HIGH);




    }

    float MapIntToFloat(int inputValue, float fromMin, float fromMax, float toMin, float toMax)
    {
        float i = ((((float)inputValue - fromMin) / (fromMax - fromMin)) * (toMax - toMin) + toMin);
        i = Mathf.Clamp(i, toMin, toMax);
        return i;
    }
}
