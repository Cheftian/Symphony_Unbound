using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f;
    public float sprintMultiplier = 2f;
    public float jumpForce = 10f;
    private float moveVelocity;
    private Rigidbody2D rb;
    private int jumpCount = 0;
    private int maxJumps = 2;
    private runes currentRune;
    private bool facingRight = true;
    [SerializeField] private GameObject popupObject;

    private Animator animator;
    private AudioSource audioSource;
    [SerializeField] private AudioClip walkSound;
    [SerializeField] private AudioClip runSound;
    [SerializeField] private AudioClip jumpSound;
    private bool isPlayingFootstep = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        if (popupObject != null)
        {
            popupObject.SetActive(false);
        }
    }

    void HandleAnimationsAndSound()
    {
        // Animasi berhenti saat tidak ada gerakan
        if (moveVelocity == 0)
        {
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsJumping", false);  // Pastikan animasi lompat dihentikan saat tidak bergerak
            StopFootstepSound();
        }
        else
        {
            // Kondisi jika karakter sedang berlari
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                animator.SetBool("IsRunning", true);
                animator.SetBool("IsWalking", false);
                animator.SetBool("IsJumping", false);  // Pastikan animasi lompat dihentikan saat berlari
                PlayFootstepSound(runSound);
            }
            else
            {
                // Kondisi jika karakter sedang berjalan
                animator.SetBool("IsRunning", false);
                animator.SetBool("IsWalking", true);
                animator.SetBool("IsJumping", false);  // Pastikan animasi lompat dihentikan saat berjalan
                PlayFootstepSound(walkSound);
            }
        }
    }

    void Update()
    {
        // Input untuk lompat
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.W)) && jumpCount < maxJumps)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);  // Pastikan untuk menggunakan rb.velocity dan bukan rb.linearVelocity
            jumpCount++;
            animator.SetTrigger("Jump");  // Mengaktifkan animasi lompat
            PlayJumpSound();
        }

        moveVelocity = 0;
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            moveVelocity = -speed;
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            moveVelocity = speed;
        }
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            moveVelocity *= sprintMultiplier;
        }

        rb.linearVelocity = new Vector2(moveVelocity, rb.linearVelocity.y);  // Pastikan menggunakan rb.velocity
        HandleAnimationsAndSound();
        FlipCharacter(moveVelocity);

        // Mengaktifkan rune jika tombol Return ditekan
        if (Input.GetKeyDown(KeyCode.Return) && currentRune != null)
        {
            currentRune.ActivateRune();
        }
    }


    void PlayFootstepSound(AudioClip clip)
    {
        if (!audioSource.isPlaying || audioSource.clip != clip)
        {
            audioSource.clip = clip;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    void StopFootstepSound()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    void PlayJumpSound()
    {
        if (jumpSound != null)
        {
            audioSource.PlayOneShot(jumpSound);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            jumpCount = 0;
            animator.SetBool("IsJumping", false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Runes"))
        {
            if (popupObject != null)
            {
                popupObject.SetActive(true);
            }
            currentRune = other.GetComponent<runes>();
        }

        CameraTrigger cameraTrigger = other.GetComponent<CameraTrigger>();
        if (cameraTrigger != null)
        {
            cameraTrigger.TriggerCameraChange();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Runes"))
        {
            currentRune = null;
            if (popupObject != null)
            {
                popupObject.SetActive(false);
            }
        }
    }

    void FlipCharacter(float moveDirection)
    {
        if ((moveDirection > 0 && !facingRight) || (moveDirection < 0 && facingRight))
        {
            facingRight = !facingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }
}
