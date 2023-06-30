using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public List<string[]> talkData;
    public List<Sprite> spritesData = new List<Sprite>();

    // 대화 데이터 수동 생성  Dialogue Data manual jenerate
    [Space]
    [Header("대화생성")]
    [Tooltip("대화 시 띄울 텍스트, emotionId 와 수량을 1대1 대응 시켜주세요!")]
    public List<string> talk = new List<string>();
    public List<int> emotionId = new List<int>();

    private void Awake()
    {
        talkData = new List<string[]>();       
        //GenerateDataWithCode();
        GenerateData_Manual();

    }
    /*
    void GenerateDataWithCode()
    {
        talkData.Add(new string[] { "1", "왜.." });
        talkData.Add(new string[] { "2", "또 뭐가 문제인건데.." });
        talkData.Add(new string[] { "3", "그만해.." });
        talkData.Add(new string[] { "4", "그만하라고!" });
    }
    */
    void GenerateData_Manual()
    {
        for (int i = 0; i < talk.Count; i++)
            talkData.Add(new string[] { emotionId[i].ToString(), talk[i] });
    }
    public void StartTalking()
    {
        GetComponent<Animator>().SetTrigger("StartTalk");
        //Debug.Log("대화 시작시 애니메이션 진행이 필요하다면 Trigger StartTalk추가 후 발동 시켜주세요.");
        if (GetComponent<AutoChat>())
            GetComponent<AutoChat>().StopChat();
    }
    public void StopTalking()
    {
        if (GetComponent<AutoChat>())
            GetComponent<AutoChat>().StartChatReservation();
    }
    public string GetTalk(int talkIndex)
    {
        if (talkIndex >= talkData.Count)
            return null;
        return talkData[talkIndex][1];
    }
    public Sprite GetSprite(int talkIndex)
    {
        if (talkIndex >= talkData.Count)
            return null;
        return spritesData[int.Parse(talkData[talkIndex][0])];
    }

}
