using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    public Vector3 newOffset = new Vector3(0, 4, -8); // Offset baru untuk kamera
    public float newFOV = 60f; // Field of View baru (untuk kamera perspektif)
    public float newOrthoSize = 5f; // Orthographic Size baru (untuk kamera ortografik)
    public float transitionSpeed = 2f; // Kecepatan transisi perubahan kamera

    public void TriggerCameraChange()
    {
        CameraFollow cameraFollow = Camera.main.GetComponent<CameraFollow>();
        Camera mainCamera = Camera.main;

        if (cameraFollow != null)
        {
            cameraFollow.SetOffset(newOffset);
        }

        if (mainCamera != null)
        {
            if (mainCamera.orthographic)
            {
                // Jika kamera ortografik, ubah ukuran ortografik
                StartCoroutine(ChangeOrthoSize(mainCamera, newOrthoSize));
            }
            else
            {
                // Jika kamera perspektif, ubah FOV
                StartCoroutine(ChangeFOV(mainCamera, newFOV));
            }
        }
    }

    // Coroutine untuk smooth transition FOV
    private System.Collections.IEnumerator ChangeFOV(Camera cam, float targetFOV)
    {
        float startFOV = cam.fieldOfView;
        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            cam.fieldOfView = Mathf.Lerp(startFOV, targetFOV, elapsedTime);
            elapsedTime += Time.deltaTime * transitionSpeed;
            yield return null;
        }

        cam.fieldOfView = targetFOV;
    }

    // Coroutine untuk smooth transition Ortho Size
    private System.Collections.IEnumerator ChangeOrthoSize(Camera cam, float targetSize)
    {
        float startSize = cam.orthographicSize;
        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            cam.orthographicSize = Mathf.Lerp(startSize, targetSize, elapsedTime);
            elapsedTime += Time.deltaTime * transitionSpeed;
            yield return null;
        }

        cam.orthographicSize = targetSize;
    }
}
