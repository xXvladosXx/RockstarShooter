using System;
using StateMachine.Player;
using UnityEngine;

namespace UI.Core
{
    public abstract class UIElement : MonoBehaviour, IUIElement
    {
        protected UIData UIData;
        
        public event Action<IUIElement> OnElementHide;
        public event Action<IUIElement> OnElementShow;

        public virtual void Init(UIData uiData)
        {
            UIData = uiData;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            OnElementHide?.Invoke(this);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            OnElementShow?.Invoke(this);
        }
    }
}