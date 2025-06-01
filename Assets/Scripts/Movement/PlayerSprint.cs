using UnityEngine;

public class PlayerSprint : MonoBehaviour
{
    public float sprintMultiplier = 2f;
    private PlayerMovement playerMovement;
    private Rigidbody2D rb;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && rb.linearVelocity.x != 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x * sprintMultiplier, rb.linearVelocity.y);
        }
    }
}
