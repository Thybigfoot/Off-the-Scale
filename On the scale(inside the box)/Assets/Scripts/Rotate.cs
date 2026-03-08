using UnityEngine;
public class Rotate : MonoBehaviour
{
    [SerializeField] private float maxAngle = 30f;
    private float angle;
    public Scales_Movement scale;
    private void FixedUpdate()
    {
        angle = -(scale.DistanceMoved / scale.MaxHeight) * maxAngle;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
