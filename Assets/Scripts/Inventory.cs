using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{
    public Transform inventoryHolder; // Object on the scene where resources will be moved
    public float itemHeightOffset = 2f; // Height between items in the stack
    public int maxInventorySize = 5; // Maximum number of items in the inventory
    public Button actionButton; // One button for adding resources and transferring to warehouse
    private List<GameObject> inventory = new List<GameObject>(); // List of items in the inventory
    private GameObject currentResource; // Current object being interacted with
    private GameObject currentSklad; // Current warehouse being interacted with
    private bool isButtonPressed = false; // Flag to track if button is pressed on the screen
    public TextMeshProUGUI inventar; // TextMeshPro object for inventory display
    public TextMeshProUGUI sklad; // TextMeshPro object for warehouse display

    void Start()
    {
        // Bind the function to the UI button
        actionButton.onClick.AddListener(() => OnActionButtonPressed());
    }

    void Update()
    {
        // Check if the current resource no longer exists
        if (currentResource == null)
        {
            inventar.gameObject.SetActive(false); // Hide the text
        }

        // Adding to inventory if a resource is present and the E key or the screen button is pressed
        if (currentResource != null && (Input.GetKeyDown(KeyCode.E) || isButtonPressed))
        {
            if (inventory.Count < maxInventorySize)
            {
                AddToInventory(currentResource);
            }
            else
            {
                Debug.Log("Oops, inventory is full!");
            }
        }

        // Transferring to warehouse
        if (currentSklad != null && (Input.GetKeyDown(KeyCode.E) || isButtonPressed))
        {
            TransferResourceToSklad(currentSklad);
        }

        // Reset the flag after button use
        isButtonPressed = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check for resource 1 or 2
        if (other.CompareTag("resurs1") || other.CompareTag("resurs2"))
        {
            currentResource = other.gameObject;
            inventar.gameObject.SetActive(true); // Show text for inventory interaction
        }
        // Check for warehouse 1 or 2
        else if (other.CompareTag("SKLAD1") || other.CompareTag("SKLAD2"))
        {
            currentSklad = other.gameObject;
            sklad.gameObject.SetActive(true); // Show text for warehouse interaction
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // If we leave the resource trigger
        if (other.CompareTag("resurs1") || other.CompareTag("resurs2"))
        {
            // Check if the resource is not destroyed and correctly update the reference
            if (currentResource == other.gameObject)
            {
                currentResource = null;
                inventar.gameObject.SetActive(false); // Disable the text
            }
        }
        // If we leave the warehouse trigger
        else if (other.CompareTag("SKLAD1") || other.CompareTag("SKLAD2"))
        {
            if (currentSklad == other.gameObject)
            {
                currentSklad = null;
                sklad.gameObject.SetActive(false); // Disable the warehouse text
            }
        }
    }

    private void AddToInventory(GameObject resource)
    {
        // Add resource to inventory
        inventory.Add(resource);
        Collider resourceCollider = resource.GetComponent<Collider>();
        if (resourceCollider != null)
        {
            resourceCollider.enabled = false; // Disable collider
        }

        // Move the resource to the designated inventoryHolder object
        resource.transform.SetParent(inventoryHolder);

        // Position it in the stack
        resource.transform.localPosition = new Vector3(0, inventory.Count * itemHeightOffset, 0);

        currentResource = null; // The object is collected, reset it
        inventar.gameObject.SetActive(false); // Disable the "Press E to pick up" text
    }

    private void TransferResourceToSklad(GameObject sklad)
    {
        if (inventory.Count > 0)
        {
            // Get the last resource from the inventory
            GameObject lastResource = inventory[inventory.Count - 1];
            string resourceTag = lastResource.tag;

            // Check if the resource is suitable for the current warehouse
            if ((resourceTag == "resurs1" && sklad.CompareTag("SKLAD1")) ||
                (resourceTag == "resurs2" && sklad.CompareTag("SKLAD2")) ||
                (resourceTag == "resurs1" && sklad.CompareTag("SKLAD2")))
            {
                // Transfer the resource to the warehouse
                lastResource.transform.SetParent(sklad.transform);

                // Position it in the warehouse considering the current number of objects in the warehouse
                int itemCountInSklad = sklad.transform.childCount;
                lastResource.transform.localPosition = new Vector3(0, itemCountInSklad * itemHeightOffset, 0);

                // Remove the resource from the inventory
                inventory.RemoveAt(inventory.Count - 1);

                Debug.Log("Resource transferred to " + sklad.name);
            }
            else
            {
                Debug.Log("This resource is not suitable for this warehouse!");
            }
        }
        else
        {
            Debug.Log("Inventory is empty!");
        }
    }

    public void OnActionButtonPressed()
    {
        isButtonPressed = true;
    }
}
