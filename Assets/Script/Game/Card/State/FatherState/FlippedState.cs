class FlippedState : CardState
{
    public override CardStates StateType => CardStates.flipped;

    public override void Enter()
    {
        //cardPresentation.FlipToBack();
    }

    public override void Execute()
    {
        // 在翻转状态下，等待其他逻辑处理
    }

    public override void Exit()
    {
        //cardPresentation.FlipToFront();
    }

}