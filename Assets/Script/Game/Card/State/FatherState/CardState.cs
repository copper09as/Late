
// 文件: CardState.cs
// 说明: 父状态基类，定义卡牌主要状态（Normal/Flipped/AlreadyUsed 等）及其生命周期接口。
public abstract class CardState
{
    protected CardStateMachine cardStateMachine;
    protected CardPresentation cardPresentation;
    public abstract CardStates StateType { get; }
    public ChildCardState childState;
    public virtual void Init(CardStateMachine cardStateMachine,CardPresentation cardPresentation,ChildCardState childState)
    {
        this.cardStateMachine = cardStateMachine;
        this.cardPresentation = cardPresentation;
        this.childState = childState;
    }
    public virtual void Reset()
    {
        
    }
    public virtual void Enter()
    {
        //EventBus.Register<Game.Event.ReturnNormalState>(ChangeNormalState);
    }
    public abstract void Execute();
    public virtual void Exit()
    {
        //EventBus.Unregister<Game.Event.ReturnNormalState>(ChangeNormalState);
    }

    /*
    protected virtual void ChangeNormalState(Game.Event.ReturnNormalState _)
    {
        var newState = new NormalState();
        newState.Init(cardStateMachine, cardPresentation);
        cardStateMachine.ChangeState(newState);
    
    }
    */
    public virtual void Release()
    {
        childState?.Release();
    }
}


public enum CardStates
{
    Normal,
    flipped,
    AlreadyUsed,
    AlreadyUsedT
}