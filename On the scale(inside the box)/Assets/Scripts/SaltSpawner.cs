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
                Destroy(activeSalt, 0.5f); // slight delay for visual polish //whurturvur yur saur claurde
            }
        }
    }
}
