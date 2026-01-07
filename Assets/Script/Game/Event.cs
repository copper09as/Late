// 文件: Event.cs
// 说明: 定义游戏中使用的事件结构体，用于在事件总线中传递消息。
// 仅包含数据结构声明，不包含逻辑实现。
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
    public struct TurnAlreadyOver
    {
    }

    public struct LastStep
    {
    }
}