using TarodevController;
using UnityEngine;
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
    public Vector2 LastVel;
    private float CubeVel;
    private float _exitCooldown = 0.2f;
    private float _exitTime;
    public CubeVelocityTracker cubeTracker;
    private Rigidbody2D cubeRb;
    public Scales_gravity OtherPlatScript;
    public PlayerController ridingPlayer;
    //apex jump stuff
    private bool _hasLaunched = false;
    public float jumpBoostMultiplier = 1.5f;
    public float apexZone = 0.5f;
    public bool nearLimit = false;
    public float _lastMovingUpTime;
    private bool tooLow = false; // moved to class field

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
            nearLimit = distanceMoved <= -(maxDistance - apexZone);
            tooLow = distanceMoved <= -maxDistance; // updates class field

            if (!tooLow)
            {
                // platform is actively moving down, other platform moving up
                _hasLaunched = false;
                float currentSpeed = CubeVel > speed ? CubeVel : speed;
                Vector2 otherNewPos = OtherPlat.position + new Vector2(0, currentSpeed * Time.deltaTime);
                OtherPlatScript.LastVel = (otherNewPos - OtherPlat.position) / Time.deltaTime;
                OtherPlatScript._lastMovingUpTime = Time.time;
                if (cubeRb != null)
                    cubeRb.linearVelocity = new Vector2(cubeRb.linearVelocity.x, -currentSpeed);
                Vector2 newPos = CurrentPlat.position + new Vector2(0, -currentSpeed * Time.deltaTime);
                LastVel = (newPos - CurrentPlat.position) / Time.deltaTime;
                CurrentPlat.MovePosition(newPos);
                OtherPlat.MovePosition(otherNewPos);
            }
            else if (tooLow && !_hasLaunched && OtherPlatScript.ridingPlayer != null)
            {
                // platform just hit its limit, launch the player on the other platform
                OtherPlatScript._lastMovingUpTime = 0f;
                Vector2 launchVel = OtherPlatScript.LastVel;
                bool recentlyMovedUp = Time.time < OtherPlatScript._lastMovingUpTime + 0.1f;
                if (OtherPlatScript.ridingPlayer.JumpPressed && recentlyMovedUp)
                {
                    launchVel *= jumpBoostMultiplier;
                }
                OtherPlatScript.ridingPlayer.AddVelocity(launchVel);
                OtherPlatScript.ridingPlayer = null;
                _hasLaunched = true;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Cube"))
        {
            onPlatform = true;
            _exitTime = Time.time + _exitCooldown;
            CubeVel = cubeTracker != null ? cubeTracker.lastVelocity : 0f;
            cubeRb = col.rigidbody;
        }
        if (col.gameObject.CompareTag("Player"))
        {
            ridingPlayer = col.gameObject.GetComponent<PlayerController>();
        }
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
        if (col.gameObject.CompareTag("Cube") && Time.time > _exitTime)
        {
            onPlatform = false;
            nearLimit = false;
        }
        if (col.gameObject.CompareTag("Player"))
        {
            // only transfer velocity if platform is actively moving upward
            if (ridingPlayer != null && LastVel.y > 0 && onPlatform && !tooLow)
            {
                ridingPlayer.AddVelocity(LastVel);
            }
            ridingPlayer = null;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Weight"))
        {
            onPlatform = false;
            nearLimit = false;
        }
    }
}