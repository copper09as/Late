// 文件: RuntimeGameData.cs
// 说明: 存放运行时的游戏数据（卡牌列表、时间、状态等），并提供保存/加载接口。
using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
[System.Serializable]
public class RuntimeGameData
{
    private int _time = 0;
    public int Time
    {
        get { return _time; }
        set { _time = value; EventBus.Publish(new Game.Event.TimeFlow { time = _time }); }
    }
    public CardPresentation cardBeClicked = null;
    [SerializeField]private SaveDataCollection saveDataCollection = new SaveDataCollection();
    public List<CardPresentation> cards;
    public GameState gameState = GameState.Playing;
    public int left = 0;
    public void Reset()
    {
        Time = 0;
        cardBeClicked = null;
        gameState = GameState.Playing;
        left = 0;
    }
    /*
    public SaveData LoadCardData()
    {
        saveDataCollection = JsonTool.LoadFromJson<SaveDataCollection>(Application.persistentDataPath + "/saveData.json");
        return LoadCardData(saveDataCollection);
    }
    */
    /*
    public SaveData LoadCardData(SaveDataCollection saveDataCollection)
    {
        var saveData = saveDataCollection.PopDataFromHistoryStack();
        Time = saveData.time;
        gameState = saveData.gameState;
        left = saveData.left;
        GameConfig.Instance.seed = saveData.seed;
        return saveData;
    }*/
    public SaveData GetCardData()
    {
        SaveData saveData = new SaveData();
        saveData.seed = GameConfig.Instance.seed;
        saveData.time = Time;
        saveData.gameState = gameState;
        saveData.left = left;
        saveData.CardIdList = new List<int>();
        saveData.cardStates = new List<CardStates>();
        saveData.childCardStates = new List<ChildCardStates>();
        saveData.CardIndexList = new List<int>();
        foreach (var card in cards)
        {
            saveData.CardIdList.Add(card.CardId);
        }
        foreach (var card in cards)
        {
            saveData.cardStates.Add(card.StateType);
            saveData.childCardStates.Add(card.ChildStateType);
        }
        foreach (var card in cards)
        {
            saveData.CardIndexList.Add(card.index);
        }

        return saveData;
    }

    public CardPresentation currentChoseCard => cards.Find(card => card.ChildStateType == ChildCardStates.BeSelect);
}
public enum GameState
{
    Playing,
    Paused,
    Win,
    Lose
}