using Core.Services;
using KlimLib.ResourceLoader;
using KlimLib.SignalBus;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityDI;
using UnityEngine;

namespace Core.Audio {
    public class AudioService : ILoadableService {
        [Dependency]
        private readonly IResourceLoaderService _ResourceLoaderService;
        [Dependency]
        private readonly SignalBus _SignalBus;

        public AudioPool AudioPool { get; private set; }

        public Transform PoolTransform => AudioPool.transform;

        public float SFXMaxDistanceMultiplier { get; private set; } = 1f;
        public float SFXVolumeMultiplier { get; private set; } = 1f;
        public float MusicVolumeMultiplier { get; private set; } = 1f;

        public AudioEffect PlayMusic(string trackName) {
            return PlaySound2D(trackName, true, true);
        }

        public AudioEffect PlaySound2D(string trackName, bool forceSingle, bool rise) {
            return PlaySound3D(trackName, forceSingle, rise, Vector3.zero);
        }

        public AudioEffect PlaySound3D(string trackName, bool forceSingle, bool rise, Vector3 position) {
            if (!CanPlayTrack(trackName))
                return null;
            var track = AudioPool.GetTrack(trackName, forceSingle);
            track.transform.position = position;
            track.Play(rise);
            return track;
        }
        
        public void PlayRandomSound(List<string> sounds, bool forceSingle, bool rise, Vector3 position) {
            if (sounds == null || sounds.Count == 0)
                return;
            var randIndex = UnityEngine.Random.Range(0, sounds.Count);
            PlaySound3D(sounds[randIndex], forceSingle, rise, position);
        }

        public AudioEffect GetTrack(string trackName) {
            var track = _ResourceLoaderService.LoadResource<AudioEffect>("Prefabs/AudioTracks/" + trackName);
            if (track == null)
                Debug.LogError($"there is no Prefabs/AudioTracks/{trackName} track");
            return _ResourceLoaderService.LoadResource<AudioEffect>("Prefabs/AudioTracks/" + trackName);
        }

        public bool CanPlayTrack(string trackName)
        {
            var track = GetTrack(trackName);
            if (track == null)
                return false;
            var audioGroup = track.AudioGroup;
            switch (audioGroup) {
                case AudioGroup.SFX:
                    return true;
                case AudioGroup.Music:
                    return true;
            }
            return true;
        }
        
        public void SetSFXVolumeMultiplier(float mult) {
            SFXVolumeMultiplier = mult;
            _SignalBus.FireSignal(new VolumeChangedSignal(AudioGroup.Music, SFXVolumeMultiplier));
        }

        public void ResetSFXVolumeMultiplier()
        {
            SetSFXVolumeMultiplier(1f);
        }

        public void SetMusicVolumeMultiplier(float mult) {
            MusicVolumeMultiplier = mult;
            _SignalBus.FireSignal(new VolumeChangedSignal(AudioGroup.Music, MusicVolumeMultiplier));
        }

        public void ResetMusicVolumeMultiplier()
        {
            SetMusicVolumeMultiplier(1f);
        }

        void ILoadableService.Load() {
            var audioPoolGameObject = new GameObject(typeof(AudioPool).Name, typeof(AudioPool)); //todo: move to task
            Object.DontDestroyOnLoad(audioPoolGameObject);
            AudioPool = audioPoolGameObject.GetComponent<AudioPool>();
        }
    }
}
