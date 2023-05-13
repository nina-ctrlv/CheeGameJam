using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardDisplay : MonoBehaviour
{

    public Card card;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI cookLevelText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI descriptionText;

    public Image cardArtworkImage;

    void Start()
    {
        nameText.text = card.name;
        cookLevelText.text = "cook\n" + card.cookLevel.ToString();
        speedText.text = "speed\n" + card.speed.ToString();
        descriptionText.text = card.description;

        cardArtworkImage.sprite = card.artwork;
    }
}
