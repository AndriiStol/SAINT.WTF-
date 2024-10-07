using System.Collections;
using UnityEngine;
using TMPro;

public class FirstWarehouse : MonoBehaviour
{
    public GameObject resourcePrefab; // Prefab of the resource object to spawn
    public Transform spawnPoint; // Point where objects will spawn
    public float timeInterval = 180f; // Time interval in seconds (3 minutes)
    public float heightOffset = 1f; // Height offset between objects
    public int maxResourceCount = 3; // Maximum number of objects

    public TextMeshProUGUI spawnStatusText; // TMP text for displaying messages

    private void Start()
    {
        // Start the coroutine to spawn resources over time
        StartCoroutine(SpawnResourcesOverTime());
    }

    IEnumerator SpawnResourcesOverTime()
    {
        while (true)
        {
            // Check the number of objects at the spawn point
            int currentResourceCount = spawnPoint.childCount;

            // If there are fewer objects than maxResourceCount, spawn the missing ones one by one
            if (currentResourceCount < maxResourceCount)
            {
                spawnStatusText.gameObject.SetActive(false); // Hide the spawn status message

                // Create a new object
                GameObject newResource = Instantiate(resourcePrefab, spawnPoint.position, Quaternion.identity);

                // Make it a child of the spawn point
                newResource.transform.SetParent(spawnPoint);

                // Position the object higher depending on the number of already spawned resources
                newResource.transform.position += new Vector3(currentResourceCount * heightOffset, 0, 0);
            }
            else
            {
                // Display the message that the spawn is stopped
                spawnStatusText.gameObject.SetActive(true);
            }

            // Wait for the specified time (3 minutes) before the next spawn
            yield return new WaitForSeconds(timeInterval);
        }
    }
}
