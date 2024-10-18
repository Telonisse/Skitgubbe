using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class NetworkedCardHandler : NetworkBehaviour
{
    [SerializeField] List<GameObject> cardPrefabs;  // List of card prefabs to be shuffled
    [SerializeField] GameObject spawnLocation;      // Location where the card will be spawned

    // We store the shuffled card indices instead of GameObjects
    public List<int> shuffledCardIndices = new List<int>();

    // Networked integer to keep track of the card index we are spawning
    [Networked] public int currentCardIndex { get; set; }

    public override void Spawned()
    {
        // Only the server/host should shuffle the deck
        if (HasStateAuthority)  // Ensures only the server shuffles
        {
            shuffledCardIndices = ShuffleDeck(cardPrefabs.Count);  // Shuffle the card indices
            RPC_SpawnCard();  // Spawn the first card from the shuffled deck
        }
        else
        {
            Debug.Log("Has no authority");
        }
    }

    // Function to shuffle the card indices (we shuffle indices instead of GameObjects)
    private List<int> ShuffleDeck(int cardCount)
    {
        List<int> indices = new List<int>();

        // Populate the list with sequential numbers corresponding to the card prefabs
        for (int i = 0; i < cardCount; i++)
        {
            indices.Add(i);
        }

        // Shuffle the indices
        for (int i = 0; i < indices.Count; i++)
        {
            int randomIndex = Random.Range(0, cardCount);
            int temp = indices[i];
            indices[i] = indices[randomIndex];
            indices[randomIndex] = temp;
        }

        return indices;
    }
    public void OnButtonPressed()
    {
        RPC_SpawnCard();
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    private void RPC_SpawnCard()
    {
        if (shuffledCardIndices.Count > 0)
        {
            int firstCardIndex = shuffledCardIndices[currentCardIndex];  // Get the first card in the shuffled list
            GameObject cardPrefabToSpawn = cardPrefabs[firstCardIndex];

            // Spawn the card across the network using Runner.Spawn (handled by the server)
            NetworkObject spawnedObject = Runner.Spawn(cardPrefabToSpawn, spawnLocation.transform.position, Quaternion.identity);

            currentCardIndex++;  // Update the index to spawn the next card
        }
        else
        {
            Debug.LogError("No cards to spawn, shuffled deck is empty!");
        }
    }

    // Function to spawn the first card from the shuffled deck
    //public void SpawnCard()
    //{
    //    if (shuffledCardIndices.Count > 0)
    //    {
    //        int firstCardIndex = shuffledCardIndices[currentCardIndex];  // Get the first card in the shuffled list
    //        GameObject cardPrefabToSpawn = cardPrefabs[firstCardIndex];

    //        // Spawn the card across the network using Runner.Spawn
    //        NetworkObject spawnedObject = Runner.Spawn(cardPrefabToSpawn,spawnLocation.transform.position,Quaternion.identity);

    //        currentCardIndex++;  // Set the networked index to the first card
    //    }
    //    else
    //    {
    //        Debug.LogError("No cards to spawn, shuffled deck is empty!");
    //    }
    //}
}
