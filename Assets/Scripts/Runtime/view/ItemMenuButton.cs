using Nomtec.Data;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Nomtec.View
{
    public class ItemMenuButton : MonoBehaviour, ISpawnableButton
    {
        [SerializeField] Button _button;
        Button Button { get { if (!_button) _button = GetComponentInChildren<Button>(); return _button; } }

        [SerializeField] Image _thumbnail;
        Image Thumbnail { get { if (!_thumbnail) _thumbnail = GetComponentInChildren<Image>(); return _thumbnail; } }

        [SerializeField] TMP_Text _textField;
        TMP_Text TextField { get { if (!_textField) _textField = GetComponentInChildren<TMP_Text>(); return _textField; } }

        public SpawnableItemData Data { get; private set; }


        private void OnEnable()
        {
            if (Button) Button.onClick.AddListener(HandleClick);
        }

        private void OnDisable()
        {
            if (Button) Button.onClick.RemoveAllListeners();
        }

        public bool Initialize(SpawnableItemData data)
        {
            try
            {
                TextField.text = data.title;
                Thumbnail.sprite = data.thumbnail;
                Data = data;
                return true;
            }
            catch (Exception e) { Debug.LogException(e); return false; }

        }

        private void HandleClick()
        {
            GameEventsManager.InvokeButtonSelected(this);
        }

        public void SetInteractive(bool status)
        {
            if (Button) Button.interactable = status;
        }
    }
}
