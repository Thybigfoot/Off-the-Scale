using TarodevController;
using UnityEngine;

public class Scales_Movement : MonoBehaviour
{
    public Scales_Weight scalesWeightA;
    public Scales_Weight scalesWeightB;

    public Rigidbody2D cube;
    public Rigidbody2D PlatA;
    public Rigidbody2D PlatB;

    private Vector2 startPosition;
    private Vector2 startPositionB;
    [SerializeField] private float Speed = 1.0f;
    [SerializeField] public float MaxHeight = 5f;
    [SerializeField] private float forceMultiplier = 0.1f;
    private float weightDifference;


    private Vector2 lastVelA;
    private Vector2 lastVelB;
    private bool _hasLaunched = false;
    [SerializeField] private float apexBoostAmount = 1.0f;
    public PlayerController Player;
    [SerializeField] private float launchMultiplier = 0.1f;
    public float DistanceMoved { get; private set; }

    public Platform_PlayerDetector detectorA;
    public Platform_PlayerDetector detectorB;

    public void Start()
    {
        startPosition = PlatA.position;
        startPositionB = PlatB.position;
    }
    private void FixedUpdate()
    {
        float forceA = scalesWeightA.currentForce * forceMultiplier;
        float forceB = scalesWeightB.currentForce * forceMultiplier;

        DistanceMoved = PlatA.position.y - startPosition.y;
        bool tooLow = DistanceMoved <= -MaxHeight;
        bool tooHigh = DistanceMoved >= MaxHeight;
        weightDifference = scalesWeightA.TotalWeight - scalesWeightB.TotalWeight;
        bool canMove = (weightDifference > 0 && !tooLow) || (weightDifference < 0 && !tooHigh);
        if (canMove)
        {
            _hasLaunched = false;
            Vector2 newPosA = PlatA.position + new Vector2(0, -weightDifference * Speed * Time.deltaTime - forceA + forceB);
            Vector2 newPosB = PlatB.position + new Vector2(0, weightDifference * Speed * Time.deltaTime + forceA - forceB);
            newPosA.y = Mathf.Clamp(newPosA.y, startPosition.y - MaxHeight, startPosition.y + MaxHeight);
            newPosB.y = Mathf.Clamp(newPosB.y, startPositionB.y - MaxHeight, startPositionB.y + MaxHeight);
            PlatA.MovePosition(newPosA);
            PlatB.MovePosition(newPosB);
            lastVelA = (newPosA - PlatA.position) / Time.deltaTime;
            lastVelB = (newPosB - PlatB.position) / Time.deltaTime;

            if (scalesWeightA.HasCube)
                cube.linearVelocity = new Vector2(cube.linearVelocity.x, (newPosA.y - PlatA.position.y) / Time.fixedDeltaTime);
            else if (scalesWeightB.HasCube)
                cube.linearVelocity = new Vector2(cube.linearVelocity.x, (newPosB.y - PlatB.position.y) / Time.fixedDeltaTime);
        }

        float proximityToLimit = Mathf.Abs(DistanceMoved) / MaxHeight;
        if (!canMove)
        {

            //inertia
            if (lastVelA.y > 0 && !_hasLaunched && detectorA.playerOnPlatform)
            {
                Vector2 launch = lastVelA * launchMultiplier;
                if (Player.JumpPressed) launch += new Vector2(0, apexBoostAmount * proximityToLimit);
                Player.AddVelocity(launch);
                _hasLaunched = true;
            }
            else if (lastVelB.y > 0 && !_hasLaunched && detectorB.playerOnPlatform)
            {
                Vector2 launch = lastVelB * launchMultiplier;
                if (Player.JumpPressed) launch += new Vector2(0, apexBoostAmount * proximityToLimit);
                Player.AddVelocity(launch);
                _hasLaunched = true;
            }
        }
    }
   
}
