// 文件: CardMode.cs
// 说明: 定义卡牌游戏的模式基类，包含卡牌布局（slots）、选择/使用卡牌的规则和洗牌/恢复逻辑。
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[CreateAssetMenu(fileName = "New CardMode", menuName = "GameMode/CardMode")]
public class CardMode : ScriptableObject
{
    [SerializeField] protected CardRepository cardRepository;
    protected struct GridSlot
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
                int id = i * 3 + j;
                slots.Add(new GridSlot(id, position));
            }
        }
    }
    protected List<GridSlot> slots;
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
        foreach (var card in runtimeGameData.cards)
        {
            ChildCardState normalState = new ChildNormalState();
            card.EnterChildState(normalState);
        }
    }
    protected virtual void UseAdjCardCallback(RuntimeGameData runtimeGameData)
    {
        SwapCard(runtimeGameData.currentChoseCard, runtimeGameData.cardBeClicked);
        if(runtimeGameData.currentChoseCard.StateType!=CardStates.flipped)
        {
            runtimeGameData.currentChoseCard.EnterState(new AlreadyUsedState());
        }
       
        FlipCards(runtimeGameData);
        runtimeGameData.Time++;
        runtimeGameData.currentChoseCard.Use(runtimeGameData);   
    }
    public virtual void ShuffleCards(RuntimeGameData runtimeGameData)
    {
        int seed = GameConfig.Instance.seed;
        var cards = runtimeGameData.cards;
        var slots = new List<GridSlot>(this.slots);
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
public virtual void ShuffleCards(List<int> preCardIdList, RuntimeGameData runtimeGameData)
{
    if (preCardIdList == null || runtimeGameData == null)
        return;
    
    var cards = runtimeGameData.cards;
    var cardDataList = cardRepository.allCards;
    
    // 验证长度
    if (preCardIdList.Count != cards.Count || preCardIdList.Count != slots.Count)
    {
        Debug.LogError($"长度不匹配: preCardIdList({preCardIdList.Count}), cards({cards.Count}), slots({slots.Count})");
        return;
    }
    
    // 创建卡牌到位置和数据的直接映射
    for (int i = 0; i < cards.Count; i++)
    {
        // preCardIdList[i] 应该是目标卡牌的ID，而不是索引
        int targetCardId = preCardIdList[i];
        
        // 找到对应的slot（根据当前索引i，而不是targetCardId）
        GridSlot slot = slots[i];  // 关键修改：使用i而不是targetCardId
        
        // 找到对应的卡片数据
        CardData cardData = null;
        for (int j = 0; j < cardDataList.Count; j++)
        {
            if (cardDataList[j].cardId == targetCardId)  // 根据ID查找
            {
                cardData = cardDataList[j];
                break;
            }
        }
        
        if (cardData == null)
        {
            Debug.LogError($"找不到ID为{targetCardId}的卡片数据");
            continue;
        }
        
        // 设置卡片位置和数据
        cards[i].transform.position = slot.position;
        cards[i].Init(slot.id, cardData);
    }
}

    public virtual void ShuffleCardsContinue(List<CardPresentation> cards, SaveData gameData)
    {
        int count = gameData.CardIdList.Count;
        for (int i = 0; i < count; i++)
        {
            int slotId = gameData.CardIndexList[i];
            int slotListIndex = slots.FindIndex(s => s.id == slotId);
            if (slotListIndex < 0) continue;
            GridSlot slot = slots[slotListIndex];
            cards[i].transform.position = slot.position;
            var cardDataList = cardRepository.allCards;
            var cardData = cardDataList.Find(data => data.cardId == gameData.CardIdList[i]);
            cards[i].Init(slot.id, cardData);
            cards[i].EnterState(gameData.cardStates[i]);
            cards[i].EnterChildState(gameData.childCardStates[i]);
        } 
        Debug.Log("ShuffleCardsContinue completed.");
    }
    
    public virtual bool CheckWin(RuntimeGameData runtimeGameData)
    {
        var data = runtimeGameData.GetCardData();
        bool flag = true;
       //runtimeGameData.SaveCardData(data);
        foreach (var card in runtimeGameData.cards)
        {
            if (card.StateType == CardStates.flipped)
            {
                flag = false;
                break;
            }
            if (card.CardId != card.index)
            {
                flag = false;
                break;
            }
        }
        if(!flag)
        {
            return false;
        }
        else
        {
            return true;
        }
    }    
    private void FlipCards(RuntimeGameData runtimeGameData)
    {

        var cardList = runtimeGameData.currentChoseCard.GetFunctionArea(runtimeGameData);
        foreach (var card in cardList)
        {
            //在正确位置并已经被使用的牌不翻转
            
            if (card == runtimeGameData.currentChoseCard && card.index==card.CardId)
            {
                continue;
            }
            else
            {
                card.Flip();
            }
        }
    }

}
