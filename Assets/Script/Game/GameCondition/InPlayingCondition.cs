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