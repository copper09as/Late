// 文件: AlreadyUsedStateT.cs
// 说明: 父状态的变体（用于延迟或过渡态），在特定条件下切回其他状态。
public class AlreadyUsedStateT : CardState
{
    public override CardStates StateType => CardStates.AlreadyUsedT;

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
        var newState = new NormalState();
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