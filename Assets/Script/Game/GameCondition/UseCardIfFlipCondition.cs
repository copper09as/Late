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