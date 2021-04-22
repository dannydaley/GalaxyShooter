using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Projectile : MonoBehaviour
{
    public float Velocity = 20f;

    public GameObject Weapon;

    public Rigidbody rb;

    public float Power;

    public GameObject projectileHit;


    void Start()
    {
        
    }






    // Update is called once per frame
    void Update()
    {
        rb.velocity = transform.forward * Velocity;
        
        if (rb.transform.position.z > (GameObject.FindGameObjectWithTag("Player").transform.position.z + 100)){
            Destroy(gameObject);
        }
        
        
        //Debug.Log(collision.name);
       // Destroy(gameObject);


    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Enemy")            
        {

            collision.transform.GetComponent<Enemy>().hp -= (int)Power;
            Instantiate(projectileHit, this.gameObject.transform.position, this.gameObject.transform.rotation);

            Destroy(this.gameObject);
                    }
        
    }
    float Remap(float inpVal, float fromMin, float fromMax, float toMin, float toMax)
    {
        float i = ((inpVal - fromMin) / (fromMax - fromMin)) * (toMax - toMin) + toMin;
        return Mathf.Clamp(i, toMin, toMax);
    }

}

