// 文件: GameCondition.cs
// 说明: 抽象的游戏条件基类，针对不同操作定义是否满足条件并可注册回调。
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
