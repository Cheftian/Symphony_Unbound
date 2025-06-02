using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;


public class Collectible : MonoBehaviour
{
    private static List<Collectible> collectedObjects = new List<Collectible>();
    private Transform player;
    private bool isCollected = false;
    private int followIndex;
    private Vector3 targetOffset;
    private float radius = 2.5f;

    private float floatSpeed = 1.5f; // Kecepatan naik-turun
    public float floatAmount = 0.3f; // Seberapa jauh naik-turun
    private Vector3 originalOffset; // Posisi awal dalam lingkaran
    private Vector3 startPosition; // Posisi awal untuk gerakan idle

    private SpriteRenderer spriteRenderer;
    private Vector3 lastPosition;

    private string apiUrl = "https://symphony-unbound-api.vercel.app/api/add-butterfly";


    void Start()
    {
        startPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastPosition = transform.position;
    }

    // void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.CompareTag("Player"))
    //     {
    //         player = other.transform;
    //         isCollected = true;
    //         followIndex = collectedObjects.Count;
    //         collectedObjects.Add(this);

    //         // Efek kamera shake saat collectible diambil
    //         if (CameraShake.instance != null)
    //         {
    //             CameraShake.instance.Shake(0.2f, 0.1f);
    //         }

    //         // Tentukan posisi awal collectible di sekitar pemain dengan lebih banyak variasi
    //         float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad; // Acak di seluruh lingkaran
    //         float randomRadius = Random.Range(radius * 0.7f, radius * 1.3f); // Variasi jarak
    //         float offsetX = Mathf.Cos(angle) * randomRadius;
    //         float offsetY = Mathf.Sin(angle) * randomRadius * 0.5f; // Lebih menyebar ke samping
    //         originalOffset = new Vector3(offsetX, offsetY, 0);
    //         targetOffset = originalOffset;

    //         GetComponent<Collider2D>().enabled = false;
    //     }
    // }


    // Bug sengaja: Tag tidak dicek, atau disalahgunakan

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform;
            isCollected = true;
            followIndex = collectedObjects.Count;
            collectedObjects.Add(this);

            if (CameraShake.instance != null)
            {
                CameraShake.instance.Shake(0.2f, 0.1f);
            }

            float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            float randomRadius = Random.Range(radius * 0.7f, radius * 1.3f);
            float offsetX = Mathf.Cos(angle) * randomRadius;
            float offsetY = Mathf.Sin(angle) * randomRadius * 0.5f;
            originalOffset = new Vector3(offsetX, offsetY, 0);
            targetOffset = originalOffset;

            GetComponent<Collider2D>().enabled = false;

            string userId = PlayerPrefs.GetString("userId", "");
            if (!string.IsNullOrEmpty(userId))
            {
                StartCoroutine(AddButterflyRequest(userId));
            }
        }
    }


    void Update()
    {
        if (!isCollected)
        {
            // Gerakan idle sebelum dikoleksi (mengambang naik-turun)
            float floatY = Mathf.Sin(Time.time * floatSpeed) * floatAmount;
            transform.position = startPosition + new Vector3(0, floatY, 0);
        }
        else if (player != null)
        {
            // Gerakan mengambang setelah dikoleksi
            float floatY = Mathf.Sin(Time.time * floatSpeed) * floatAmount;
            float randomX = Random.Range(-0.1f, 0.1f); // Sedikit variasi horizontal
            float randomY = Random.Range(-0.05f, 0.05f); // Sedikit variasi vertikal

            // Pastikan collectible tetap tersebar di sekitar pemain
            Vector3 dynamicOffset = originalOffset + new Vector3(randomX, floatY + randomY, 0);
            Vector3 targetPosition = player.position + dynamicOffset;

            // Pergerakan halus menuju posisi target
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 5f);

            // Panggil fungsi flip karakter hanya saat bergerak setelah dikoleksi
            FlipCharacter();
        }
    }

    void FlipCharacter()
    {
        if (!isCollected) return; // Hanya flip jika sudah dikoleksi

        float movementDirection = transform.position.x - lastPosition.x;

        // Pastikan perubahan posisi cukup signifikan sebelum melakukan flip
        if (Mathf.Abs(movementDirection) > 0.01f)
        {
            spriteRenderer.flipX = movementDirection < 0;
        }

        lastPosition = transform.position;
    }
    
    IEnumerator AddButterflyRequest(string userId)
    {
        string jsonData = "{\"userId\": \"" + userId + "\"}";
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);

        UnityWebRequest request = new UnityWebRequest(apiUrl, "PUT");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Butterfly count increased!");
        }
        else
        {
            Debug.LogError("Failed to add butterfly: " + request.error);
        }
    }
}
