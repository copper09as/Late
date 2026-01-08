using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class GameConfig:MonoBehaviour
{
    public static GameConfig Instance;
    public int seed;
    public bool continueMode = false;
    public List<int> preCardIdList;
// 文件: GameConfig.cs
// 说明: 管理游戏的配置与选项（如随机种子、继续模式标志等），并提供单例访问。
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public string modePath = "CardMode/CardMode";
    public CardMode LoadMode()
    {
        CardMode CurrentMode = Resources.Load<CardMode>(modePath);
        Debug.Log("Load Mode from "+ modePath);
        return CurrentMode;
    }
}