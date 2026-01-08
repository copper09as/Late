using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InputFieldMain: MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField]private TMP_InputField seedInputField;
    [SerializeField]private Button transformButton;
    [SerializeField]private GameObject stateImages;
    public int state = 0;

    private void Start()
    {
        transformButton.onClick.AddListener(transformState);
    }

    private void transformState()
    {
        if(state == 0)
        {
            seedInputField.text = "Enter Pre...";
            state = 1;
        }
        else
        {
           seedInputField.text = "";
            state = 0;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(state == 0)
        {
            return;
        }
        stateImages.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
       if(state == 0)
        {
            return;
        }
        stateImages.SetActive(false);
    }
    public void GameStart()
    {
        GameConfig.Instance.preCardIdList.Clear();
        if(state == 0)
        {
        if (string.IsNullOrEmpty(seedInputField.text))
            GameConfig.Instance.seed = DateTime.Now.Millisecond*DateTime.Now.Second*DateTime.Now.DayOfWeek.GetHashCode();
        else if(int.TryParse(seedInputField.text,out int seed) && seed != 0)
        {
            GameConfig.Instance.seed = seed;
        }
        else
            GameConfig.Instance.seed = seedInputField.text.GetHashCode();
        }
        else
        {
            string preInput = seedInputField.text;
            if(preInput.Length!=9)
            {
                var dic = new Dictionary<string,Action>();
                Action action = null;
                dic.Add("OK",action);
                MessageBox.Instance.ShowMessage(dic,"Pre Seed Length must be 9,such as 123456789");
                return;
            }
            foreach(char c in preInput)
            {
                if(!char.IsDigit(c))
                {
                    var dic = new Dictionary<string,Action>();
                    Action action = null;
                    dic.Add("OK",action);
                    MessageBox.Instance.ShowMessage(dic,"Pre Seed must be all digit,such as 123456789");
                    return;
                }
                else if(c == '0')
                {
                    var dic = new Dictionary<string,Action>();
                    Action action = null;
                    dic.Add("OK",action);
                    MessageBox.Instance.ShowMessage(dic,"Pre Seed must not contain 0,such as 123456789");
                    return;
                }
                else
                {
                    int digit = int.Parse(c.ToString())-1;
                    if(GameConfig.Instance.preCardIdList.Contains(digit))
                    {
                        var dic = new Dictionary<string,Action>();
                        Action action = null;
                        dic.Add("OK",action);
                        MessageBox.Instance.ShowMessage(dic,"Pre Seed must not contain duplicate digit,such as 123456789");
                        return;
                    }
                    GameConfig.Instance.preCardIdList.Add(digit);
                }
            }

        }
         SceneManager.LoadScene("MainGame");
    }

}