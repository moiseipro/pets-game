using System;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

namespace Effects.Views
{
    public class AudioEffect : MonoBehaviour
    {
        public IObjectPool<AudioEffect> pool;
        public AudioClip[] _randomClips;
        
        private AudioSource _audioSource;
        private Transform _transform;
        
        private void Awake()
        {
            _transform = transform;
            _audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (!_audioSource.isPlaying && (_audioSource.time == 0f)) {
                pool.Release(this);
            }
        }

        public void StartEffect(int index)
        {
            _audioSource.pitch = 1f + Random.Range(0f, 0.15f) + index / 10f;
            _audioSource.clip = _randomClips[Random.Range(0, _randomClips.Length)];
            _audioSource.Play();
        }
        
    }
}