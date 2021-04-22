using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject enemy;
    public int hp = 100;
    public Collider collider;
    // Start is called before the first frame update

    public GameObject asteroidExplosion;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (hp < 1)
        {

            StartCoroutine(wait());
            //Destroy(this.gameObject);
        }
        if (this.gameObject.transform.position.z < (GameObject.FindGameObjectWithTag("Player").transform.position.z - 10))
        {
            Destroy(gameObject);
        }
    }
    IEnumerator wait()
    {
        hp = 1000000;
        Debug.Log("loop");
        Destroy(collider);    
        Instantiate(asteroidExplosion, this.gameObject.transform.position, this.gameObject.transform.rotation);
            GameObject.FindGameObjectWithTag("Player").transform.GetComponent<Score>().score += 1000;
        yield return new WaitForSeconds(0.2f);
        Destroy(this.gameObject);
    }
    
    
    private void OnTriggerEnter(Collider collision)
    {
        /*
        if (collision.gameObject.tag == "Projectile")
        {
            hp -= 10;
            Debug.Log("Hiit : " + hp);
        }
        */
    }
    
    
}
