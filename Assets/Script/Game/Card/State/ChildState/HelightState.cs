using System.Diagnostics;

class HelightState : ChildCardState
{
    public override ChildCardStates StateType => ChildCardStates.Helight;
    public override void Enter()
    {
        cardPresentation.StartFrameHighlight();
        EventBus.Register<Game.Event.TurnOver>(ChangeNormalState);
    }

    public override void Execute()
    {
        // 在高亮状态下，等待其他逻辑处理
    }

    public override void Exit()
    {
        UnityEngine.Debug.Log("Exit Helight State");
        cardPresentation.CancelFrameHighlight();
        EventBus.Unregister<Game.Event.TurnOver>(ChangeNormalState);
    }
    protected virtual void ChangeNormalState(Game.Event.TurnOver _)
    {
        var newState = new ChildNormalState();
        newState.Init(cardStateMachine, cardPresentation);
        cardStateMachine.ChangeChildState(newState);
    }
}   
