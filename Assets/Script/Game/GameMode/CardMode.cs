using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[CreateAssetMenu(fileName = "New CardMode", menuName = "GameMode/CardMode")]
public class CardMode : ScriptableObject
{
    [SerializeField] private CardRepository cardRepository;
    private struct GridSlot
    {
        public int id;
        public Vector2 position;

        public GridSlot(int id, Vector2 position)
        {
            this.id = id;
            this.position = position;
        }
    }
    protected virtual void InitSlots()
    {
        slots = new List<GridSlot>(9);
        float startX = -3f;
        float startY = 3f;
        float offsetX = 3f;
        float offsetY = -3f;

        for (int j = 0; j < 3; j++)
        {
            for (int i = 0; i < 3; i++)
            {
                Vector2 position = new Vector2(startX + j * offsetX, startY + i * offsetY);

                // id: 从上往下（同一列 top->bottom），再到下一列；3x3 => 0..8
                int id = i * 3 + j;
                slots.Add(new GridSlot(id, position));
            }
        }
    }
    private List<GridSlot> slots;
    protected List<GameCondition> gameConditions;
    public virtual void ChoseCard(RuntimeGameData runtimeGameData)
    {
        foreach (var i in gameConditions)
        {
            if (i.ConditionallyTrue(runtimeGameData))
                return;
        }
    }
    private void SwapCard(CardPresentation cardA, CardPresentation cardB)
    {
        (cardB.transform.position, cardA.transform.position) = (cardA.transform.position, cardB.transform.position);
        int tempIndex = cardA.index;
        cardA.index = cardB.index;
        cardB.index = tempIndex;
    }
    public virtual void Init(RuntimeGameData runtimeGameData)
    {
        InitSlots();

        NotInPlayingCondition notInPlayingCondition = new NotInPlayingCondition();
        ChoseSameCardCondition choseSameCardCondition = new ChoseSameCardCondition();
        UseAdjCardCondition useAdjCardCondition = new  UseAdjCardCondition();
        UseCardCondition useCardCondition = new UseCardCondition();
        UseCardEqualLastCardCondition useCardEqualLastCardCondition = new UseCardEqualLastCardCondition();
        UseCardIfFlipCondition useCardIfFlipCondition = new UseCardIfFlipCondition(); 

        useCardEqualLastCardCondition.Init(new List<Action<RuntimeGameData>> {});
        useCardCondition.Init(new List<Action<RuntimeGameData>> { UseCardCallback });
        useAdjCardCondition.Init(new List<Action<RuntimeGameData>> { UseAdjCardCallback});
        useCardIfFlipCondition.Init(new List<Action<RuntimeGameData>> {});
        notInPlayingCondition.Init(new List<Action<RuntimeGameData>> {});
        choseSameCardCondition.Init(new List<Action<RuntimeGameData>> { CancelSelection});

        gameConditions = new List<GameCondition>
         { 
            notInPlayingCondition,
            useCardIfFlipCondition,
            choseSameCardCondition,
            useCardEqualLastCardCondition,
            useCardCondition, 
            useAdjCardCondition
        };
        runtimeGameData.Reset();
        
    }

    protected virtual void UseCardCallback(RuntimeGameData runtimeGameData)
    {
        runtimeGameData.cardBeClicked.EnterChildState(new BeSelectState());
        var cardList = 
        runtimeGameData.cardBeClicked.GetFunctionArea
        (runtimeGameData).Where(i=>i!=runtimeGameData.cardBeClicked);
        foreach (var card in cardList)
        {
            var helightState = new HelightState();
            card.EnterChildState(helightState);
        }
        var selectedState = new BeSelectState();
        runtimeGameData.cardBeClicked.EnterChildState(selectedState);
    }
    
    protected virtual void CancelSelection(RuntimeGameData runtimeGameData)
    {
        runtimeGameData.currentChoseCard.CancelSelected();
        //runtimeGameData.currentChoseCard = null;
        foreach (var card in runtimeGameData.cards)
        {
            ChildCardState normalState = new ChildNormalState();
            card.EnterChildState(normalState);
        }
    }
    protected virtual void UseAdjCardCallback(RuntimeGameData runtimeGameData)
    {
        SwapCard(runtimeGameData.currentChoseCard, runtimeGameData.cardBeClicked);
        FlipCards(runtimeGameData);
        //runtimeGameData.currentChoseCard.CancelSelected();
        if(runtimeGameData.currentChoseCard.StateType!=CardStates.flipped)
        {
            runtimeGameData.currentChoseCard.EnterState(new AlreadyUsedState());
        }
        //runtimeGameData.currentChoseCard = null;
        runtimeGameData.Time++;
        EventBus.Publish(new Game.Event.CheckWin { });
        EventBus.Publish(new Game.Event.TurnOver { });
    }

    public virtual void ShuffleCards(RuntimeGameData runtimeGameData)
    {
        int seed = GameConfig.Instance.seed;
        var cards = runtimeGameData.cards;
        Debug.Log("Shuffle with seed: " + seed);
        for (int i = 0; i < cards.Count; i++)
        {
            UnityEngine.Random.InitState(seed);
            int randomIndex = UnityEngine.Random.Range(0, slots.Count);
            GridSlot slot = slots[randomIndex];
            cards[i].transform.position = slot.position;
            var cardDataList = cardRepository.allCards;
            cards[i].Init(slot.id, cardDataList[i]);
            slots.RemoveAt(randomIndex);
        }
    }
    public virtual bool CheckWin(RuntimeGameData runtimeGameData)
    {
        foreach (var card in runtimeGameData.cards)
        {
            if (card.StateType == CardStates.flipped)
            {
                return false;
            }
            if (card.CardId != card.index)
            {
                return false;
            }
        }
        return true;
    }    
    private void FlipCards(RuntimeGameData runtimeGameData)
    {

        var cardList = runtimeGameData.currentChoseCard.GetFunctionArea(runtimeGameData);
        foreach (var card in cardList)
        {
            if (card.StateType!= CardStates.AlreadyUsed || card.index!=card.CardId)
            {
                card.Flip();
            }
        }
        runtimeGameData.currentChoseCard.Use(runtimeGameData);
    }

}
