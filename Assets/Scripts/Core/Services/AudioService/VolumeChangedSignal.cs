namespace Core.Audio
{
    public struct VolumeChangedSignal {
        public AudioGroup Group;
        public float Volume;

        public VolumeChangedSignal(AudioGroup group, float volume) {
            this.Group = group;
            this.Volume = volume;
        }
    }
}