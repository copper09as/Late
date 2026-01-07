class UseCardCondition : GameCondition
{
    public override bool ConditionallyTrue(RuntimeGameData runtimeGameData)
    {
        var result = runtimeGameData.currentChoseCard == null;
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