using System;
using StateMachine.Player;

namespace UI.Core
{
    public interface IUIElement
    {
        public event Action<IUIElement>  OnElementHide;
        public event Action<IUIElement>  OnElementShow;

        void Init(UIData player);
        void Hide();
        void Show();
    }
}