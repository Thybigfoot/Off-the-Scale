using UnityEngine;

public class Scales_Weight : MonoBehaviour
{
    [SerializeField] private float saltWeight;
    [SerializeField] private float cubeWeight;
    [SerializeField] private float maxSaltWeight = 10f;
    private float _exitTime;
    [SerializeField] private float _exitCooldown = 0.2f;
    private float targetCubeWeight = 0f;
    [SerializeField] private float easeSpeed = 0.5f;
    public float TotalWeight => saltWeight + cubeWeight;
    
    public float currentForce;
    [SerializeField] private float decaySpeed = 1f;
    public bool HasCube => targetCubeWeight > 0f;

    private void FixedUpdate()
    {
        cubeWeight = Mathf.MoveTowards(cubeWeight, targetCubeWeight, easeSpeed * Time.fixedDeltaTime);
        currentForce = Mathf.MoveTowards(currentForce, 0f, decaySpeed * Time.fixedDeltaTime);
    }
    public void AddSalt(float amount)
    {
        saltWeight = Mathf.Min(saltWeight + amount, maxSaltWeight);
    }

    public void SetCubeWeight(float weight)
    {
        targetCubeWeight = weight;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Cube"))
        {
            _exitTime = Time.time + _exitCooldown;
            CubeVelocityTracker tracker = col.gameObject.GetComponent<CubeVelocityTracker>();
            SetCubeWeight(col.rigidbody.mass);
            currentForce = tracker.lastVelocity * col.rigidbody.mass;

        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Cube"))
        {
            // if cube has significant horizontal velocity it was knocked off
            bool knockedOff = Mathf.Abs(col.rigidbody.linearVelocity.x) > 0.5f;

            if (Time.time > _exitTime || knockedOff)
            {
                SetCubeWeight(0);
            }
        }
    }

}
