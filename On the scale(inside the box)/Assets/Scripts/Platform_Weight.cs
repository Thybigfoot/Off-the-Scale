using UnityEngine;

public class Scales_Weight : MonoBehaviour
{
    [SerializeField] private float saltWeight;
    [SerializeField] private float cubeWeight;
    [SerializeField] private float maxSaltWeight = 10f;
    public float TotalWeight => saltWeight + cubeWeight;

    public void AddSalt(float amount)
    {
        saltWeight = Mathf.Min(saltWeight + amount, maxSaltWeight);
    }

    public void SetCubeWeight(float weight)
    {
        cubeWeight = weight;
    }
}
