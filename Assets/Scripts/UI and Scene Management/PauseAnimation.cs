using UnityEngine;
using System.Collections.Generic;

public class PauseAnimation : MonoBehaviour
{
    public Animator pauseAnimator;
    public GameObject pauseOverlay;

    public void ShowPause()
    {
        pauseOverlay.SetActive(true);
        pauseAnimator.SetTrigger("SlideIn");
    }

    public void HidePause()
    {
        pauseAnimator.SetTrigger("SlideOut");
        Invoke("DeactivatePause", 0.5f); // Delay sebelum disable panel
    }

    void DeactivatePause()
    {
        pauseOverlay.SetActive(false);
    }
}
