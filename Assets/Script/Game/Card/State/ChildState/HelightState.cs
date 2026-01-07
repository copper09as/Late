using System.Diagnostics;

// 文件: HelightState.cs
// 说明: 子状态，表示卡牌在被高亮（提示）时的视觉效果处理。
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
        cardPresentation.CancelFrameHighlight();
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
