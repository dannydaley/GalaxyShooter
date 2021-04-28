using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //rigid body attached to game object
    public Rigidbody rb;

    public GameObject enemy;

    //this enemys hp
    public int hp = 100;

    //this enemys collider
    public Collider collider;
    // Start is called before the first frame update

    //asteroid explosion prefab, a particle effect
    public GameObject asteroidExplosion;

    // Update is called once per frame
    void Update()
    {
        //if this enemys hp drops below 0
        if (hp < 1)
        {
            //start the wait co-routine
            StartCoroutine(wait());
        }
        //if the enemy has been passed by the player
        if (this.gameObject.transform.position.z < (GameObject.FindGameObjectWithTag("Player").transform.position.z - 10))
        {
            //destroy this enemy
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// This coroutine triggers when HP drops below 0,
    /// it sets the hp to 100000 to preventr the object from destroying itself instantly, 
    /// spawns the explosion animation,
    /// increases the score, waits for 0.2 seconds and then destroys the gameobject
    /// </summary>
    /// <returns></returns>
    IEnumerator wait()
    {
        //set hp to prevent destroy
        hp = 1000000;

        //destroy the collider
        Destroy(collider);    

        //spawn explosion animation
        Instantiate(asteroidExplosion, this.gameObject.transform.position, this.gameObject.transform.rotation);
        
        //add points to players score
        GameObject.FindGameObjectWithTag("Player").transform.GetComponent<Score>().score += 1000;

        //pause to allow animation to play
        yield return new WaitForSeconds(0.2f);

        //Destroy this gameobject
        Destroy(this.gameObject);
    }    
}
