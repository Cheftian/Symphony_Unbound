using UnityEngine;

public class ChaseTrigger : MonoBehaviour
{
    [SerializeField] private GameObject objekAktif;
    [SerializeField] private GameObject objekNonaktif;

    private bool isPlayerInside = false;
    private Rigidbody2D playerRb;

    private void Start()
    {
        if (objekAktif != null)
        {
            objekAktif.SetActive(false);
        }
        if (objekNonaktif != null)
        {
            objekNonaktif.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            // Efek kamera shake saat collectible diambil
            if (CameraShake.instance != null)
            {
                CameraShake.instance.Shake(0.2f, 0.1f);
            }
            playerRb = other.GetComponent<Rigidbody2D>();
            if (objekAktif != null)
            {
                objekAktif.SetActive(true);
            }
            if (objekNonaktif != null)
            {
                objekNonaktif.SetActive(false);
            }
        }
    }

    // private void OnTriggerExit2D(Collider2D other)
    // {
    //     if (other.CompareTag("Player"))
    //     {
    //         isPlayerInside = false;
    //         timeSpent = 0f;
    //         if (objekAktif != null)
    //         {
    //             objekAktif.SetActive(false);
    //         }
    //         if (objekNonaktif != null)
    //         {
    //             objekNonaktif.SetActive(true);
    //         }
    //     }
    // }
}
