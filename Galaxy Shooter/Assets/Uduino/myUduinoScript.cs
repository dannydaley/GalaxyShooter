using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System;
public class myUduinoScript : MonoBehaviour
{
    public float distance;

    SerialPort stream = new SerialPort(@"\\.\" + "COM5", 9600); // Use this for initialization
    void Start () {
        stream.Open(); 
   
        //StartCoroutine(GetDistance());
        //StartCoroutine(BlinkLoopL());
        //stream.ReadTimeout = 25;
    } // Update is called once per frame


    private int FixedUpdate()
    {
                    int data = stream.ReadByte();
            Debug.Log(data);
           // yield return new WaitForFixedUpdate();
            return data;
    }
    void Update () {
        FixedUpdate();
       // GetDistance();
/*
    Vector2 temp = transform.position; if (stream.IsOpen)
        {
/*
try {



    data = Mathf.Clamp(data, 5, 25);

data -= 5; data /= 20; data *= 10; data -= 5;

temp.x = data;
                

} catch (System.Exception)
{

    Debug.Log("timeout");

}

transform.position = temp;

}
        }*/

}}
