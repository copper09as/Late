
// 文件: GameManager.cs
// 说明: 管理游戏主要流程，注册并响应事件（使用卡牌、初始化、检查胜利、上一步等）。
// 该类负责初始化游戏模式、处理输入以及调用卡牌模式相关方法。
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


class GameManager : MonoBehaviour
{
    private CardMode cardMode;
    [SerializeField] private RuntimeGameData runtimeGameData;
    [SerializeField] private GameObject cardPrefab;

    void Awake()
    {
        EventBus.Register<Game.Event.UseCard>(OnUseCard);
        EventBus.Register<Game.Event.Init>(Init);
        EventBus.Register<Game.Event.CheckWin>(CheckWin);
        EventBus.Register<Game.Event.LastStep>(LastStep);
    }
    void Start()
    {
        EventBus.Publish(new Game.Event.Init());
    }
    void OnDestroy()
    {
        EventBus.Unregister<Game.Event.UseCard>(OnUseCard);
        EventBus.Unregister<Game.Event.Init>(Init);
        EventBus.Unregister<Game.Event.CheckWin>(CheckWin);
        EventBus.Unregister<Game.Event.LastStep>(LastStep);
    }
    void Init(Game.Event.Init initEvent)
    {
        Init();
    }
    void LastStep(Game.Event.LastStep lastStepEvent)
    {
        //runtimeGameData.GetLastSaveData();
        //cardMode.ShuffleCardsContinue(runtimeGameData.cards, runtimeGameData.GetLastSaveData());
    }
    void Init()
    {
        if (runtimeGameData.cards.Count == 0)
        {
            runtimeGameData.cards = new List<CardPresentation>();
            for (int i = 0; i < 9; i++)
            {
                var cardObj = Instantiate(cardPrefab);
                var card = cardObj.GetComponent<CardPresentation>();
                runtimeGameData.cards.Add(card);
            }
        }

        cardMode = GameConfig.Instance.LoadMode();
        cardMode.Init(runtimeGameData);
        ShuffleCards();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)) OnUseCard(new Game.Event.UseCard { cardID = runtimeGameData.cards.Find(i => i.index == 0).CardId });
        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)) OnUseCard(new Game.Event.UseCard { cardID = runtimeGameData.cards.Find(i => i.index == 1).CardId });
        if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3)) OnUseCard(new Game.Event.UseCard { cardID = runtimeGameData.cards.Find(i => i.index == 2).CardId });
        if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4)) OnUseCard(new Game.Event.UseCard { cardID = runtimeGameData.cards.Find(i => i.index == 3).CardId });
        if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5)) OnUseCard(new Game.Event.UseCard { cardID = runtimeGameData.cards.Find(i => i.index == 4).CardId });
        if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6)) OnUseCard(new Game.Event.UseCard { cardID = runtimeGameData.cards.Find(i => i.index == 5).CardId });
        if (Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown(KeyCode.Keypad7)) OnUseCard(new Game.Event.UseCard { cardID = runtimeGameData.cards.Find(i => i.index == 6).CardId });
        if (Input.GetKeyDown(KeyCode.Alpha8) || Input.GetKeyDown(KeyCode.Keypad8)) OnUseCard(new Game.Event.UseCard { cardID = runtimeGameData.cards.Find(i => i.index == 7).CardId });
        if (Input.GetKeyDown(KeyCode.Alpha9) || Input.GetKeyDown(KeyCode.Keypad9)) OnUseCard(new Game.Event.UseCard { cardID = runtimeGameData.cards.Find(i => i.index == 8).CardId });
    }
    private void CheckWin(Game.Event.CheckWin checkWinEvent)
    {
        if (cardMode.CheckWin(runtimeGameData))
        {
            GameWin();
        }
    }
    private void GameWin()
    {
        runtimeGameData.gameState = GameState.Win;
        var winDic = new Dictionary<string, System.Action>();
        winDic.Add("Restart", () =>
        {
            Init();
        });
        winDic.Add("Main Menu", () =>
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        });
        winDic.Add("Quit", () =>
        {
            Application.Quit();
        });
        winDic.Add("Cancel", null);
        MessageBox.Instance.ShowMessage(winDic, "You Win!Spend times: " + runtimeGameData.Time);
    }
    private void GameLose()
    {
        runtimeGameData.gameState = GameState.Lose;
        var loseDic = new Dictionary<string, System.Action>();
        loseDic.Add("Restart", () =>
        {
            Init();
        });
        loseDic.Add("Main Menu", () =>
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        });
        loseDic.Add("Quit", () =>
        {
            Application.Quit();
        });
        loseDic.Add("Cancel", null);
        MessageBox.Instance.ShowMessage(loseDic, "You Lose!");
    }
    public void OnUseCard(Game.Event.UseCard useCardEvent)
    {
        var card = runtimeGameData.cards.Find(c => c.CardId == useCardEvent.cardID);
        runtimeGameData.cardBeClicked = card;
        cardMode.ChoseCard(runtimeGameData);
        //SaveData saveData = runtimeGameData.GetCardData();
    }
    public void ShuffleCards()
    {
        /*
        if (GameConfig.Instance.continueMode)
        {
            cardMode.ShuffleCardsContinue(runtimeGameData);
            GameConfig.Instance.continueMode = false;
            return;
        }*/
        cardMode.ShuffleCards(runtimeGameData);
        //runtimeGameData.SaveCardData(runtimeGameData.GetCardData());
    }
}