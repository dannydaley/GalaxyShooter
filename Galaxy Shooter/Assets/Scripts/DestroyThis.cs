using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyThis : MonoBehaviour
{/// <summary>
/// This script when attached to game objects will Destroy that game object after a 5 second delay.
/// Made for destroying fade transitions blocking UI elements
/// </summary>
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyAfter5());
    }


    IEnumerator DestroyAfter5()
    {
        yield  return new WaitForSecondsRealtime(5);
        Destroy(this.gameObject);
    }
}
