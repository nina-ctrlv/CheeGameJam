using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerHand : MonoBehaviour
{

    public List<Card> hand;
    public List<Card> foodCards;
    public List<Card> gadgetCards;
    public GameObject cardPrefab;

    public float cardGap;

    public Tilemap tilemap;

    // Start is called before the first frame update
    void Start()
    {
        hand = new List<Card>();
        hand.Add(foodCards[Random.Range(0, foodCards.Count)]);
        hand.Add(foodCards[Random.Range(0, foodCards.Count)]);

        hand.Add(gadgetCards[Random.Range(0, gadgetCards.Count)]);
        hand.Add(gadgetCards[Random.Range(0, gadgetCards.Count)]);
        hand.Add(gadgetCards[Random.Range(0, gadgetCards.Count)]);
        var offset = new Vector3(-((hand.Count-1) * (cardPrefab.transform.localScale.x + cardGap))/2, 0, 0);
        hand.ForEach(cardInfo =>
        {
            GameObject newCard = Instantiate(cardPrefab, transform.position + offset, transform.rotation, transform);
            newCard.GetComponent<CardDisplay>().card = cardInfo;
            newCard.GetComponent<CardController>().tilemap = tilemap;
            newCard.GetComponent<CardController>().card = cardInfo;
            offset.x += cardPrefab.transform.localScale.x + cardGap;
            newCard.GetComponent<NetworkObject>().Spawn();
        });
    }
}
