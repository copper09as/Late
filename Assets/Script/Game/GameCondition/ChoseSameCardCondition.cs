// 文件: ChoseSameCardCondition.cs
// 说明: 条件：选中同一张卡（用于取消选择等逻辑）。
using System;
using System.Collections.Generic;

class ChoseSameCardCondition : GameCondition
{
    public override bool ConditionallyTrue(RuntimeGameData runtimeGameData)
    {
        var result = runtimeGameData.cardBeClicked == runtimeGameData.currentChoseCard;
        if (result)
        {
            foreach (var i in onConditionTrueCallbacks)
            {
                i.Invoke(runtimeGameData);
            }
        }
        return result;
    }
}