using System;
using System.Collections.Generic;
using System.Linq;
using StateMachine.Player;
using UnityEngine;

namespace UI.Core
{
    public class UIController : MonoBehaviour
    {
        [field: SerializeField] public Player Player { get; private set; }

        protected UIData UIData;
        protected List<StaticUIElement> StaticUIElements = new List<StaticUIElement>();
        protected List<PopupUIElement> PopupUIElements  = new List<PopupUIElement>();
        private void Start()
        {
            StaticUIElements = GetComponentsInChildren<StaticUIElement>().ToList();
            PopupUIElements = GetComponentsInChildren<PopupUIElement>().ToList();

            UIData = new UIData
            {   
                Player = Player
            };
            
            foreach (var popupUIElement in PopupUIElements)
            {
                popupUIElement.Init(UIData);
            }

            foreach (var staticUIElement in StaticUIElements)
            {
                staticUIElement.Init(UIData);
            }
        }

    }
}