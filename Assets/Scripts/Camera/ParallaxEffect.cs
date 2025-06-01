using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public Transform[] backgrounds;  // Array untuk menyimpan layer background
    public float[] parallaxScales;   // Nilai perbedaan kecepatan untuk tiap layer
    public float smoothing = 1f;     // Seberapa halus efek parallax

    private Transform cam;  // Kamera utama
    private Vector3 previousCamPos;

    void Start()
    {
        cam = Camera.main.transform;
        previousCamPos = cam.position;

        // Jika nilai parallaxScales belum diatur, buat otomatis
        if (parallaxScales.Length == 0)
        {
            parallaxScales = new float[backgrounds.Length];
            for (int i = 0; i < backgrounds.Length; i++)
            {
                parallaxScales[i] = (i + 1) * 0.1f;  // Semakin jauh, semakin kecil kecepatannya
            }
        }
    }

    void Update()
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            float parallax = (previousCamPos.x - cam.position.x) * parallaxScales[i];

            Vector3 targetPos = new Vector3(
                backgrounds[i].position.x + parallax, 
                backgrounds[i].position.y, 
                backgrounds[i].position.z);

            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, targetPos, smoothing * Time.deltaTime);
        }

        previousCamPos = cam.position;
    }
}
