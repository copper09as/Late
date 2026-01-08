// 文件: CardDataOutDoor.cs
// 说明: 卡牌数据的一个变体或示例
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New OutDoor Card", menuName = "Card/OutDoorCard")]
class CardDataOutDoor : CardData
{
    public override void UseCard(RuntimeGameData runtimeGameData)
    {

        var dic = new System.Collections.Generic.Dictionary<string, Action>();
        int left = runtimeGameData.left;
        runtimeGameData.gameState = GameState.Paused;
        if (runtimeGameData.left < 3)
        {
            Action flipAll = () =>
            {


                runtimeGameData.left++;
                foreach (var card in runtimeGameData.cards)
                {
                    if (card.StateType == CardStates.flipped)
                    {
                        card.Flip();
                    }
                }
                runtimeGameData.gameState = GameState.Playing;
                EventBus.Publish(new Game.Event.TurnOver { });
                runtimeGameData.SaveCurrentGameData();
                EventBus.Publish(new Game.Event.CheckWin());


            };
            dic.Add("Flip All Cards Back (" + (3 - left) + " left)", flipAll);
        }

        Action continueAction = () =>
        {
            runtimeGameData.gameState = GameState.Playing;
            EventBus.Publish(new Game.Event.TurnOver { });
            runtimeGameData.SaveCurrentGameData();
            EventBus.Publish(new Game.Event.CheckWin());
        };

        dic.Add("Cancel", continueAction);
        MessageBox.Instance.ShowMessage(dic, "Do you want to flip all cards back?");

    }
}