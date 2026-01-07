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