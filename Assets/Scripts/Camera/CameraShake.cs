using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;
    private Vector3 originalPosition;

    void Awake()
    {
        instance = this;
    }

    public void Shake(float duration, float magnitude)
    {
        originalPosition = transform.position;
        StartCoroutine(ShakeCoroutine(duration, magnitude));
    }

    private IEnumerator ShakeCoroutine(float duration, float magnitude)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float offsetX = Random.Range(-1f, 1f) * magnitude;
            float offsetY = Random.Range(-1f, 1f) * magnitude;

            transform.position = originalPosition + new Vector3(offsetX, offsetY, 0);
            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.position = originalPosition;
    }
}
