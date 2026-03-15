using UnityEngine;
using UnityEngine.SceneManagement;
public class Reset : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) Reseting();
    }
    public void Reseting()
    {
        TutorialManager.SkipOnReload = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
