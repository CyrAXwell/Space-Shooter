using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public GameObject GameOverScreenUI;
    public GameObject GameWinScreenUI;

    [SerializeField] AudioSource buttonSound;
    [SerializeField] AudioSource quitSound;

    private AudioManager _audioManager;

    void Start()
    {
        _audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        GameOverScreenUI.SetActive(false);
        GameWinScreenUI.SetActive(false);
    }

    public void GameOver()
    {
        _audioManager.PlaySFX(_audioManager.Lose);
        GameOverScreenUI.SetActive(true);
        Time.timeScale = 0f;
        StateNameController.isPaused = true;
        GameObject.Find("Wave panel").GetComponent<WaveManager>().ClearObjects();
    }

    public void GameWin()
    {
        _audioManager.PlaySFX(_audioManager.Win);
        GameWinScreenUI.SetActive(true);
        Time.timeScale = 0f;
        StateNameController.isPaused = true;
        GameObject.Find("Wave panel").GetComponent<WaveManager>().ClearObjects();
    }

    public void MainMenu()
    {
        //buttonSound.Play();
        GameOverScreenUI.SetActive(false);
        Time.timeScale = 1f;
        StateNameController.isPaused = false;
        //StartCoroutine(MainMenuWithDelay(0.2f));
        SceneManager.LoadScene(0);
    }

    public void GameQuit()
    {
        //quitSound.Play();
        //StartCoroutine(GameQuitWithDelay(0.2f));
        Application.Quit();
    }

    private IEnumerator MainMenuWithDelay(float interval)
    {
        yield return new WaitForSeconds(interval);
        SceneManager.LoadScene(0);
        
    }

    private IEnumerator GameQuitWithDelay(float interval)
    {
        yield return new WaitForSeconds(interval);
        Application.Quit();
    }
    
}
