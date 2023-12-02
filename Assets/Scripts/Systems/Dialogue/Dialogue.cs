using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "New Dialogue/Dialogue")]
[Tooltip("��ȭ �� ��� �ؽ�Ʈ, emotionId �� ������ 1��1 ���� �����ּ���!")]
public class Dialogue : ScriptableObject
{
    public int id;
    public List<string> talkerName = new List<string>();
    public List<string> talk = new List<string>();
    public List<Sprite> sprite = new List<Sprite>();
}
