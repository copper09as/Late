// 文件: NormalState.cs (Father)
// 说明: 父状态，表示卡牌的普通（未被使用或翻转）状态。
public class NormalState : CardState
{
    public override CardStates StateType => CardStates.Normal;

    public override void Enter()
    {
    }

    public override void Execute()
    {
        // 在正常状态下，等待其他逻辑处理
    }

    public override void Exit()
    {
    }
}