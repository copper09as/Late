using UnityEngine;

public class AlreadyUsedState : CardState
{
    public override CardStates StateType => CardStates.AlreadyUsed;
    private int count = 0;

    public override void Enter()
    {
        base.Enter();
        cardPresentation.AlreadyUsed();
        EventBus.Register<Game.Event.TurnOver>(ChangeNormalState);
    }

    public override void Execute()
    {
    }

    public override void Exit()
    {
        base.Exit();
        cardPresentation.NormalUsed();
        EventBus.Unregister<Game.Event.TurnOver>(ChangeNormalState);
    }
    protected virtual void ChangeNormalState(Game.Event.TurnOver _)
    {
        if (count++ ==  0) return;
        var newState = new NormalState();
        newState.Init(cardStateMachine, cardPresentation, new ChildNormalState());
        cardStateMachine.ChangeState(cardPresentation,newState);
    }
}