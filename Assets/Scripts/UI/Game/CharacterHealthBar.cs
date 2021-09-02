using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Character.Health;
using Core.Services;
using KlimLib.SignalBus;
using UnityDI;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHealthBar : MonoBehaviour
{
    [Dependency]
    private readonly SignalBus _signalBus;
    public Slider Slider; 
    private void Awake()
    {
        ContainerHolder.Container.BuildUp(this);
        _signalBus.Subscribe<CharacterSpawnedSignal>(OnCharacterSpawnedSignal, this);
    }

    private void OnCharacterSpawnedSignal(CharacterSpawnedSignal signal)
    {
        signal.Unit.OnDamage += CharacterOnOnDamage;
        SetSliderValue(signal.Unit.Health / signal.Unit.MaxHealth);
    }

    private void CharacterOnOnDamage(IDamageable arg1, Damage arg2)
    {
        SetSliderValue(arg1.Health / arg1.MaxHealth);
    }

    private void SetSliderValue(float val)
    {
        Slider.value = val;
    }

    private void OnDestroy()
    {
        var character = CharacterUnit.Characters.FirstOrDefault();
        if(character != null)
            character.OnDamage -= CharacterOnOnDamage;
        _signalBus.UnSubscribeFromAll(this);
    }
}
