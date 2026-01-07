// 文件: JsonTool.cs
// 说明: 简单的 JSON 序列化/反序列化工具，封装对 `JsonUtility` 和文件读写的调用。
using UnityEngine;

public static class JsonTool
{
    public static void SaveToJson<T>(string path, T obj)
    {
        string json = JsonUtility.ToJson(obj, true);
        try
        {
            System.IO.File.WriteAllText(path, json);
            Debug.Log($"Saved JSON to {path}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to save JSON to {path}: {e.Message}");
        }
    }
    public static T LoadFromJson<T>(string path)
    {
        try
        {
            if (!System.IO.File.Exists(path))
            {
                Debug.LogWarning($"JSON file not found at {path}");
                return default;
            }

            string json = System.IO.File.ReadAllText(path);
            T obj = JsonUtility.FromJson<T>(json);
                                       Debug.Log($"Loaded JSON from {path}");
            return obj;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to load JSON from {path}: {e.Message}");
            return default;
        }
    }
}