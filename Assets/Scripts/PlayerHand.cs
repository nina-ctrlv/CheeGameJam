using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerHand : MonoBehaviour
{

    public List<Card> hand;
    public GameObject cardPrefab;

    public float cardGap;

    public Tilemap tilemap;

    // Start is called before the first frame update
    void Start()
    {
        var offset = new Vector3(-((hand.Count-1) * (cardPrefab.transform.localScale.x + cardGap))/2, 0, 0);
        hand.ForEach(cardInfo =>
        {
            GameObject newCard = Instantiate(cardPrefab, transform.position + offset, transform.rotation, transform);
            newCard.GetComponent<CardDisplay>().card = cardInfo;
            newCard.GetComponent<SpriteController>().tilemap = tilemap;
            offset.x += cardPrefab.transform.localScale.x + cardGap;
        });
    }
}
