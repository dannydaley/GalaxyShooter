using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponL : MonoBehaviour
{
    public Transform firePoint;

    public GameObject bulletPrefab;
    // Start is called before the first frame update



    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("FireL"))
        {
        Shoot();
        }
            
    }

    void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}
