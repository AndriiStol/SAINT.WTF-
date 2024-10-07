using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ThirdWarehouse : MonoBehaviour
{
    public GameObject resourceToSpawn; // Prefab of the new object to be spawned
    public Transform spawnPoint; // Spawn point for the new object
    public GameObject sklad2; // Warehouse where resources need to be checked
    public string requiredTag1 = "resurs1"; // Tag for the first resource type
    public string requiredTag2 = "resurs2"; // Tag for the second resource type
    public int requiredAmount = 3; // Required amount of each resource type
    public TextMeshProUGUI resourceText; // TMP text for displaying resource information

    private int remainingResurs1; // Remaining resurs1 for spawning
    private int remainingResurs2; // Remaining resurs2 for spawning

    void Start()
    {
        // Initialize starting values
        remainingResurs1 = requiredAmount;
        remainingResurs2 = requiredAmount;
        UpdateResourceText(); // Update the text at the start
    }

    void Update()
    {
        // Check if there are enough resources and spawn the object
        if (CheckForResourcesAndRemove(sklad2, requiredTag1, requiredTag2, requiredAmount))
        {
            SpawnNewResource();
        }
    }

    bool CheckForResourcesAndRemove(GameObject sklad, string tag1, string tag2, int requiredAmount)
    {
        int resourceCount1 = 0;
        int resourceCount2 = 0;
        List<GameObject> resourcesToRemove = new List<GameObject>();

        // Loop through all child objects in the warehouse and check the resource tags
        foreach (Transform child in sklad.transform)
        {
            // Count objects with tag resurs1
            if (child.CompareTag(tag1))
            {
                resourceCount1++;
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

                resourcesToRemove.Add(child.gameObject); // Add to the list for removal

                if (resourceCount1 == requiredAmount)
                {
                    continue; // If enough resources are found, skip further checks for resurs1
                }
            }

            // Count objects with tag resurs2
            if (child.CompareTag(tag2))
            {
                resourceCount2++;
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

                resourcesToRemove.Add(child.gameObject); // Add to the list for removal

                if (resourceCount2 == requiredAmount)
                {
                    continue; // If enough resources are found, skip further checks for resurs2
                }
            }

            // If both resource types are gathered, exit the loop
            if (resourceCount1 >= requiredAmount && resourceCount2 >= requiredAmount)
            {
                break;
            }
        }

        // If enough of both resource types are found, remove them and update the text
        if (resourceCount1 >= requiredAmount && resourceCount2 >= requiredAmount)
        {
            foreach (GameObject resource in resourcesToRemove)
            {
                Destroy(resource); // Remove the resources from the scene
            }
            remainingResurs1 = 0; // Reset remaining resources
            remainingResurs2 = 0;
            UpdateResourceText(); // Update the text
            return true;
        }

        // Update remaining resource counts
        remainingResurs1 = requiredAmount - resourceCount1;
        remainingResurs2 = requiredAmount - resourceCount2;
        UpdateResourceText(); // Update the text

        return false; // Not enough resources for spawning
    }

    void SpawnNewResource()
    {
        // Spawn a new object at the specified point
        Instantiate(resourceToSpawn, spawnPoint.position += new Vector3(1, 0, 0), Quaternion.identity);
        Debug.Log("A new object has been created!");

        // After spawning, reset remaining resources
        remainingResurs1 = requiredAmount;
        remainingResurs2 = requiredAmount;
        UpdateResourceText(); // Reset the text
    }

    void UpdateResourceText()
    {
        // Update the TMP text with the remaining amounts of resurs1 and resurs2
        resourceText.text = "Production at the third machine has stopped. Missing: " +
                            remainingResurs1 + " green cubes and " + remainingResurs2 + " blue cubes for spawning.";
    }
}
