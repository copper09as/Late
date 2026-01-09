// 文件: FlippedState.cs
// 说明: 父状态，表示卡牌被翻转时的表现（展示背面图）。
class FlippedState : CardState
{
    public override CardStates StateType => CardStates.flipped;

    public override void Enter()
    {
        cardPresentation.SetImage(true);
        cardPresentation.StartCoroutine(cardPresentation.FlipCoroutine());
    }

    public override void Execute()
    {
    }

    public override void Exit()
    {
        cardPresentation.SetImage(false);
        cardPresentation.StartCoroutine(cardPresentation.FlipCoroutine());
    }

}