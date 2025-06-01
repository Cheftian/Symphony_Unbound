using UnityEngine;

public interface IMove
{
    void Move(Rigidbody2D rb, float speed, float sprintMultiplier);
}

public class MoveHandler : MonoBehaviour, IMove
{
    public void Move(Rigidbody2D rb, float speed, float sprintMultiplier)
    {
        float moveVelocity = 0;

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

        rb.linearVelocity = new Vector2(moveVelocity, rb.linearVelocity.y);
    }
}
