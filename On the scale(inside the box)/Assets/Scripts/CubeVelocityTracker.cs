using UnityEngine;

public class CubeVelocityTracker : MonoBehaviour
{
    private Rigidbody2D rb;
    public float lastVelocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        lastVelocity = Mathf.Abs(rb.linearVelocity.y);
    }
}
