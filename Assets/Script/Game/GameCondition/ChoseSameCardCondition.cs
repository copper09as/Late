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