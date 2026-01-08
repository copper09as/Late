// 文件: CardData.cs
// 说明: 定义卡牌数据结构（用于卡牌内容和属性）。

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class CardData : ScriptableObject
{
    public int cardId;
    [SerializeField]
    private string cardName;
    [SerializeField]
    private string cardDescription;
    public Sprite cardImage;           
    public Sprite flipCardImage;
    [SerializeField]
    private List<int> functionArea;

    private void OnEnable()
    {
        if (functionArea == null)
            functionArea = new List<int>();
    }

    public IReadOnlyList<int> FunctionArea => functionArea;
    public virtual void UseCard(RuntimeGameData runtimeGameData)
    {
        EventBus.Publish(new Game.Event.TurnOver { });
        runtimeGameData.SaveCurrentGameData();
        EventBus.Publish(new Game.Event.CheckWin { });
    }
}