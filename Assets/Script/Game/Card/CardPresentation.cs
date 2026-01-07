using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardPresentation : MonoBehaviour
{
    [SerializeField] private CardData cardData;
    public int index;
    public SpriteRenderer frame;
    private SpriteRenderer sr;
    private CardStateMachine cardStateMachine = new();
    private Color frameOriginalColor;

    // 新增：保存协程引用，便于正确停止
    private Coroutine beSelectedCoroutine;
    private Coroutine frameHighlightCoroutine;
    public CardStates StateType => cardStateMachine.GetStateType();

    public void Flip()
    {
        StopAllCoroutines();
        bool isFlip = StateType == CardStates.flipped;
        if (isFlip)
        {
            var normalState = new NormalState();
            EnterState(normalState);
            UnityEngine.Debug.Log("Flip to Normal State");
        }
        else
        {
            var flippedState = new FlippedState();
            EnterState(flippedState);
            UnityEngine.Debug.Log("Flip to Flipped State");
        }
        SetImage(!isFlip);
        StartCoroutine(FlipCoroutine());
    }
    public void Init(int index, CardData cardData)
    {
        sr = GetComponent<SpriteRenderer>();
        frame = transform.Find("Frame")?.GetComponent<SpriteRenderer>() ?? sr;
        if (frame != null)
        {
            frame.gameObject.SetActive(false);
            frameOriginalColor = frame.color;
        }
        this.index = index;
        this.cardData = cardData;
        this.name = cardData.name;
        SetImage(false);
        var normalState = new NormalState();
        EnterState(normalState);
    }
    public void EnterState(CardState state)
    {
        cardStateMachine.ChangeState(this,state);
    }
    public void EnterChildState(ChildCardState state)
    {
        state.Init(cardStateMachine,this);
        cardStateMachine.ChangeChildState(state);
    }
    private void SetImage(bool isFlip)
    {
        if (!isFlip)
            sr.sprite = cardData.cardImage;
        else
            sr.sprite = cardData.flipCardImage;
    }
    public void AlreadyUsed()
    {
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.5f);
    }
    public void NormalUsed()
    {
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);
    }
    private void OnMouseDown()
    {
        EventBus.Publish(new Game.Event.UseCard { cardID = this.CardId });
        EventBus.Publish(new Game.Event.ShowState
        {
            fatherState = this.StateType.ToString(),
            childState = this.cardStateMachine.GetChildStateType().ToString()
        });
    }
    //public CardStates StateType => cardStateMachine.GetStateType();
    public void BeSelectedStart()
    {
        // 启动选中时取消可能的框高亮，保证互斥
        if (frameHighlightCoroutine != null)
        {
            StopCoroutine(frameHighlightCoroutine);
            frameHighlightCoroutine = null;
            if (frame != null) frame.color = frameOriginalColor;
        }

        if (frame != null) frame.gameObject.SetActive(true);
        if (beSelectedCoroutine == null)
            beSelectedCoroutine = StartCoroutine(BeSelected());
    }   
    //被选中逐渐改变颜色
    private IEnumerator BeSelected()
    {
        if (frame == null && sr == null)
            yield break;

        SpriteRenderer target = frame != null ? frame : sr;
        if (target == null)
            yield break;

        Color original = target.color;
        Color targetColor = Color.grey;

        float toDuration = 0.4f;
        float holdDuration = 0.2f;
        float backDuration = 0.4f;
        while (true)
        {
            float elapsed = 0f;
            while (elapsed < toDuration)
            {
                float t = elapsed / toDuration;
                target.color = Color.Lerp(original, targetColor, t);
                elapsed += Time.deltaTime;
                yield return null;
            }

            target.color = targetColor;

            elapsed = 0f;
            while (elapsed < holdDuration)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }

            elapsed = 0f;
            while (elapsed < backDuration)
            {
                float t = elapsed / backDuration;
                target.color = Color.Lerp(targetColor, original, t);
                elapsed += Time.deltaTime;
                yield return null;
            }

            target.color = original;
        }
    }
    /*
    0 1 2      2%3 == 1 
    3 4 5
    6 7 8
    */
    public bool AdjCard(int otherIndex)
    {
        int diff = otherIndex - this.index;
        int abs = Mathf.Abs(diff);
        // vertical neighbor (up/down)
        if (abs == 3) return true;
        // horizontal neighbor (left/right) - ensure same row to avoid wrap-around
        if (abs == 1)
            return this.index / 3 == otherIndex / 3;
        return false;
    }
    public void CancelSelected()
    {
        if (beSelectedCoroutine != null)
        {
            StopCoroutine(beSelectedCoroutine);
            beSelectedCoroutine = null;
        }
        if (frame != null)
        {
            frame.color = frameOriginalColor;
            frame.gameObject.SetActive(false);
        }
    }

    // 新增：开始蓝色框高亮（循环从原色到蓝再回原色）
    public void StartFrameHighlight()
    {
        // 启动框高亮时取消选中协程，保证互斥
        if (beSelectedCoroutine != null)
        {
            StopCoroutine(beSelectedCoroutine);
            beSelectedCoroutine = null;
            if (frame != null) frame.color = frameOriginalColor;
        }

        if (frame == null) return;
        frame.gameObject.SetActive(true);
        if (frameHighlightCoroutine == null)
            frameHighlightCoroutine = StartCoroutine(FrameHighlight());
    }

    // 新增：取消蓝色框高亮并恢复颜色
    public void CancelFrameHighlight()
    {
        if (frameHighlightCoroutine != null)
        {
            StopCoroutine(frameHighlightCoroutine);
            frameHighlightCoroutine = null;
        }
        if (frame != null)
        {
            frame.color = frameOriginalColor;
            frame.gameObject.SetActive(false);
        }
    }

    // 新增：蓝色高亮协程
    private IEnumerator FrameHighlight()
    {
        if (frame == null)
            yield break;

        Color original = frameOriginalColor;
        Color targetColor = Color.cyan; // 蓝色高亮（可调整为 Color.blue）

        float toDuration = 0.35f;
        float holdDuration = 0.15f;
        float backDuration = 0.35f;

        while (true)
        {
            float elapsed = 0f;
            while (elapsed < toDuration)
            {
                float t = elapsed / toDuration;
                frame.color = Color.Lerp(original, targetColor, t);
                elapsed += Time.deltaTime;
                yield return null;
            }

            frame.color = targetColor;

            elapsed = 0f;
            while (elapsed < holdDuration)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }

            elapsed = 0f;
            while (elapsed < backDuration)
            {
                float t = elapsed / backDuration;
                frame.color = Color.Lerp(targetColor, original, t);
                elapsed += Time.deltaTime;
                yield return null;
            }

            frame.color = original;
        }
    }

    /*
    0 1 2      2%3 == 1 
    3 4 5
    6 7 8
    */

    //鼠标进入和退出后产生视觉效果
    void OnMouseEnter()
    {
        transform.localScale *= 1.1f;
        EventBus.Publish(new Game.Event.ShowState
        {
            fatherState = this.StateType.ToString(),
            childState = this.cardStateMachine.GetChildStateType().ToString()
        });
    }
    void OnMouseExit()
    {
        transform.localScale /= 1.1f;
        EventBus.Publish(new Game.Event.ShowState
        {
            fatherState = "None",
            childState = "None"
        });
    }
   private IEnumerator FlipCoroutine()
    {

        float duration = 0.25f;
        float elapsed = 0f;
        Quaternion from = transform.rotation;
        Quaternion to = from * Quaternion.Euler(0f, 180f, 0f);

        while (elapsed < duration)
        {
            transform.rotation = Quaternion.Slerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = from;

    }
    
    public void Use(RuntimeGameData runtimeGameData)
    {
        cardData.UseCard(runtimeGameData);
    }
    public List<CardPresentation> GetFunctionArea(RuntimeGameData runtimeGameData)
    {
        var cardIndexes = cardData.FunctionArea;
        var cardList = runtimeGameData.cards.Where(i => cardIndexes.Contains(i.index)).ToList();
        return cardList;
    }
    public int CardId => cardData.cardId;

    public ChildCardStates ChildStateType => cardStateMachine.GetChildStateType();
}
