using Meta.WitAi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FindSpawnPositions;

public class ThrowCardHandler : MonoBehaviour
{
    [SerializeField] List<GameObject> cards;
    [SerializeField] GameObject killPile;
    public bool first = false;

    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, Vector3.up, out hit, 1f))
        {
            if (hit.collider.tag == "Respawn" && IsCardInPile(hit.collider.gameObject) == false)
            {
                if (first == false)
                {
                    cards.Add(hit.collider.gameObject);
                    first = true;
                }
                else if (hit.collider.GetComponent<Card>().GetCardNum() >= cards[cards.Count - 1].GetComponent<Card>().GetCardNum() || hit.collider.GetComponent<Card>().GetCardNum() == 2 || hit.collider.GetComponent<Card>().GetCardNum() == 10)
                {
                    cards.Add(hit.collider.gameObject);
                }
            }
        }
        ShowLastCard();
        CheckLast4();
        CheckFor10();
    }

    bool IsCardInPile(GameObject checkCard)
    {
        foreach (var card in cards)
        {
            if (card == checkCard)
            {
                return true;
            }
        }
        return false;
    }

    void ShowLastCard()
    {
        if (cards.Count > 0)
        {
            for (int i = 0; i < cards.Count; i++)
            {
                cards[i].gameObject.SetActive(false);
            }
            cards[cards.Count - 1].SetActive(true);
        }
    }

    void CheckLast4()
    {
        if (cards.Count > 3)
        {
            int a = cards[cards.Count - 1].GetComponent<Card>().GetCardNum();
            int b = cards[cards.Count - 2].GetComponent<Card>().GetCardNum();
            int c = cards[cards.Count - 3].GetComponent<Card>().GetCardNum();
            int d = cards[cards.Count - 4].GetComponent<Card>().GetCardNum();
            if (a == b && b == c && c == d)
            {
                KillCards();
            }
        }
    }

    void CheckFor10()
    {
        if (cards.Count > 0)
        {
            if (cards[cards.Count - 1].GetComponent<Card>().GetCardNum() == 10)
            {
                KillCards();
            }
        }
    }

    void KillCards()
    {
        for(int i = 0;i < cards.Count; i++)
        {
            cards[i].GetComponent<Card>().KillCard();
            cards[i].transform.position = killPile.transform.position;
            cards[i].SetActive(true);
        }
        cards.Clear();
        first = false;
    }
}
