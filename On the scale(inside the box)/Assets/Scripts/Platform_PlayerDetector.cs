using UnityEngine;
public class Platform_PlayerDetector : MonoBehaviour
{
    public bool playerOnPlatform = false;

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player")) playerOnPlatform = true;
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player")) playerOnPlatform = false;
    }
}