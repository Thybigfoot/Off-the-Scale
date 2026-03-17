using UnityEngine;

public class Key : MonoBehaviour
{
    public bool hasKey;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            hasKey = true;
            Destroy(gameObject);
            //bimbimbambam
        }
    }

}
