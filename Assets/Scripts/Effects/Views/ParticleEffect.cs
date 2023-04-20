using System;
using UnityEngine;
using UnityEngine.Pool;

namespace Effects.Views
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleEffect : MonoBehaviour
    {
        public IObjectPool<ParticleEffect> pool;
        
        private ParticleSystem _particleSystem;
        private Transform _transform;
        

        private void Awake()
        {
            _transform = transform;
            _particleSystem = GetComponent<ParticleSystem>();
            _particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        private void OnParticleSystemStopped()
        {
            pool.Release(this);
        }

        public void StartEffect(Vector3 position)
        {
            _transform.position = position;
            _particleSystem.Play();
        }
    }
}