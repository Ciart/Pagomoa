using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoneCount : MonoBehaviour
{
    [SerializeField] public int stoneCount;
    [SerializeField] private int _maxCount;
    [SerializeField] private Sprite[] _drillImages;
    [SerializeField] private Image _drillImage;
    [SerializeField] private TextMeshProUGUI _countText;

    public static StoneCount Instance = null;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
    }
    private void Start()
    {
        _countText.text = $"{stoneCount} / {_maxCount}";        
    }
    public void UpCount(int count)
    {
        stoneCount += count;
        _countText.text = $"{stoneCount} / {_maxCount}";
        if (_maxCount / 2 == stoneCount)
            _drillImage.sprite = _drillImages[0];
    }
}
