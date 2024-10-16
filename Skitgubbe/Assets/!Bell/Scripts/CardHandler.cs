using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CardHandler : MonoBehaviour
{
    [SerializeField] GameObject spawnLocation;
    [SerializeField] List<GameObject> cardPrefab;

    public List<GameObject> shuffledCards;

    public int currentCard = 0;

    private void Start()
    {
        shuffledCards = ShuffleSymbols(cardPrefab);
        AssignSymbols();
    }

    private List<GameObject> ShuffleSymbols(List<GameObject> symbols)
    {
        List<GameObject> shuffled = new List<GameObject>(symbols);

        for (int i = 0; i < shuffled.Count; i++)
        {
            GameObject temp = shuffled[i];
            int randomIndex = Random.Range(0, symbols.Count);
            shuffled[i] = shuffled[randomIndex];
            shuffled[randomIndex] = temp;
        }

        return shuffled;
    }

    private void AssignSymbols()
    {
        Instantiate(shuffledCards[currentCard], new Vector3(spawnLocation.transform.position.x, spawnLocation.transform.position.y + 0.5f, spawnLocation.transform.position.z), shuffledCards[currentCard].transform.rotation);
        currentCard++;
    }

    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(spawnLocation.transform.position, Vector3.up, out hit, 1f))
        {
            if (hit.collider.tag != "Respawn" && currentCard < 52)
            {
                Instantiate(shuffledCards[currentCard], new Vector3(spawnLocation.transform.position.x, spawnLocation.transform.position.y + 0.5f, spawnLocation.transform.position.z), shuffledCards[currentCard].transform.rotation);
                currentCard++;
            }
            //else if(hit.collider.tag == "Respawn")
            //{
            //    int num = hit.collider.GetComponent<Card>().GetCardNum();
            //    Debug.Log(num);
            //}
        }

        Debug.DrawRay(spawnLocation.transform.position, Vector3.up * 1, Color.red);
    }
}
