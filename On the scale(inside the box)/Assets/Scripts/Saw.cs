using UnityEngine;
using UnityEngine.SceneManagement;

public class Saw : MonoBehaviour
{
    [SerializeField]private float spinSpeed = 90f;
    private void Update()
    {
        transform.Rotate(0, 0, spinSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) Reseting();

    }

    public void Reseting()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
