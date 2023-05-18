using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pause_menu;
    public bool is_paused;

    // Start is called before the first frame update
    void Start()
    {
        is_paused = false;
        pause_menu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if(is_paused)
        {
            Cursor.visible = true;
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(!is_paused)
            {
                Pause();
            }

            else
            {
                Resume();
            }
        }
    }

    public void Pause()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        //https://docs.unity3d.com/ScriptReference/Time-timeScale.html
        is_paused = true;
        Time.timeScale = 0f;
        pause_menu.SetActive(true);
    }

    public void Resume()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        is_paused = false;
        Time.timeScale = 1f;
        pause_menu.SetActive(false);
    }
}
