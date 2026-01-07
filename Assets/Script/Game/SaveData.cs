// 文件: SaveData.cs
// 说明: 定义用于序列化/反序列化的保存数据结构（卡牌索引、ID、状态等）。
using System.Collections.Generic;

[System.Serializable]
public class SaveDataCollection
{
    public List<SaveData> historyDataList = new();

}
[System.Serializable]
public class SaveData
{
    public int seed;
    public int time;
    public GameState gameState;
    public int left;
    public List<int> CardIdList;
    public List<CardStates> cardStates;
    public List<ChildCardStates> childCardStates;
    public List<int> CardIndexList;
}