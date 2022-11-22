using System;
using System.Collections.Generic;
using UnityEngine;

public class JsonHelper : MonoBehaviour
{
    private const string m_JsonHeader = "{\"Items\":";

    public static List<T> FromJsonArray<T>(string raw)
    {
        string json = string.Concat(m_JsonHeader, raw, "}");
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJsonArray<T>(List<T> list)
    {
        Wrapper<T> wrapper = new Wrapper<T>
        {
            Items = list
        };
        string result = JsonUtility.ToJson(wrapper);
        return result.Substring(m_JsonHeader.Length, result.Length - 10);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public List<T> Items;
    }
}
