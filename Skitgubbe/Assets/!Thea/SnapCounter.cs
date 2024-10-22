using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapCounter : MonoBehaviour
{
    public List<GameObject> gameObjectsToActivate; // Assign GameObjects in the inspector (at least 23)
    public int cardCount = 0;
    public float delayBeforeTurnOff = 0.5f; // Delay in seconds before deactivating snap points

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Card"))
        {
            cardCount++;

            // Ensure cardCount does not exceed the maximum allowed (up to 21 cards + 2 extra GameObjects)
            cardCount = Mathf.Clamp(cardCount, 0, gameObjectsToActivate.Count - 1);

            // Update GameObjects immediately
            UpdateGameObjects();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Card"))
        {
            cardCount--;

            // Ensure cardCount does not go below 0
            cardCount = Mathf.Clamp(cardCount, 0, gameObjectsToActivate.Count - 1);

            // Delay turning off GameObjects after a card exits
            StartCoroutine(DelayedUpdateGameObjects());
        }
    }

    private IEnumerator DelayedUpdateGameObjects()
    {
        // Wait for the specified delay before updating GameObjects
        yield return new WaitForSeconds(delayBeforeTurnOff);
        UpdateGameObjects();
    }

    private void UpdateGameObjects()
    {
        // Activate GameObjects based on the card count + 2
        for (int i = 0; i < gameObjectsToActivate.Count; i++)
        {
            if (i < cardCount + 2)
            {
                gameObjectsToActivate[i].SetActive(true);  // Turn on extra GameObjects (cardCount + 2)
            }
            else
            {
                gameObjectsToActivate[i].SetActive(false); // Turn off the rest
            }
        }
    }
}
