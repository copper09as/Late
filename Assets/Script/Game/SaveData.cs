// 文件: SaveData.cs
// 说明: 定义用于序列化/反序列化的保存数据结构（卡牌索引、ID、状态等）。
using System.Collections.Generic;

[System.Serializable]
public class SaveDataCollection
{
public List<SaveData> historyDataList = new List<SaveData>();
private int currentIndex = -1;

// 添加历史记录
public void AddHistoryData(SaveData data)
{
    historyDataList.Add(data);
    SynIndex();
}
public void SynIndex()
{
    currentIndex = historyDataList.Count - 1;
}
// 撤回
public SaveData Undo()
{
    if(historyDataList.Count <=1)
        return null;
    SaveData data = null;
    if(currentIndex == historyDataList.Count - 1)
    {
        data = historyDataList[currentIndex-1];
        historyDataList.RemoveAt(currentIndex);
        return data;
    }
    int index = historyDataList.Count - 2;
    if (index < 0)
        return null;
    data = historyDataList[index];
    historyDataList.RemoveAt(index+1);

    return data;

}

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
    public override string ToString()
    {
        return 
        $"Seed: {seed}, Time: {time}, GameState: {gameState}, Left: {left}, CardIds: [{string.Join(", ", CardIdList)}]";
    }
}