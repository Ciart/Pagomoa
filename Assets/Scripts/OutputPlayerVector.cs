using UnityEngine;
using TMPro;

public class OutputPlayerVector : MonoBehaviour
{
    [SerializeField] private Transform _player;

    [SerializeField] private TextMeshProUGUI _tmpGuiText;
    private Vector2 _playerPos;
    void Update()
    {
        SetPlayerVectorOutput();
    }

    public void SetPlayerVectorOutput()
    {
        if (!_player) { return ; }
        _playerPos = _player.position; 

        _tmpGuiText.text = string.Format("X : {0:0000.00}   Y : {1:0000.00}", _playerPos.x, _playerPos.y);
    }
}
