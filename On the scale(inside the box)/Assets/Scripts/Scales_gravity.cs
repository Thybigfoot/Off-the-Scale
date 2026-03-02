using TarodevController;
using UnityEngine;
using UnityEngine.Rendering;

public class Scales_gravity : MonoBehaviour
{

    //platform stuff
    private Rigidbody2D CurrentPlat;
    public Rigidbody2D OtherPlat;
    private bool onPlatform = false;
    public float speed = 2f;
    private Vector2 startPosition;
    public float maxDistance = 3f;

    //player inertia stuff
    private Vector2 LastVel;        

    private void Start()
    {
        CurrentPlat = GetComponent<Rigidbody2D>();
        CurrentPlat.bodyType = RigidbodyType2D.Kinematic;
        OtherPlat.bodyType = RigidbodyType2D.Kinematic;
        startPosition = CurrentPlat.position;
    }


    private void FixedUpdate()
    {
        if (onPlatform)
        {
            float distanceMoved = CurrentPlat.position.y - startPosition.y;
            bool tooLow = distanceMoved <= -maxDistance;
            bool tooHigh = distanceMoved >= maxDistance;

            if (!tooLow)
            {
                Vector2 newPos = CurrentPlat.position + new Vector2(0, -speed * Time.deltaTime);
                LastVel = (newPos - CurrentPlat.position) / Time.deltaTime;
                CurrentPlat.MovePosition(newPos);
                OtherPlat.MovePosition(OtherPlat.position + new Vector2(0, speed * Time.deltaTime));
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Weight")) onPlatform = true;
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Weight")) onPlatform = true;
    }
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Weight")) onPlatform = true;
    }
    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Weight"))
        {
            onPlatform = false;
            PlayerController pc = col.gameObject.GetComponent<PlayerController>();
            if (pc != null && LastVel.y > 0) pc.AddVelocity(LastVel);
           
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Weight")) onPlatform = false;
    }
}
