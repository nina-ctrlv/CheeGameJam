using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{

    public new string name;
    public string description;
    public Sprite artwork;

    // Tower info
    public float spawnFrequency;
    public float spawnRange;

    // Spawned info
    public float cookLevel;
    public float spawnedItemSpeed;
    public GameObject itemToSpawn;
    public Sprite draggingSprite;
}
