
public abstract class ChildCardState
{
    protected CardStateMachine cardStateMachine;
    protected CardPresentation cardPresentation;
    public abstract ChildCardStates StateType { get; }
    public virtual void Init(CardStateMachine cardStateMachine,CardPresentation cardPresentation)
    {
        this.cardStateMachine = cardStateMachine;
        this.cardPresentation = cardPresentation;
    }
    public virtual void Enter()
    {
        
    }
    public abstract void Execute();
    public virtual void Exit()
    {
        
    }
}
public enum ChildCardStates
{
    Normal,
    BeSelect,
    Helight
}
