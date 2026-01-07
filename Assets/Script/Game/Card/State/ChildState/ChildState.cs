
// 文件: ChildState.cs
// 说明: 子状态基类，定义子状态生命周期接口（Init/Enter/Execute/Exit/Release）。
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
    public virtual void Release()
    {
        
    }
}
public enum ChildCardStates
{
    Normal,
    BeSelect,
    Helight
}
