// 文件: MainUiManager.cs
// 说明: 主菜单的 UI 管理，处理模式选择、种子输入与场景跳转。
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainUiManager : MonoBehaviour
{
    [SerializeField]private Button ClassicModeBtn;
    [SerializeField]private Button DarkModeBtn;
    [SerializeField]private Button BiliLinkBtn;
    [SerializeField]private Button ContinueModeBtn;
    [SerializeField]private TMP_InputField seedInput;
    private string biliLink = "https://www.bilibili.com/video/BV1xNqrBDE4Y/?spm_id_from=333.1391.0.0&vd_source=45bde1244c0f43e2f9d9e5830f6c0518";
    // Start is called before the first frame update
    void Start()
    {
        ClassicModeBtn.onClick.AddListener(OnClassicModeClicked);
        DarkModeBtn.onClick.AddListener(OnDarkModeClicked);
        BiliLinkBtn.onClick.AddListener(() => Application.OpenURL(biliLink));
        ContinueModeBtn.onClick.AddListener(OnContinueModeClicked);
    }
    private void OnClassicModeClicked()
    {
        if (string.IsNullOrEmpty(seedInput.text))
            GameConfig.Instance.seed = DateTime.Now.Millisecond;
        else if(int.TryParse(seedInput.text,out int seed) && seed != 0)
        {
            GameConfig.Instance.seed = seed;
        }
        else
            GameConfig.Instance.seed = seedInput.text.GetHashCode();
        GameConfig.Instance.modePath = "Data/GameMode/CardMode";
        SceneManager.LoadScene("MainGame");
        
    }
    private void OnContinueModeClicked()
    {
        GameConfig.Instance.continueMode = true;
        GameConfig.Instance.modePath = "Data/GameMode/CardMode";
        SceneManager.LoadScene("MainGame");
    }
    private void OnDarkModeClicked()
    {
        if (string.IsNullOrEmpty(seedInput.text))
            GameConfig.Instance.seed = DateTime.Now.Millisecond;
        else if(int.TryParse(seedInput.text,out int seed) && seed != 0)
        {
            GameConfig.Instance.seed = seed;
        }
        else
            GameConfig.Instance.seed = seedInput.text.GetHashCode();
        GameConfig.Instance.modePath = "Data/GameMode/DarkMode";
        SceneManager.LoadScene("MainGame");
    }
}
