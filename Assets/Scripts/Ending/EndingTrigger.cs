using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Text;
using System.Collections;


public class EndingTrigger : MonoBehaviour
{
    [SerializeField] private string endingSceneName;
    [SerializeField] private float waitTime = 3f;
    [SerializeField] private GameObject popupObject;

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
            if (popupObject != null)
            {
                popupObject.SetActive(true); // Tampilkan popup saat masuk
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            timeSpent = 0f;
            if (popupObject != null)
            {
                popupObject.SetActive(false); // Sembunyikan popup saat keluar
            }
        }
    }

    private void Update()
    {
        if (isPlayerInside && playerRb != null && playerRb.linearVelocity.magnitude < 0.1f)
        {
            timeSpent += Time.deltaTime;
            if (timeSpent >= waitTime)
            {
                int endingNumber = 0;

                if (endingSceneName == "Ending1") endingNumber = 1;
                else if (endingSceneName == "Ending2") endingNumber = 2;
                else if (endingSceneName == "Ending3") endingNumber = 3;

                StartCoroutine(AddEndingRequest(endingNumber));
                enabled = false;
            }
        }
        else
        {
            timeSpent = 0f;
        }
    }


    IEnumerator AddEndingRequest(int endingNumber)
    {
        string userId = PlayerPrefs.GetString("userId", "");
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogWarning("User ID tidak ditemukan.");
            yield break;
        }

        string jsonData = "{\"userId\": \"" + userId + "\", \"endingNumber\": " + endingNumber + "}";
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);

        UnityWebRequest request = new UnityWebRequest("https://symphony-unbound-api.vercel.app/api/add-ending", "PUT");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Ending berhasil disimpan!");
        }
        else
        {
            Debug.LogError("Gagal menyimpan ending: " + request.error);
        }

        // Setelah request selesai, pindah ke scene ending
        SceneManager.LoadScene(endingSceneName);
    }

}
