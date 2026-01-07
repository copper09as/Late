using System.Diagnostics;

public class CardStateMachine
{
    private CardState currentState;

    public void ChangeState(CardPresentation cardPresentation, CardState newState)
    {
        newState.Init(this, cardPresentation, currentState?.childState);
        if (currentState != null)
        {
            currentState.Exit();
        }
        currentState = newState;
        currentState.Enter();
    }
    public void ChangeChildState(ChildCardState newState)
    {
        if (currentState != null && currentState.childState != null)
        {
            currentState.childState.Exit();
        }
        currentState.childState = newState;
        currentState.childState.Enter();
    }
    public CardStates GetStateType()
    {
        if (currentState != null)
        {
            return currentState.StateType;
        }
        return default;
    }
    public ChildCardStates GetChildStateType()
    {
        if (currentState != null && currentState.childState != null)
        {
            return currentState.childState.StateType;
        }
        return default;
    }
    public void ResetCurrentState()
    {
        currentState?.Reset();
    }
    public void Release()
    {
        currentState.Release();
    }
}