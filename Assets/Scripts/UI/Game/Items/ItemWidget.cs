using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Game.Items;
using KlimLib.SignalBus;
using UnityDI;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ItemWidget : MonoBehaviour
{
    [Dependency] private readonly SignalBus _SignalBus;
    [Dependency] private readonly ItemsService _ItemsService;
    
    public string ItemId;
    public Text AmountText;
    public string AmountStringTemplate;
    public Image PreviewImage;
    protected virtual void Awake()
    {
        ContainerHolder.Container.BuildUp(GetType(), this);
    }

    protected void Start()
    {
        SetupImage();
        UpdateAmount();
        _SignalBus.Subscribe<ItemAmountChangedSignal>(OnItemAmountChangedSignal, this);
    }

    protected virtual void OnDestroy()
    {
        _SignalBus.UnSubscribeFromAll(this);
    }

    protected virtual void OnItemAmountChangedSignal(ItemAmountChangedSignal signal)
    {
        UpdateAmount();
    }
    
    protected virtual void SetupImage()
    {
        if(PreviewImage == null)
            return;
        
    }

    protected virtual void UpdateAmount()
    {
        if (AmountText == null)
            return;
        var itemsCount = _ItemsService.ItemsAmount(ItemId);
        if (string.IsNullOrEmpty(AmountStringTemplate))
            AmountText.text = itemsCount.ToString();
        else
        {
            AmountText.text = AmountStringTemplate + itemsCount.ToString();
        }
    }
}
