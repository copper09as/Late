// 文件: InPlayingCondition.cs
// 说明: 条件：当前不在游戏中（用于禁止某些操作）。
using System;
using System.Collections.Generic;

public class NotInPlayingCondition:GameCondition
{
    int a = 1;
    public override bool ConditionallyTrue(RuntimeGameData runtimeGameData)
    {
        var result = runtimeGameData.gameState != GameState.Playing;
        if(result)
        {
            foreach(var i in onConditionTrueCallbacks)
            {
                i.Invoke(runtimeGameData);
            }
        }
        return result;
    }

}