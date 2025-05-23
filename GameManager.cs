using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool isPaused;
    public GameObject img;

    // Start is called before the first frame update
    void Start()
    {
        if(Time.timeScale == 0f)
        {
            Resume();
        }
    }

    // Update is called once per frame
    void Update()
    {
        ReturnGame();
        CallPauseResume();
    }

    public void ReturnGame()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            SceneManager.LoadScene(0);
        }
    }

    public void CallPauseResume()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        isPaused = true;
        img.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        isPaused = false;
        img.SetActive(false);
    }
}
