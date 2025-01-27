using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button_Script : MonoBehaviour
{
    public GameObject pausescreen;

    public bool pausescreenactive = false;

    public void Start()
    {
        Time.timeScale = 1.0f;
    }

    public void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
            if(pausescreenactive == false)
            {
                pausescreen.SetActive(true);
                pausescreenactive = true;
                Time.timeScale = 0.0f;
            }
            else
            {
                pausescreen.SetActive(false);
                pausescreenactive = false;
                Time.timeScale = 1.0f;
            }
    }

    public void Game()
    {
        // Switch to the scene with build index 1
        SceneManager.LoadScene(1);
    }

    public void Menu()
    {
        // Switch to the scene with build index 0
        SceneManager.LoadScene(0);
    }

    public void Resume()
    {
        pausescreen.SetActive(false);
        pausescreenactive = false;
        Time.timeScale = 1.0f;
    }

    public void Controls()
    {
        // Switch to the scene with build index 4
        SceneManager.LoadScene(4);
    }
}
