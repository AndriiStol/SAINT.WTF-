using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Don't forget to add reference to TextMeshPro

public class SecondWarehouse : MonoBehaviour
{
    public GameObject resourceToSpawn; // Prefab of the new object to spawn
    public Transform spawnPoint; // Point where the new object will spawn
    public GameObject sklad1; // Warehouse where resources need to be checked
    public string requiredTag = "resurs1"; // Tag of the resource to check for
    public int requiredAmount = 3; // Amount of resources required for spawning
    public TextMeshProUGUI resourceText; 

    private int remainingResources; // Remaining resources for spawning

    void Start()
    {
        // Update the resource text when the game starts
        UpdateResourceText();
    }

    void Update()
    {
        // Check if there are enough resources with the required tag in the warehouse
        if (CheckForResourcesAndRemove(sklad1, requiredTag, requiredAmount))
        {
            // Spawn a new resource if enough resources are found
            SpawnNewResource();
        }
    }

    bool CheckForResourcesAndRemove(GameObject sklad, string tag, int requiredAmount)
    {
        int resourceCount = 0;
        List<GameObject> resourcesToRemove = new List<GameObject>();

        // Iterate through all child objects of the warehouse and look for objects with the required tag
        foreach (Transform child in sklad.transform)
        {
            if (child.CompareTag(tag))
            {
                resourceCount++;

                MeshRenderer meshRenderer = child.GetComponent<MeshRenderer>();
                if (meshRenderer != null)
                {
                    meshRenderer.enabled = false; // Disable the object's visibility
                }

                Collider collider = child.GetComponent<Collider>();
                if (collider != null)
                {
                    collider.enabled = false; // Disable the collider
                }
                resourcesToRemove.Add(child.gameObject); // Add to the list of objects to remove

                // If enough resources are found, exit the loop
                if (resourceCount == requiredAmount)
                {
                    break;
                }
            }
        }

        // If enough resources are found, remove them and return true
        if (resourceCount >= requiredAmount)
        {
            foreach (GameObject resource in resourcesToRemove)
            {
                Destroy(resource); // Remove the resource from the scene
            }
            remainingResources = 0; // Update the remaining resources count
            UpdateResourceText(); // Update the text display
            return true;
        }

        // Update the remaining resources count
        remainingResources = requiredAmount - resourceCount;
        UpdateResourceText();
        return false; // Not enough resources for spawning
    }

    void SpawnNewResource()
    {
        // Spawn a new object at the specified point
        Instantiate(resourceToSpawn, spawnPoint.position += new Vector3(1, 0, 0), Quaternion.identity);
        Debug.Log("A new object has been created!");
    }

    void UpdateResourceText()
    {
        // Update the TextMeshPro text with information on how many resources are still needed
        resourceText.text = "Production at the second machine stopped due to lack of resources. Resources needed for spawning: " + remainingResources.ToString();
    }
}
