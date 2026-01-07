using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[System.Serializable]
public class RuntimeGameData
{
    private int _time = 0;
    public int Time
    {
        get { return _time; }
        set { _time = value;EventBus.Publish(new Game.Event.TimeFlow { time = _time }); }
    }
    //public CardPresentation lastCard = null;
    //public CardPresentation currentChoseCard = null;
    public CardPresentation cardBeClicked = null;
    [SerializeField]public List<CardPresentation> cards;
    public GameState gameState = GameState.Playing;
    public int left = 0;
    public void Reset()
    {
        Time = 0;
        cardBeClicked = null;
        gameState = GameState.Playing;
        left = 0;
    }
    public RuntimeGameData LoadCardData()
    {
        return null;
    }
    public void SaveCardData()
    {

    }
    public CardPresentation currentChoseCard=>cards.Find(card => card.ChildStateType ==ChildCardStates.BeSelect);
}
public enum GameState
{
    Playing,
    Paused,
    Win,
    Lose
}