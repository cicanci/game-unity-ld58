using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _inventoryLabel;

    private readonly string _inventoryTemplate = "Cards\nFire: {0}\nIce: {1}";
    private readonly Dictionary<CardType, int> _inventory = new()
    {
        { CardType.Fire, 0 },
        { CardType.Ice, 0 }
    };

    private void Awake()
    {
        UpdateUI();
    }

    private void OnEnable()
    {
        MessageQueue.Instance.AddListener<CardPickedUpMessage>(OnCardPickedUp);
    }

    private void OnDisable()
    {
        MessageQueue.Instance.RemoveListener<CardPickedUpMessage>(OnCardPickedUp);
    }

    private void OnCardPickedUp(CardPickedUpMessage message)
    {
        Debug.Log($"CardPickedUpMessage: {message.Quantity}x {message.Type}");

        _inventory[message.Type] += message.Quantity;

        UpdateUI();
    }

    private void UpdateUI()
    {
        _inventoryLabel.text = string.Format(_inventoryTemplate, 
            _inventory[CardType.Fire], _inventory[CardType.Ice]);
    }
}
