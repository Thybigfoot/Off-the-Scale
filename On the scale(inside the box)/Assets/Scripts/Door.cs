using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public Key key;
    private int index;
    private int nextIndex;
    private Animator animator;
    private void Start()
    {
        index = SceneManager.GetActiveScene().buildIndex;
        nextIndex = index + 1;
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    { 
        animator.SetBool("isOpen", key.hasKey);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && key.hasKey)
        {
            if (nextIndex < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(nextIndex);
            }
            else
            {
                Debug.LogError($"Scene does not existc{nextIndex}");
            }
        }
    }
}
