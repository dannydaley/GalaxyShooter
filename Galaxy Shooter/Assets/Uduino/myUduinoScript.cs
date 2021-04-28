using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System;
public class myUduinoScript : MonoBehaviour
{
    //creates a serial port stream \\.\COM5 with a 9600 baurd rate
    SerialPort stream = new SerialPort(@"\\.\" + "COM5", 9600); 
    void Start () {
        //opens the data stream for serial communication with the arduino
        stream.Open();
    } 
    private int FixedUpdate()
    {
        //reads data from the stream
        int data = stream.ReadByte();  
        
        //returns data
        return data;
    }
    void Update () {
        FixedUpdate();
    }
}
