// 文件: DarkMode.cs
// 说明: 特殊模式示例（暗模式），在 CardMode 基础上实现不同的初始化或规则。

using System;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "DarkMode", menuName = "DarkMode")]
public class DarkMode:CardMode
{
    private CardPresentation firstChoseCard;
    public override void Init(RuntimeGameData runtimeGameData)
    {
        base.InitSlots();
        firstChoseCard = null;
        NotInPlayingCondition notInPlayingCondition = new NotInPlayingCondition();
        ChoseSameCardCondition choseSameCardCondition = new ChoseSameCardCondition();
        UseAdjCardCondition useAdjCardCondition = new  UseAdjCardCondition();
        UseCardCondition useCardCondition = new UseCardCondition();
        UseCardEqualLastCardCondition useCardEqualLastCardCondition = new UseCardEqualLastCardCondition();
        UseCardIfFlipCondition useCardIfFlipCondition = new UseCardIfFlipCondition(); 

        useCardEqualLastCardCondition.Init(new List<Action<RuntimeGameData>> {});
        useCardCondition.Init(new List<Action<RuntimeGameData>> { UseCardCallback });
        useAdjCardCondition.Init(new List<Action<RuntimeGameData>> { UseAdjCardCallback });
        useCardIfFlipCondition.Init(new List<Action<RuntimeGameData>> {UseCardIfFlipCallback});
        notInPlayingCondition.Init(new List<Action<RuntimeGameData>> { });
        choseSameCardCondition.Init(new List<Action<RuntimeGameData>> { CancelSelection});

        gameConditions = new List<GameCondition>
         { 
            notInPlayingCondition,
            useCardIfFlipCondition,
            choseSameCardCondition,
            useCardEqualLastCardCondition,
            useCardCondition, 
            useAdjCardCondition
        };
    }
    protected void UseCardIfFlipCallback(RuntimeGameData runtimeGameData)
    {
        if(runtimeGameData.Time == 0 && firstChoseCard == null)
        {
            firstChoseCard = runtimeGameData.cardBeClicked;
            runtimeGameData.cardBeClicked.Flip();
            base.UseCardCallback(runtimeGameData);
        }
    }
    public override void ShuffleCards(RuntimeGameData runtimeGameData)
    {
        var cards = runtimeGameData.cards;
        base.ShuffleCards(runtimeGameData);
        foreach (var card in cards)
        {
            card.Flip();
        }
    }
}