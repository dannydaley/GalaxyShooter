using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Uduino;
using UnityEngine.UI;



public class Shooting : MonoBehaviour
{


    //Firepoint for left lazer
    public Transform firePointL;

    //firepoint for right lazer
    public Transform firePointR;

    //prefab of bullet to fire
    public GameObject bulletPrefab;

    //Left and Right potentiometer values
    public float leftPotValue;
    public float rightPotValue;

    //power gauge for left lazer
    public Slider leftGuage;

    //power gauge for left lazer
    public Slider rightGuage;


    //public float powerChargeTime;

    public float maxCharge = 1024;


    public float fillSpeed = 1.0f;

    
    private RectTransform fillRect;
    private float targetValue = 0f;
    private float curValue = 0f;

    float remappedLeftPotValue;
    float remappedRightPotValue;
    float Remap(float inpVal, float fromMin, float fromMax, float toMin, float toMax)
    {
        float i = ((inpVal - fromMin) / (fromMax - fromMin)) * (toMax - toMin) + toMin;
        return Mathf.Clamp(i, toMin, toMax);
    }


    const int redLedL = 4;
    const int orangeLedL = 3;
    const int greenLedL = 2;

    const int redLedR = 13;
    const int orangeLedR = 12;
    const int greenLedR = 11;



    // Start is called before the first frame update
    void Start()
    {
        UduinoManager.Instance.pinMode(AnalogPin.A2, PinMode.Input); //Left potentiometer
        UduinoManager.Instance.pinMode(AnalogPin.A3, PinMode.Input); // Right potentiometer
        UduinoManager.Instance.OnDataReceived += DataReceived;


        UduinoManager.Instance.pinMode(redLedL, PinMode.Output);
        UduinoManager.Instance.pinMode(orangeLedL, PinMode.Output);
        UduinoManager.Instance.pinMode(greenLedL, PinMode.Output);

        UduinoManager.Instance.pinMode(redLedR, PinMode.Output);
        UduinoManager.Instance.pinMode(orangeLedR, PinMode.Output);
        UduinoManager.Instance.pinMode(greenLedR, PinMode.Output);
         
        
        StartCoroutine(BlinkLoopL());
    }

    IEnumerator BlinkLoopL()
    {
        while (true)
        {
            if (leftPotValue > 800)
            {
                UduinoManager.Instance.digitalWrite(orangeLedL, State.LOW);
                UduinoManager.Instance.digitalWrite(redLedL, State.LOW);
                UduinoManager.Instance.digitalWrite(greenLedL, State.HIGH);
            }
            else if (leftPotValue < 800 && leftPotValue > 400)
            {
                UduinoManager.Instance.digitalWrite(greenLedL, State.LOW);
                UduinoManager.Instance.digitalWrite(redLedL, State.LOW);
                UduinoManager.Instance.digitalWrite(orangeLedL, State.HIGH);
            }
            else if (leftPotValue < 400)
            {
                UduinoManager.Instance.digitalWrite(orangeLedL, State.LOW);
                UduinoManager.Instance.digitalWrite(greenLedL, State.LOW);
                UduinoManager.Instance.digitalWrite(redLedL, State.HIGH);
            }

            if (rightPotValue > 800)
            {
                UduinoManager.Instance.digitalWrite(orangeLedR, State.LOW);
                UduinoManager.Instance.digitalWrite(redLedR, State.LOW);
                UduinoManager.Instance.digitalWrite(greenLedR, State.HIGH);
            }
            else if (rightPotValue < 800 && rightPotValue > 400)
            {
                UduinoManager.Instance.digitalWrite(greenLedR, State.LOW);
                UduinoManager.Instance.digitalWrite(redLedR, State.LOW);
                UduinoManager.Instance.digitalWrite(orangeLedR, State.HIGH);
            }
            else if (rightPotValue < 400)
            {
                UduinoManager.Instance.digitalWrite(orangeLedR, State.LOW);
                UduinoManager.Instance.digitalWrite(greenLedR, State.LOW);
                UduinoManager.Instance.digitalWrite(redLedR, State.HIGH);
            }

            yield return new WaitForSeconds(0.01f);
        }
    }
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
        leftGuage.value += (100 * Time.deltaTime);
        rightGuage.value += (100 * Time.deltaTime);
        Debug.Log(Time.deltaTime);
        leftPotValue = UduinoManager.Instance.analogRead(AnalogPin.A2);
        rightPotValue = UduinoManager.Instance.analogRead(AnalogPin.A3);
        //Debug.Log("Left Reading: " + leftPotValue);
        //Debug.Log("Right Reading:" + rightPotValue);
        if (Input.GetButtonDown("FireR"))
        {
            ShootR();
        }
        if (Input.GetButtonDown("FireL"))
        {
            ShootL();
        }
    }

    void ShootL()
    {
        remappedLeftPotValue = Remap(leftPotValue, 1024, 0, 0, 1024);
        if (leftGuage.value >= (int)remappedLeftPotValue)
        {
        Instantiate(bulletPrefab, firePointL.position, firePointL.rotation).transform.GetComponent<Projectile>().Power = remappedLeftPotValue;
        leftGuage.value -= (int)remappedLeftPotValue;
        } 

    }

    void ShootR()
    {

        remappedRightPotValue = Remap(rightPotValue, 1024, 0, 0, 1024);
        if (rightGuage.value >= (int)remappedRightPotValue)
        {
            Instantiate(bulletPrefab, firePointR.position, firePointR.rotation).transform.GetComponent<Projectile>().Power = remappedRightPotValue;
            rightGuage.value -= (int)remappedRightPotValue;

        }

    }
}
