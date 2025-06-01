using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float sprintMultiplier = 2f;
    public float jumpForce = 10f;
    public int maxJumps = 2;
    
    private Rigidbody2D rb;
    private bool facingRight = true;
    private IJump jumpHandler;
    private IMove moveHandler;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpHandler = GetComponent<IJump>();
        moveHandler = GetComponent<IMove>();
    }

    void Update()
    {
        if (moveHandler != null)
        {
            moveHandler.Move(rb, speed, sprintMultiplier);
            FlipCharacter(rb.linearVelocity.x);
        }

        if (jumpHandler != null)
        {
            jumpHandler.Jump(rb, jumpForce);
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
