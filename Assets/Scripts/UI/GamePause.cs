using Static_Classes;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePause : MonoBehaviour
{
    [SerializeField] private GameObject pauseCanvas;
    [SerializeField] private string forbiddenScene = "MainMenu";

    private bool isPaused = false;

    private void Awake()
    {
        if (pauseCanvas != null)
            pauseCanvas.SetActive(false);
    }

    private void Update()
    {
        if (IsCurrentSceneForbidden())
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();

        if (isPaused && Input.GetMouseButtonDown(0))
            ResumeGame();
    }

    private bool IsCurrentSceneForbidden()
    {
        return SceneManager.GetActiveScene().name == forbiddenScene;
    }

    public void TogglePause()
    {
        if (IsCurrentSceneForbidden())
            return;

        isPaused = !isPaused;

        if (isPaused)
            PauseGame();
        else
            ResumeGame();
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
        AudioListener.pause = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        pauseCanvas?.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        pauseCanvas?.SetActive(false);
        isPaused = false;
    }
}
