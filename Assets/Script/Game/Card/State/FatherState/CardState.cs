
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
}


public enum CardStates
{
    Normal,
    flipped,
    AlreadyUsed
}