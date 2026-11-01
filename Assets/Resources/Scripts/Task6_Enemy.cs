using UnityEngine;

[RequireComponent(
    typeof(SpriteRenderer), 
    typeof(Rigidbody2D)
)]
public class Task6_Enemy : MonoBehaviour
{
    public float speed = 2.0f;
    public float minDistance = 2.0f;
    public Transform objective;
    private Vector2 movement;
    private SpriteRenderer renderer;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
        if (objective == null) {
            Debug.LogError("Enemy: no objective set!");
        }
    }

    void Update()
    {
        movement = objective.position - transform.position;
        if (movement.magnitude <= minDistance) {
            movement.Set(0, 0);
            return;
        }
        movement.Normalize();
        if (movement.x < 0.0f) {
            renderer.flipX = false;
        } else {
            renderer.flipX = true;
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = speed * movement;
    }
}
