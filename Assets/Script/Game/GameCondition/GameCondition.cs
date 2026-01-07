using System;
using System.Collections.Generic;
using UnityEngine.PlayerLoop;

public abstract class GameCondition
{
    protected List<Action<RuntimeGameData>> onConditionTrueCallbacks = new List<Action<RuntimeGameData>>();
    public virtual void Init(List<Action<RuntimeGameData>> OnConditionTrue)
    {
        onConditionTrueCallbacks = OnConditionTrue;
    }
    public abstract bool ConditionallyTrue(RuntimeGameData runtimeGameData);
}
