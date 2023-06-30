using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public List<string[]> talkData;
    public List<Sprite> spritesData = new List<Sprite>();

    // ��ȭ ������ ���� ����  Dialogue Data manual jenerate
    [Space]
    [Header("��ȭ����")]
    [Tooltip("��ȭ �� ��� �ؽ�Ʈ, emotionId �� ������ 1��1 ���� �����ּ���!")]
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
        talkData.Add(new string[] { "1", "��.." });
        talkData.Add(new string[] { "2", "�� ���� �����ΰǵ�.." });
        talkData.Add(new string[] { "3", "�׸���.." });
        talkData.Add(new string[] { "4", "�׸��϶��!" });
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
        //Debug.Log("��ȭ ���۽� �ִϸ��̼� ������ �ʿ��ϴٸ� Trigger StartTalk�߰� �� �ߵ� �����ּ���.");
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
