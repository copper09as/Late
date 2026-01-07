// 文件: CardRepository.cs
// 说明: 管理所有卡牌数据的仓库（列表），用于在模式中分配卡牌。
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardRepository", menuName = "CardRepository", order = 1)]
public class CardRepository : ScriptableObject
{
    public List<CardData> allCards;
}