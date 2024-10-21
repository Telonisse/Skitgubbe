using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapCounter : MonoBehaviour
{
    public List<GameObject> gameObjectsToActivate; // Assign 23 GameObjects in the inspector
    private int cardCount = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Card"))
        {
            cardCount++;

            // Ensure cardCount does not exceed the number of GameObjects
            cardCount = Mathf.Clamp(cardCount, 0, gameObjectsToActivate.Count);

            // Activate the GameObject corresponding to the current card count
            if (cardCount > 0 && cardCount <= gameObjectsToActivate.Count)
            {
                gameObjectsToActivate[cardCount - 1].SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Card"))
        {
            cardCount--;

            // Ensure cardCount does not go below 0
            cardCount = Mathf.Clamp(cardCount, 0, gameObjectsToActivate.Count);

            // Deactivate the GameObject corresponding to the current card count
            if (cardCount >= 0 && cardCount < gameObjectsToActivate.Count)
            {
                gameObjectsToActivate[cardCount].SetActive(false);
            }
        }
    }
}
