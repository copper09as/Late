// 文件: MessageBoxBtn.cs
// 说明: 消息弹窗按钮组件，初始化显示文本并在点击时执行一系列回调。
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