using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//from Leah Marshall MPIE menu.cs
public class MainMenu : MonoBehaviour
{
    //press start -> change to first scene
    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    //press exit -> exit game
    public void Exit()
    {
        Application.Quit();
    }
}
