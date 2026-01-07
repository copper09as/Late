using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New OutDoor Card", menuName = "Card/OutDoorCard")]
class CardDataOutDoor : CardData
{
    public override void UseCard(RuntimeGameData runtimeGameData)
    {
        if(runtimeGameData.left>=3)
        {
            return;
        }
        var dic = new System.Collections.Generic.Dictionary<string, Action>();
        runtimeGameData.gameState = GameState.Paused;
        Action flipAll = () =>
        {
            runtimeGameData.gameState = GameState.Playing;
            runtimeGameData.left++;
            foreach (var card in runtimeGameData.cards)
            {
                if (card.StateType == CardStates.flipped)
                {
                    card.Flip();
                }
            }
            EventBus.Publish(new Game.Event.CheckWin());
        };
        Action continueAction = () =>
        {
            runtimeGameData.gameState = GameState.Playing;
        };
        var left = runtimeGameData.left;
        dic.Add("Flip All Cards Back (" + (3-left) + " left)", flipAll);
        dic.Add("Cancel", continueAction);
        MessageBox.Instance.ShowMessage(dic, "Do you want to flip all cards back?");

    }
}