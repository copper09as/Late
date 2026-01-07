using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessageBox : MonoBehaviour
{
    public static MessageBox Instance;
    [SerializeField]private Transform messageBoxTransform;
    [SerializeField]private GameObject btnPrefab;
    [SerializeField]private TextMeshProUGUI messageText;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        gameObject.SetActive(false);
    }

    public void ShowMessage(Dictionary<string,Action> btnActionDic,string message)
    {
        if(gameObject.activeSelf)
            return;
        messageText.text = message;
        int count = btnActionDic.Count;
        gameObject.SetActive(true);
        
        foreach(var i in btnActionDic)
        {
            Action btnAction = i.Value;
            List<Action> actionList = new List<Action> { HideMessageBox, ClearMessageBox, btnAction };
            GameObject btnObj = Instantiate(btnPrefab, messageBoxTransform);
            btnObj.GetComponent<MessageBoxBtn>().Init(i.Key, actionList);
        }
    }
    private void ClearMessageBox()
    {
        foreach(Transform child in messageBoxTransform)
        {
            Destroy(child.gameObject);
        }
    }
    private void HideMessageBox()
    {
        gameObject.SetActive(false);
    }
}
