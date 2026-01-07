// 文件: UseCardIfFlipCondition.cs
// 说明: 条件：被点击的卡片为翻转状态（用于特定触发逻辑）。
class UseCardIfFlipCondition : GameCondition
{
    public override bool ConditionallyTrue(RuntimeGameData runtimeGameData)
    {
        var result = runtimeGameData.currentChoseCard == null
         && runtimeGameData.cardBeClicked.StateType == CardStates.flipped;
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