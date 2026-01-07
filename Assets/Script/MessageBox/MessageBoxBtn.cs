using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

class MessageBoxBtn : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI btnText;
    [SerializeField] private Button btn;
    private List<Action> btnAction;

    public void Init(string text, List<Action> action)
    {
        btnText.text = text;
        btnAction = action;
        btn.onClick.AddListener(OnBtnClick);
    }
    public void OnBtnClick()
    {
        foreach (var action in btnAction)
        {
            action?.Invoke();
        }
    }
}