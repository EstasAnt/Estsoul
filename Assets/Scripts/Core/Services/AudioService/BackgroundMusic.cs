using KlimLib.SignalBus;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityDI;
using UnityEngine;

namespace Core.Audio {
    public class BackgroundMusic : MonoBehaviour {
        public List<TrackPlayInfo> Tracks;
        public float Delay;
        
        [Dependency]
        private readonly AudioService _AudioService;
        [Dependency]
        private readonly SignalBus _SignalBus;

        private AudioEffect _AudioTrack;
        private List<TrackPlayInfo> _TracksOnMatchStart = new List<TrackPlayInfo>();

        private IEnumerator Start() {
            ContainerHolder.Container.BuildUp(this);
            yield return new WaitForSeconds(Delay);
            foreach(var track in Tracks) {
                if (track.PlayMoment == PlayMomentType.Start)
                    Play(track);
                // else if (track.PlayMoment == PlayMomentType.MatchStart)
                //     _TracksOnMatchStart.Add(track);
            }
            // _SignalBus.Subscribe<MatchStartSignal>(OnMatchStartSignal, this);
        }

        // private void OnMatchStartSignal(MatchStartSignal signal) {
        //     _TracksOnMatchStart.ForEach(_ => Play(_));
        // }

        private void Play(TrackPlayInfo track) {
            _AudioTrack = _AudioService.PlayMusic(track.TarackName);
        }

        private void OnDestroy() {
            _SignalBus?.UnSubscribeFromAll(this);
        }
    }

    [Serializable]
    public class TrackPlayInfo {
        public string TarackName;
        public PlayMomentType PlayMoment;
    }

    public enum PlayMomentType {
        Start,
        MatchStart,
    }

}
