using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI timeTxt;
    [SerializeField]private Button ReturnBtn;
    [SerializeField]private Button RestartButton;
    [SerializeField]private TextMeshProUGUI fatherStateTxt;
    [SerializeField]private TextMeshProUGUI childStateTxt;
    [SerializeField]private TextMeshProUGUI currentSeed;
    void Awake()
    {
        EventBus.Register<Game.Event.TimeFlow>(UpdateTime);
        EventBus.Register<Game.Event.ShowState>(UpdateStateText);
        EventBus.Register<Game.Event.Init>(UpdateSeedText);
        timeTxt.text="Time: 00";
        ReturnBtn.onClick.AddListener(()=>
        {
            EventBus.Publish(new Game.Event.ReturnToMainMenu());
            SceneManager.LoadScene("MainMenu");
        });
        RestartButton.onClick.AddListener(()=>
        {
            GameConfig.Instance.seed = DateTime.Now.Millisecond;
            EventBus.Publish(new Game.Event.Init());
        });
    }
    private void UpdateSeedText(Game.Event.Init initEvent)
    {
        currentSeed.text="Seed: "+GameConfig.Instance.seed;
    }
    private void UpdateStateText(Game.Event.ShowState showState)
    {
        fatherStateTxt.text="Father State: "+showState.fatherState;
        childStateTxt.text="Child State: "+showState.childState;
    }
    void OnDestroy()
    {
        EventBus.Unregister<Game.Event.TimeFlow>(UpdateTime);
        EventBus.Unregister<Game.Event.ShowState>(UpdateStateText);
        EventBus.Unregister<Game.Event.Init>(UpdateSeedText);
 
    }
    public void UpdateTime(Game.Event.TimeFlow timeFlow)
    {
        int time = timeFlow.time;
        timeTxt.text="Time: "+time.ToString("00");
    }

}
