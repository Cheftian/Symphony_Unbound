using UnityEngine;

public interface IJump
{
    void Jump(Rigidbody2D rb, float jumpForce);
}

public class JumpHandler : MonoBehaviour, IJump
{
    private bool isGrounded = false;
    private int jumpCount = 0;
    private int maxJumps = 1;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            jumpCount = 0;
        }
    }

    public void Jump(Rigidbody2D rb, float jumpForce)
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.W)) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
        }
    }
}
