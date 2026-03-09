using UnityEngine;

public class Door : MonoBehaviour
{
    public Key key;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && key.hasKey)
        {
            //change scene
        }
    }
}
