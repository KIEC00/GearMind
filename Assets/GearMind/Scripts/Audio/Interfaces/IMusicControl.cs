namespace Assets.GearMind.Audio
{
    public interface IMusicControl
    {
        public bool IsPlaying { get; }
        public bool IsPaused { get; }
        public IMusicQueue MusicQueue { get; set; }
        public float DelayBetweenClips { get; set; }
        public void Play();
        public void Pause();
        public void UnPause();
        public void Stop();
    }
}
