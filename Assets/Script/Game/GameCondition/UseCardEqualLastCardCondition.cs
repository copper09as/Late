// 文件: UseCardEqualLastCardCondition.cs
// 说明: 条件：点击的卡片处于已使用状态（用于特定规则处理）。
class UseCardEqualLastCardCondition : GameCondition
{
    public override bool ConditionallyTrue(RuntimeGameData runtimeGameData)
    {
        var result = runtimeGameData.currentChoseCard == null 
        && (runtimeGameData.cardBeClicked.StateType == CardStates.AlreadyUsed 
        || runtimeGameData.cardBeClicked.StateType == CardStates.AlreadyUsedT);
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