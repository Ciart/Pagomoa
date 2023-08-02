using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class DataManager : MonoBehaviour
{
    static GameObject container;
    static DataManager instance;
    public static DataManager Instance
    {
        get
        {
            if (!instance)
            {
                container = new GameObject();
                container.name = "Data Manager";
                instance = container.AddComponent(typeof(DataManager)) as DataManager;
                DontDestroyOnLoad(container);
            }
            return instance;
        }
    }

    // 게임 데이터 파일 이름 설정 (이름.json)
    string GameDataFileName = "GameData.json";

    public GameData data = new GameData();

    public void LoadGameData()
    {
        string filePath = Application.persistentDataPath + "/" + GameDataFileName;
        Debug.Log($"{Application.persistentDataPath} 경로의 데이터를 로드합니다");

        if (File.Exists(filePath))
        {
            string FromJsonData = File.ReadAllText(filePath);
            try
            {
                data = JsonUtility.FromJson<GameData>(FromJsonData);
            }
            catch (System.Exception e)
            {
                Debug.Log("불러오기 실패: " + e);
            }
        }

    }
    public void SaveGameData()
    {
        string filePath = Application.persistentDataPath + "/" + GameDataFileName;
        string ToJasonData = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, ToJasonData);
        Debug.Log("저장 완료");
    }
}
