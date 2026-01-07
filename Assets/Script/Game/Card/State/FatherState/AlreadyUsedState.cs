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
        Debug.Log(count);
        if (count <=  0) 
        {
            count++;
            return;
        }
        var newState = new NormalState();
        Debug.Log("Change to Normal State from AlreadyUsedState");
        newState.Init(cardStateMachine, cardPresentation,childState);
        cardStateMachine.ChangeState(cardPresentation,newState);
    }
    public override void Reset()
    {       

    }
    public override void Release()
    {
        base.Release();
        EventBus.Unregister<Game.Event.TurnOver>(ChangeNormalState);
    }
}