// 文件: UseAdjCardCondition.cs
// 说明: 条件：被点击的卡片与当前选中卡片相邻（用于可交换逻辑）。
class UseAdjCardCondition : GameCondition
{
    public override bool ConditionallyTrue(RuntimeGameData runtimeGameData)
    {
        var result = runtimeGameData.currentChoseCard != null 
        && runtimeGameData.cardBeClicked.AdjCard( runtimeGameData.currentChoseCard.index);
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