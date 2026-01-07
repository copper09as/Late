// 文件: NormalState.cs (Child)
// 说明: 子状态，表示卡牌的普通（未选中/无高亮）状态。
public class ChildNormalState : ChildCardState
{
    public override ChildCardStates StateType => ChildCardStates.Normal;

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