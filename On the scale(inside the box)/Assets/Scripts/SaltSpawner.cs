using UnityEngine;
public class SaltSpawner : MonoBehaviour
{
    public GameObject saltPrefab;
    public Camera mainCamera;
    private GameObject activeSalt;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorld = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mouseWorld.z = 0f;
            activeSalt = Instantiate(saltPrefab, mouseWorld, Quaternion.identity);
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (activeSalt != null)
            {
                // Stop the salt script and collider immediately
                activeSalt.GetComponent<Salting>().enabled = false;
                activeSalt.GetComponent<BoxCollider2D>().enabled = false;

                // Stop particles emitting but let existing ones fade
                ParticleSystem particles = activeSalt.GetComponentInChildren<ParticleSystem>();
                if (particles != null) particles.Stop();

                // Destroy the whole object after particles have faded
                Destroy(activeSalt, 3f);
            }
        }
    }
}