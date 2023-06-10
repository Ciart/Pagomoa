using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Mineral", menuName = "ScriptableObjects/MineralData", order = 1)]
public class MineralData: ScriptableObject
{
    public string mineralName;
    
    public int tier;

    public Sprite sprite;

    public int price;
}