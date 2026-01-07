namespace Game.Event
{
    public struct GameWin
    {
        
    }
    public struct GameLose
    {
        
    }
    public struct CheckWin
    {
    }
    public struct UseCard
    {
        public int cardID;
    }
    public struct TimeFlow
    {
        public int time;
    }
    public struct Init
    {
    }
    public struct ReturnToMainMenu
    {
    }
    public struct ShowState
    {
        public string fatherState;
        public string childState;
    }
    public struct TurnOver
    {
        
    }

    /*
    public struct ReturnNormalState
    {
    }*/
}