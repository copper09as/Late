using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class GameConfig:MonoBehaviour
{
    public static GameConfig Instance;
    public int seed;
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
    public string modePath = "CardMode/DefaultCardMode";
    public CardMode LoadMode()
    {
        CardMode CurrentMode = Resources.Load<CardMode>(modePath);
        Debug.Log("Load Mode from "+ modePath);
        return CurrentMode;
    }
}