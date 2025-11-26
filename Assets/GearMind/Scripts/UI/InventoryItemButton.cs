using System;
using EditorAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.GearMind.UI
{
    public class InventoryItemButton : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField, Required]
        private TMP_Text _nameText;

        [SerializeField, Required]
        private TMP_Text _qtyText;

        [SerializeField, Required]
        private Image _iconImage;

        [SerializeField]
        private Color _interactableColor = Color.black;

        [SerializeField]
        private Color _nonInteractableColor = new Color(0.5f, 0.5f, 0.5f, 1f);

        public event Action OnPointerDownEvent;

        public string Name
        {
            get => _name;
            set => SetName(value);
        }
        public int Quantity
        {
            get => _quantity;
            set => SetQuantity(value);
        }

        public Sprite Icon
        {
            get => _icon;
            set => SetIcon(value);
        }

        private string _name;
        private int _quantity;
        private Sprite _icon;
        private bool _interactable = true;

        public void OnPointerDown(PointerEventData eventData) => OnPointerDownEvent?.Invoke();

        private void SetName(string name)
        {
            _name = name;
            _nameText.text = name;
        }

        private void SetQuantity(int quantity)
        {
            _quantity = quantity;
            _qtyText.text = quantity.ToString();
        }

        private void SetIcon(Sprite icon)
        {
            _icon = icon;
            _iconImage.sprite = icon;

            UpdateItemName();
        }
        private void UpdateItemName()
        {
            _nameText.gameObject.SetActive(_icon == null);
        }

        public bool Interactable
        {
            get => _interactable;
            set
            {
                _interactable = value;
                Color color = _interactable ? _interactableColor : _nonInteractableColor;
                _nameText.color = color;
                _qtyText.color = color;
            }
        }
    }
}
