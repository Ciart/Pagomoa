using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ciart.Pagomoa
{
    public class ChildButton : Button
    {
        public void SetButtonByParent(string state, bool instant)
        {
            if (!gameObject.activeInHierarchy)
                return;

            Color tintColor;
            Sprite transitionSprite;
            string triggerName;

            switch (state)
            {
                case "Normal":
                    tintColor = colors.normalColor;
                    transitionSprite = null;
                    triggerName = animationTriggers.normalTrigger;
                    break;
                case "Highlighted":
                    tintColor = colors.highlightedColor;
                    transitionSprite = spriteState.highlightedSprite;
                    triggerName = animationTriggers.highlightedTrigger;
                    break;
                case "Pressed":
                    tintColor = colors.pressedColor;
                    transitionSprite = spriteState.pressedSprite;
                    triggerName = animationTriggers.pressedTrigger;
                    break;
                case "Selected":
                    tintColor = colors.selectedColor;
                    transitionSprite = spriteState.selectedSprite;
                    triggerName = animationTriggers.selectedTrigger;
                    break;
                case "Disabled":
                    tintColor = colors.disabledColor;
                    transitionSprite = spriteState.disabledSprite;
                    triggerName = animationTriggers.disabledTrigger;
                    break;
                default:
                    tintColor = Color.black;
                    transitionSprite = null;
                    triggerName = string.Empty;
                    break;
            }

            switch (transition)
            {
                case Transition.ColorTint:
                    StartColorTween(tintColor, instant);
                    break;
                case Transition.SpriteSwap:
                    DoSpriteSwap(transitionSprite);
                    break;
                case Transition.Animation:
                    TriggerAnimation(triggerName);
                    break;
            }
        }
        void StartColorTween(Color targetColor, bool instant)
        {
            if (targetGraphic == null)
                return;

            targetGraphic.CrossFadeColor(targetColor, instant ? 0f : colors.fadeDuration, true, true);
        }

        void DoSpriteSwap(Sprite newSprite)
        {
            if (image == null)
                return;

            image.overrideSprite = newSprite;
        }

        void TriggerAnimation(string triggername)
        {
            //#if PACKAGE_ANIMATION
            //            if (transition != Transition.Animation || animator == null || !animator.isActiveAndEnabled || !animator.hasBoundPlayables || string.IsNullOrEmpty(triggername))
            //                return;

            //            animator.ResetTrigger(animationTriggers.normalTrigger);
            //            animator.ResetTrigger(animationTriggers.highlightedTrigger);
            //            animator.ResetTrigger(animationTriggers.pressedTrigger);
            //            animator.ResetTrigger(animationTriggers.selectedTrigger);
            //            animator.ResetTrigger(animationTriggers.disabledTrigger);

            //            animator.SetTrigger(triggername);
            //#endif
        }
    }
}
