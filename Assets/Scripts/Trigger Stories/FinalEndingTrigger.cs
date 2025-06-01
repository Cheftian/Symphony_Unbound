using UnityEngine;

public class FinalEndingTrigger : MonoBehaviour
{
    [SerializeField] private float waitTime = 30f; // Waktu yang harus dihabiskan di dalam trigger
    [SerializeField] private GameObject popupObject; // GameObject yang akan muncul saat pemain masuk

    private float timeSpent = 0f;
    private bool isPlayerInside = false;
    private Rigidbody2D playerRb;

    private void Start()
    {
        if (popupObject != null)
        {
            popupObject.SetActive(false); // Sembunyikan saat awal
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            playerRb = other.GetComponent<Rigidbody2D>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            timeSpent = 0f;
        }
    }

    private void Update()
    {
        if (isPlayerInside && playerRb != null && playerRb.linearVelocity.magnitude < 0.1f)
        {
            timeSpent += Time.deltaTime;
            if (timeSpent >= waitTime)
            {
                 if (popupObject != null)
                    {
                        popupObject.SetActive(true); // Sembunyikan saat awal
                    }
            }
        }
        else
        {
            timeSpent = 0f;
        }
    }
}
