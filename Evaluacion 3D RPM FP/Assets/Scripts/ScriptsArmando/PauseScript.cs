using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("Pause Screen")]
    public GameObject pauseMenuUI;
    public bool isPaused = false; 

    private void Start()
    {
        pauseMenuUI.SetActive(false);
        isPaused = false;
        Cursor.visible = false;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused == false)
            {
                Pause();
            }
                
            else if (isPaused == true)
            {
                Resume();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        Cursor.visible = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        Cursor.visible = true;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
        Debug.Log("Salir del juego");
    }
}


