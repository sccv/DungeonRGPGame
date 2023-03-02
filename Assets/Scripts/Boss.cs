using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    public float fireballSpeed = 2.5f;
    public float distance = 0.25f;
    public Transform fireball;

    private void Update()
    {
        fireball.position = transform.position + new Vector3(-Mathf.Cos(Time.time * fireballSpeed) * distance, Mathf.Sin(Time.time * fireballSpeed) * distance, 0);
    }

    protected override void UpdateMotor(Vector3 input)
    {
        // Reset move delta
        moveDelta = new Vector3(input.x * xSpeed, input.y * ySpeed, 0);

        // Swap the sprite direction, whether you're going to left or right
        if (moveDelta.x > 0)
        {
            transform.localScale = new Vector3(1.8f, 1.8f, 1);
        }
        else if (moveDelta.x < 0)
        {
            transform.localScale = new Vector3(-1.8f, 1.8f, 1);
        }

        // Add push vertor, if any
        moveDelta += pushDirection;

        // Reduce push force every frame, based off recovery speed
        pushDirection = Vector3.Lerp(pushDirection, Vector3.zero, pushRecoverSpeed);


        // Make sure we can move in this direction, by casting a box there first, if the box return null, we're free to move
        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(0, moveDelta.y), Mathf.Abs(moveDelta.y * Time.deltaTime), LayerMask.GetMask("Actors", "Blocking"));


        if (hit.collider == null)
        {
            // Make this thing move
            transform.Translate(0, moveDelta.y * Time.deltaTime, 0);
        }

        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(moveDelta.x, 0), Mathf.Abs(moveDelta.x * Time.deltaTime), LayerMask.GetMask("Actors", "Blocking"));
        if (hit.collider == null)
        {
            // Make this thing move
            transform.Translate(moveDelta.x * Time.deltaTime, 0, 0);
        }
    }
}
