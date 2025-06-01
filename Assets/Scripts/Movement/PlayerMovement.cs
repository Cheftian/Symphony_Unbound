using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;
    private float moveVelocity;
    private bool facingRight = true; // Menyimpan arah karakter menghadap

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveVelocity = 0;

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            moveVelocity = -speed;
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            moveVelocity = speed;
        }

        rb.linearVelocity = new Vector2(moveVelocity, rb.linearVelocity.y);

        // Panggil fungsi FlipCharacter() saat bergerak
        FlipCharacter(moveVelocity);
    }

    void FlipCharacter(float moveDirection)
    {
        if ((moveDirection > 0 && !facingRight) || (moveDirection < 0 && facingRight))
        {
            facingRight = !facingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1; // Balik karakter secara horizontal
            transform.localScale = scale;
        }
    }
}
