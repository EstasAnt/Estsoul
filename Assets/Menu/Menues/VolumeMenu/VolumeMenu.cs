using Core.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityDI;
using UnityEngine;
using UnityEngine.UI;

public class VolumeMenu : BaseMenu
{
    [Dependency]
    private AudioService audioService;

    public Slider volumeSlider, musicVolumeSlider;

    public void Start()
    {
        ContainerHolder.Container.BuildUp(this);
        volumeSlider.value = audioService.SFXVolumeMultiplier;
        musicVolumeSlider.value = audioService.MusicVolumeMultiplier;
    }

    public void ApplyVolume()
    {
        audioService.SetSFXVolumeMultiplier(volumeSlider.value);
        audioService.SetMusicVolumeMultiplier(musicVolumeSlider.value);
    }
}
