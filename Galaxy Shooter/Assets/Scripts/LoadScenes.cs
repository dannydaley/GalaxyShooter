using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScenes : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadMainMenu()
    {

        SceneManager.LoadScene(0);

    }

    public void LoadOptions()
    {
        SceneManager.LoadScene(1);

    }

    public void LoadGame()
    {
        SceneManager.LoadScene(2);
    }

    public void LoadDeath()
    {
        SceneManager.LoadScene(3);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
