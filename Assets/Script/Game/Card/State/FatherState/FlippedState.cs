// 文件: FlippedState.cs
// 说明: 父状态，表示卡牌被翻转时的表现（展示背面图）。
class FlippedState : CardState
{
    public override CardStates StateType => CardStates.flipped;

    public override void Enter()
    {
        cardPresentation.SetImage(true);
    }

    public override void Execute()
    {
        // 在翻转状态下，等待其他逻辑处理
    }

    public override void Exit()
    {
        cardPresentation.SetImage(false);
    }

}