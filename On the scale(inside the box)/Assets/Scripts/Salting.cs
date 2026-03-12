using UnityEngine;
using UnityEngine.UIElements;
public class Salting : MonoBehaviour
{
    //settings
    public float expandSpeed = 5f;
    public float maxLength = 20f;
    public LayerMask scaleLayerMask;
    private BoxCollider2D col;
    private float currentLength = 0f;
    private bool isTouch = false;
    private Vector2 origin;
    private float targetDistance = 0f;
    private ParticleSystem saltParticles;
    public float saltAmountPerSecond = 1f;

    private void Start()
    {
        col = GetComponent<BoxCollider2D>();
        col.enabled = false; // disable until stream arrives
        origin = transform.position;
        //start at zero height
        col.size = new Vector2(col.size.x, 0f);
        col.offset = Vector2.zero;
        saltParticles = GetComponentInChildren<ParticleSystem>();
        saltParticles.Play(); // start immediately on spawn
    }

    void Update()
    {
        if (!isTouch)
        {
            currentLength += expandSpeed * Time.deltaTime;
            UpdateCollider();
            CheckForScale();
        }
        else
        {
            // Keep expanding until we visually reach the scale
            if (currentLength < targetDistance)
            {
                currentLength += expandSpeed * Time.deltaTime;
                currentLength = Mathf.Min(currentLength, targetDistance); // don't overshoot
                UpdateCollider();
            }
            else
            {
                col.enabled = true;
                // continuously add salt weight while stream is active
                if (targetScaleWeight != null)
                    targetScaleWeight.AddSalt(saltAmountPerSecond * Time.deltaTime);
                currentLength += expandSpeed * Time.deltaTime;
                UpdateCollider();
            }
        }
    }

    void UpdateCollider()
    {
        //grow downwards aka opposite of my ween type shii
        col.size = new Vector2(col.size.x, currentLength);
        col.offset = new Vector2(0f, -currentLength / 2f);
    }

    private Scales_Weight targetScaleWeight = null;

    void CheckForScale()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            origin,
            Vector2.down,
            currentLength,
            scaleLayerMask
        );
        if (hit.collider != null && !isTouch)
        {
            targetDistance = hit.distance;
            targetScaleWeight = hit.collider.GetComponent<Scales_Weight>();
            Debug.Log("Hit: " + hit.collider.gameObject.name + " | ScaleWeight found: " + (targetScaleWeight != null));
            isTouch = true;
        }
    }
}