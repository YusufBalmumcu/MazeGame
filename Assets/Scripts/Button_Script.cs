using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button_Script : MonoBehaviour
{
    public void Game()
    {
        // Switch to the scene with build index 1
        SceneManager.LoadScene(1);
    }
}
