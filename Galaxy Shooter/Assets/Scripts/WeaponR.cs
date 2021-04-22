using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponR : MonoBehaviour
{
    public Transform firePoint;

    public GameObject bulletPrefab;
    // Start is called before the first frame update



    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("FireR"))
        {
            Shoot();
        }

    }

    void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}
