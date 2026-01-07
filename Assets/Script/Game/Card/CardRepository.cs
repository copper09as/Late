using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardRepository", menuName = "CardRepository", order = 1)]
public class CardRepository : ScriptableObject
{
    public List<CardData> allCards;
}