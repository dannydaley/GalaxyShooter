using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{


    public GameObject ship;
    public void switchUduino()
    {

        ship.transform.GetComponent<Movement>().UseUduino = !ship.transform.GetComponent<Movement>().UseUduino;
    }
}
