using System;
using EditorAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.GearMind.Test
{
    public class InventoryTestButton : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField, Required]
        private TMP_Text _nameText;

        [SerializeField, Required]
        private TMP_Text _qtyText;

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

        private string _name;
        private int _quantity;

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
    }
}
