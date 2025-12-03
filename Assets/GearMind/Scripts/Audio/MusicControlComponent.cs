using System.Collections;
using System.Collections.Generic;
using EditorAttributes;
using UnityEngine;
using VContainer;

namespace Assets.GearMind.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class MusicControlComponent : MonoBehaviour, IMusicControl
    {
        public bool IsPlaying => _playingCoroutine != null;
        public bool IsPaused => _queueEnumerator != null && !IsPlaying;

        public IMusicQueue MusicQueue
        {
            get => _musicQueue;
            set => ChangeQueue(value, IsPlaying);
        }

        public float DelayBetweenClips
        {
            get => _delayBetweenClips;
            set => _delayBetweenClips = value;
        }

        [Inject]
        private IMusicQueue _musicQueue;

        [SerializeField, Required]
        private AudioSource _audioSource;

        [SerializeField, Min(0f)]
        private float _delayBetweenClips = 0.5f;

        private Coroutine _playingCoroutine = null;
        private IEnumerator<AudioClip> _queueEnumerator = null;

        private bool WasStarted => _queueEnumerator != null;

        [Button]
        public void Play()
        {
            Stop();
            InitEnumerator();
            _audioSource.clip = _queueEnumerator.Current;
            _audioSource.Play();
            StartPlayingCoroutine();
        }

        [Button]
        public void Pause()
        {
            if (!IsPlaying)
                return;
            StopPlayingCoroutine();
            _audioSource.Pause();
        }

        [Button]
        public void UnPause()
        {
            if (IsPlaying || !WasStarted)
                return;
            _audioSource.UnPause();
            StartPlayingCoroutine();
        }

        [Button]
        public void Stop()
        {
            if (!WasStarted)
                return;
            StopPlayingCoroutine();
            _audioSource.Stop();
            _audioSource.clip = null;
            DisposeEnumerator();
        }

        private IEnumerator PlayingCoroutine()
        {
            while (true)
            {
                yield return new WaitWhile(() => _audioSource.isPlaying || !Application.isFocused);
                yield return new WaitForSecondsRealtime(_delayBetweenClips);
                _queueEnumerator.MoveNext();
                _audioSource.clip = _queueEnumerator.Current;
                _audioSource.Play();
            }
        }

        private void InitEnumerator()
        {
            _queueEnumerator = _musicQueue.ClipsCycle.GetEnumerator();
            _queueEnumerator.MoveNext();
        }

        private void DisposeEnumerator()
        {
            _queueEnumerator?.Dispose();
            _queueEnumerator = null;
        }

        private void StartPlayingCoroutine()
        {
            _playingCoroutine = StartCoroutine(PlayingCoroutine());
        }

        private void StopPlayingCoroutine()
        {
            if (_playingCoroutine == null)
                return;
            StopCoroutine(_playingCoroutine);
            _playingCoroutine = null;
        }

        private void ChangeQueue(IMusicQueue musicQueue, bool autoPlay)
        {
            _musicQueue = musicQueue;
            if (autoPlay)
                Play();
            else
                Stop();
        }

        private void Awake()
        {
            _audioSource.ignoreListenerPause = true;
            _audioSource.ignoreListenerVolume = true;
        }

        private void OnDisable() => Stop();

        private void OnValidate()
        {
            if (!_audioSource)
                _audioSource = GetComponent<AudioSource>();
            if (!_audioSource)
                Debug.LogError("Can not find AudioSource", this);
        }
    }
}
