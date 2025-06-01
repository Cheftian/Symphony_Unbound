using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseOverlay; // Panel overlay pause
    public GameObject pauseIcon;    // Icon pause
    public GameObject objectivebox; 
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // Tekan ESC untuk pause
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        pauseOverlay.SetActive(isPaused);
        pauseIcon.SetActive(!isPaused); // Sembunyikan icon pause saat overlay aktif
        objectivebox.SetActive(!isPaused); // Sembunyikan icon pause saat overlay aktif

        if (isPaused)
        {
            Time.timeScale = 0f; // Pause game
        }
        else
        {
            Time.timeScale = 1f; // Resume game
        }
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseOverlay.SetActive(false);
        pauseIcon.SetActive(true); // Munculkan kembali icon pause
        objectivebox.SetActive(true); // Munculkan kembali icon pause
        Time.timeScale = 1f; // Lanjutkan game
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f; // Pastikan waktu berjalan kembali normal
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
