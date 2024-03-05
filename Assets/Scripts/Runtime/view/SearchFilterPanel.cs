using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Nomtec.View
{
    public class SearchFilterPanel : MonoBehaviour
    {
        [SerializeField] TMP_InputField _inputField;
        TMP_InputField InputField { get { if(!_inputField) _inputField = GetComponentInChildren<TMP_InputField>(); return _inputField; } }

        [SerializeField] ItemMenu _itemMenu;
        [SerializeField] ItemMenu ItemMenu { get { if (!_itemMenu) _itemMenu = GetComponentInChildren<ItemMenu>(); return _itemMenu; } }

        private void Start()
        {
            if (InputField) InputField.onValueChanged.AddListener(HandleValueChanged);
        }

        private void HandleValueChanged(string value)
        {
            ItemMenu.Filter(value);
        }
    }
}
