using System.Collections.Generic;

[System.Serializable]
public class DataDictionary<TKey, TValue>
{
    public TKey Key;
    public TValue Value;
}

[System.Serializable]
public class DicList<TKey, TValue>
{
    public List<DataDictionary<TKey, TValue>> data;
}


public static class ListDictionaryConverter
{
    public static DicList<TKey, TValue> ToList<TKey, TValue>(Dictionary<TKey, TValue> jsonDicData, bool pretty = false)
    {
        List<DataDictionary<TKey, TValue>> dataList = new List<DataDictionary<TKey, TValue>>();
        DataDictionary<TKey, TValue> dictionaryData;
        foreach (TKey key in jsonDicData.Keys)
        {
            dictionaryData = new DataDictionary<TKey, TValue>();
            dictionaryData.Key = key;
            dictionaryData.Value = jsonDicData[key];
            dataList.Add(dictionaryData);
        }
        DicList<TKey, TValue> arrayJson = new DicList<TKey, TValue>();
        arrayJson.data = dataList;

        return arrayJson;
    }
    //JsonUtility.FromJson<JsonDataArray<TKey, TValue>>(jsonData);
    public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(DicList<TKey, TValue> dataList)
    {
        Dictionary<TKey, TValue> returnDictionary = new Dictionary<TKey, TValue>();
        for (int i = 0; i < dataList.data.Count; i++)
        {
            DataDictionary<TKey, TValue> dictionaryData = dataList.data[i];
            returnDictionary[dictionaryData.Key] = dictionaryData.Value;
        }

        return returnDictionary;
    }
}
