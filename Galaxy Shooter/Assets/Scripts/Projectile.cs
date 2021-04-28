using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Projectile : MonoBehaviour
{

    //Speed of prejectile
    public float Velocity = 20f;

    //max range of projectile
    public int maxRange = 100;


    public GameObject Weapon;

    //rigid body of projectile
    public Rigidbody rb;

    //how much damage the projectile does
    public float Power;

    //animation for when projectile makes contact
    public GameObject projectileHit;


    // Update is called once per frame
    void Update()
    {
        //apply speed to the projectiles rigid body
        rb.velocity = transform.forward * Velocity;
        
        //if projectile goes out of range
        if (rb.transform.position.z > (GameObject.FindGameObjectWithTag("Player").transform.position.z + maxRange))
        {
            //destroy projectile
            Destroy(gameObject);
        }
    }

    //This method runs when the projectile collides with something
    private void OnTriggerEnter(Collider collision)
    {
        //if object hit has the "Enemy" tag
        if (collision.gameObject.tag == "Enemy")            
        {
            //reduce the hp of the enemy by this projectiles power
            collision.transform.GetComponent<Enemy>().hp -= (int)Power;

            //spawn projectile hit animation
            Instantiate(projectileHit, this.gameObject.transform.position, this.gameObject.transform.rotation);

            //Destroy this projectile
            Destroy(this.gameObject);                    
        }        
    }
}

