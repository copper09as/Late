// 文件: BeSelectState.cs
// 说明: 子状态，表示卡牌被选中时的视觉与行为处理（框高亮、响应回合结束恢复普通状态）。
class BeSelectState : ChildCardState
{
    public override ChildCardStates StateType => ChildCardStates.BeSelect;
    public override void Enter()
    {
        base.Enter();
        cardPresentation.BeSelectedStart();
        EventBus.Register<Game.Event.TurnOver>(ChangeNormalState);
    }

    public override void Execute()
    {
        // 在被选中状态下，等待其他逻辑处理
    }

    public override void Exit()
    {
        base.Exit();
        cardPresentation.CancelSelected();
        EventBus.Unregister<Game.Event.TurnOver>(ChangeNormalState);
    }
    protected virtual void ChangeNormalState(Game.Event.TurnOver _)
    {
        var newState = new ChildNormalState();
        newState.Init(cardStateMachine, cardPresentation);
        cardStateMachine.ChangeChildState(newState);
    }
    public override void Release()
    {
        base.Release();
        EventBus.Unregister<Game.Event.TurnOver>(ChangeNormalState);
    }

}