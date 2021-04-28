using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScenes : MonoBehaviour
{
    /// <summary>
    /// This script is attached to a prefab in every game scene to control scene loading functions 
    /// and fade animations between scenes
    /// </summary>


    //Prefab for fade out animation prefab
    public GameObject FadeOutPrefab;

    /// <summary>
    /// /these methods are called to invoke each scenes load coroutine
    /// </summary>
    public void LoadMainMenu()
    {
        StartCoroutine(FadeOutMain());
    }

    public void LoadOptions()
    {
        StartCoroutine(FadeOutOptions());
    }

    public void LoadGame()
    {
        StartCoroutine(FadeOutGame());       
    }

    public void LoadDeath()
    {
        StartCoroutine(FadeOutDeath());       
    }

    public void QuitGame()
    {
        StartCoroutine(FadeOutQuit());
    }


    /// <summary>
    /// These co-routines play a fade animation, pause andf then load the next scene
    /// </summary>
    /// <returns></returns>
    IEnumerator FadeOutQuit()
    {        
        Instantiate(FadeOutPrefab, this.gameObject.transform.position, this.gameObject.transform.rotation);
        yield return new WaitForSecondsRealtime(3);
        Application.Quit();
    }
    IEnumerator FadeOutDeath()
    {
        Instantiate(FadeOutPrefab, this.gameObject.transform.position, this.gameObject.transform.rotation);
        yield return new WaitForSecondsRealtime(3);
        SceneManager.LoadScene(3);
    }

    IEnumerator FadeOutGame()
    {
        Instantiate(FadeOutPrefab, this.gameObject.transform.position, this.gameObject.transform.rotation);
        yield return new WaitForSecondsRealtime(3);
        SceneManager.LoadScene(2);
    }

    IEnumerator FadeOutOptions()
    {
        Instantiate(FadeOutPrefab, this.gameObject.transform.position, this.gameObject.transform.rotation);
        yield return new WaitForSecondsRealtime(3);
        SceneManager.LoadScene(1);
    }

    IEnumerator FadeOutMain()
    {
        Instantiate(FadeOutPrefab, this.gameObject.transform.position, this.gameObject.transform.rotation);
        yield return new WaitForSecondsRealtime(3);
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// These methods control the pusing of the game by stopping and resuming the games time scaling
    /// </summary>
    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void resumeGame()
    {
        Time.timeScale = 1;
    }
}
