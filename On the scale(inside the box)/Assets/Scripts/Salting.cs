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

    private void Start()
    {
        col = GetComponent<BoxCollider2D>();
        col.enabled = false; // disable until stream arrives
        origin = transform.position;
        //start at zero height
        col.size = new Vector2(col.size.x, 0f);
        col.offset = Vector2.zero;
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
                // Salt has visually arrived, collider activates exactly when stream reaches scale
                col.enabled = true;
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
            isTouch = true;
        }
    }
}
