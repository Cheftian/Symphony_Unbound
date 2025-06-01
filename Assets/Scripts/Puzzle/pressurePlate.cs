using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    private Vector3 originalPosition;
    private Vector3 pressedPosition;
    private bool isPressed = false;
    private PlatePuzzle puzzle; // Puzzle utama
    private Rigidbody2D rb;
    private float moveSpeed = 2f;
    private AudioSource audioSource;
    
    public AudioClip pressSound;
    public AudioClip releaseSound;
    public int plateID; // ID unik untuk setiap plate

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        originalPosition = transform.position;
        pressedPosition = originalPosition - new Vector3(0, transform.localScale.y / 2, 0);
    }

    public void SetPuzzle(PlatePuzzle puzzleReference)
    {
        puzzle = puzzleReference; // Sambungkan ke puzzle utama
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && IsPlayerAbove(collision.gameObject) && !isPressed)
        {
            isPressed = true;
            PlaySound(pressSound);
            StopAllCoroutines();
            StartCoroutine(MovePlate(pressedPosition));
            puzzle?.ActivatePlate(plateID); // Kirim ID plate ke puzzle
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && isPressed)
        {
            isPressed = false;
            PlaySound(releaseSound);
            StopAllCoroutines();
            StartCoroutine(MovePlate(originalPosition));
        }
    }

    private System.Collections.IEnumerator MovePlate(Vector3 targetPosition)
    {
        while (Vector3.Distance(rb.position, targetPosition) > 0.01f)
        {
            rb.MovePosition(Vector3.MoveTowards(rb.position, targetPosition, moveSpeed * Time.fixedDeltaTime));
            yield return new WaitForFixedUpdate();
        }
    }

    private bool IsPlayerAbove(GameObject player)
    {
        Collider2D playerCollider = player.GetComponent<Collider2D>();
        return playerCollider.bounds.min.y >= transform.position.y;
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
