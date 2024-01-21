using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SwapImage : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        for (int i = 0; i < RightClickMenu.Instance.menus.Count; i++)
        {
            if (eventData.pointerEnter.gameObject == RightClickMenu.Instance.menus[i])
            {
                HoverOn(i);
            }
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        for (int i = 0; i < RightClickMenu.Instance.menus.Count; i++)
        {
            if (eventData.pointerEnter.gameObject == RightClickMenu.Instance.menus[i])
            {
                HoverOff(i);
            }
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        for (int i = 0; i < RightClickMenu.Instance.menus.Count; i++)
        {
            if (eventData.pointerEnter.gameObject == RightClickMenu.Instance.menus[i])
            {
                PressedMenu(i);
            }
        }
    }
    private void HoverOn(int index)
    {
        if(index == 0)
        {
            this.GetComponent<Image>().sprite = RightClickMenu.Instance.hoverMenuImages[2];
            RightClickMenu.Instance.lines[index].GetComponent<Image>().sprite = RightClickMenu.Instance.hoverMenuImages[1];
            RightClickMenu.Instance.lines[index + 1].GetComponent<Image>().sprite = RightClickMenu.Instance.hoverMenuImages[0];
        }
        else if (index > 0 && index < RightClickMenu.Instance.menus.Count - 1)
        {
            this.GetComponent<Image>().sprite = RightClickMenu.Instance.hoverMenuImages[2];
            RightClickMenu.Instance.lines[index].GetComponent<Image>().sprite = RightClickMenu.Instance.hoverMenuImages[0];
            RightClickMenu.Instance.lines[index + 1].GetComponent<Image>().sprite = RightClickMenu.Instance.hoverMenuImages[0];
        }
        else if (index == RightClickMenu.Instance.menus.Count -1)
        {
            this.GetComponent<Image>().sprite = RightClickMenu.Instance.hoverMenuImages[2];
            RightClickMenu.Instance.lines[index].GetComponent<Image>().sprite = RightClickMenu.Instance.hoverMenuImages[0];
            RightClickMenu.Instance.underLines[0].GetComponent<Image>().sprite = RightClickMenu.Instance.hoverMenuImages[1];
        }
    }
    private void HoverOff(int index)
    {
        if (index == 0)
        {
            this.GetComponent<Image>().sprite = RightClickMenu.Instance.basicMenuImages[2];
            RightClickMenu.Instance.lines[index].GetComponent<Image>().sprite = RightClickMenu.Instance.basicMenuImages[1];
            RightClickMenu.Instance.lines[index + 1].GetComponent<Image>().sprite = RightClickMenu.Instance.basicMenuImages[0];
        }
        else if (index > 0 && index < RightClickMenu.Instance.menus.Count - 1)
        {
            this.GetComponent<Image>().sprite = RightClickMenu.Instance.basicMenuImages[2];
            RightClickMenu.Instance.lines[index].GetComponent<Image>().sprite = RightClickMenu.Instance.basicMenuImages[0];
            RightClickMenu.Instance.lines[index + 1].GetComponent<Image>().sprite = RightClickMenu.Instance.basicMenuImages[0];
        }
        else if (index == RightClickMenu.Instance.menus.Count - 1)
        {
            this.GetComponent<Image>().sprite = RightClickMenu.Instance.basicMenuImages[2];
            RightClickMenu.Instance.lines[index].GetComponent<Image>().sprite = RightClickMenu.Instance.basicMenuImages[0];
            RightClickMenu.Instance.underLines[0].GetComponent<Image>().sprite = RightClickMenu.Instance.basicMenuImages[1];
        }
    }
    private void PressedMenu(int index)
    {
        if (index == 0)
        {
            this.GetComponent<Image>().sprite = RightClickMenu.Instance.pressedMenuImages[2];
            RightClickMenu.Instance.lines[index].GetComponent<Image>().sprite = RightClickMenu.Instance.pressedMenuImages[1];
            RightClickMenu.Instance.lines[index + 1].GetComponent<Image>().sprite = RightClickMenu.Instance.pressedMenuImages[0];
        }
        else if (index > 0 && index < RightClickMenu.Instance.menus.Count - 1)
        {
            this.GetComponent<Image>().sprite = RightClickMenu.Instance.pressedMenuImages[2];
            RightClickMenu.Instance.lines[index].GetComponent<Image>().sprite = RightClickMenu.Instance.pressedMenuImages[0];
            RightClickMenu.Instance.lines[index + 1].GetComponent<Image>().sprite = RightClickMenu.Instance.pressedMenuImages[0];
        }
        else if (index == RightClickMenu.Instance.menus.Count - 1)
        {
            this.GetComponent<Image>().sprite = RightClickMenu.Instance.pressedMenuImages[2];
            RightClickMenu.Instance.lines[index].GetComponent<Image>().sprite = RightClickMenu.Instance.pressedMenuImages[0];
            RightClickMenu.Instance.underLines[0].GetComponent<Image>().sprite = RightClickMenu.Instance.pressedMenuImages[1];
        }
    }
}
