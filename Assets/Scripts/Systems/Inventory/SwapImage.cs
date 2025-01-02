using Ciart.Pagomoa.Systems;
using Ciart.Pagomoa.Systems.Inventory;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class SwapImage : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] private Image _buttonimage;
    private void Start() => _buttonimage = GetComponent<Image>();
    
    [Header("버튼 이미지 스프라이트")]
    [SerializeField] private Sprite _defaultButtonSprite;
    [SerializeField] private Sprite _hoverButtonSprite;
    [SerializeField] private Sprite _pressedButtonSprite;
    
    [Header("버튼 경계선 스프라이트")]
    [SerializeField] private Sprite _defaultLineSprite;
    [SerializeField] private Sprite _hoverPressedLineSprite;
    
    [Header("버튼 마지막 경계선 이미지 스프라이트")]
    [SerializeField] private Sprite _defaultUnderLineSprite;
    [SerializeField] private Sprite _hoverPressedUnderLineSprite;
    
    public Sprite GetDefaultLine() { return _defaultLineSprite; }
    public Sprite GetDefaultUnderLine() { return _defaultUnderLineSprite; }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        var menu = UIManager.instance.bookUI.GetRightClickMenu().menu;
        
        for (int i = 0; i < menu.Count; i++)
        {
            if (eventData.pointerEnter.gameObject == menu[i].gameObject)
            {
                HoverOn(i);
            }
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        var rightClickMenu = UIManager.instance.bookUI.GetRightClickMenu();
        
        for (int i = 0; i < rightClickMenu.menu.Count; i++)
        {
            if (eventData.pointerEnter.gameObject == rightClickMenu.menu[i].gameObject)
            {
                HoverOff(i);
            }
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        var menu = UIManager.instance.bookUI.GetRightClickMenu().menu;
        
        for (int i = 0; i < menu.Count; i++)
        {
            if (eventData.pointerEnter.gameObject == menu[i].gameObject)
            {
                PressedMenu(i);
            }
        }
    }
    private void HoverOn(int index)
    {
        var rightClickMenu = UIManager.instance.bookUI.GetRightClickMenu();
        
        _buttonimage.sprite = _hoverButtonSprite;
        if(index == 0)
        {
            rightClickMenu.lines[index].sprite = _hoverPressedUnderLineSprite;
            rightClickMenu.lines[index + 1].sprite = _hoverPressedLineSprite;
        }
        else if (index > 0 && index <rightClickMenu.menu.Count - 1)
        {
            rightClickMenu.lines[index].sprite = _hoverPressedLineSprite;
            rightClickMenu.lines[index + 1].sprite = _hoverPressedLineSprite;
        }
        else if (index == rightClickMenu.menu.Count -1)
        {
            rightClickMenu.lines[index].sprite = _hoverPressedLineSprite;
            rightClickMenu.underLine.sprite = _hoverPressedUnderLineSprite;
        }
    }
    private void HoverOff(int index)
    {
        var rightClickMenu = UIManager.instance.bookUI.GetRightClickMenu();
        
        _buttonimage.sprite = _defaultButtonSprite;
        if (index == 0)
        {
            rightClickMenu.lines[index].sprite = _defaultUnderLineSprite;
            rightClickMenu.lines[index + 1].sprite = _defaultLineSprite;
        }
        else if (index > 0 && index < rightClickMenu.menu.Count - 1)
        {
            rightClickMenu.lines[index].sprite = _defaultLineSprite;
            rightClickMenu.lines[index + 1].sprite = _defaultLineSprite;
        }
        else if (index == rightClickMenu.menu.Count - 1)
        {
            rightClickMenu.lines[index].sprite = _defaultLineSprite;
            rightClickMenu.underLine.sprite = _defaultUnderLineSprite;
        }
    }
    private void PressedMenu(int index)
    {
        var rightClickMenu = UIManager.instance.bookUI.GetRightClickMenu();
        
        _buttonimage.sprite = _pressedButtonSprite;
        if (index == 0)
        {
            rightClickMenu.lines[index].sprite = _hoverPressedUnderLineSprite;
            rightClickMenu.lines[index + 1].sprite = _hoverPressedLineSprite;
        }
        else if (index > 0 && index < rightClickMenu.menu.Count - 1)
        {
            rightClickMenu.lines[index].sprite = _hoverPressedLineSprite;
            rightClickMenu.lines[index + 1].sprite = _hoverPressedLineSprite;
        }
        else if (index == rightClickMenu.menu.Count - 1)
        {
            rightClickMenu.lines[index].sprite = _hoverPressedLineSprite;
            rightClickMenu.underLine.sprite = _hoverPressedUnderLineSprite;
        }
    }
}
