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