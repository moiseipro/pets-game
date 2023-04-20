using System;
using System.Collections.Generic;
using Effects.Views;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Ability.Views
{
    public class LineView : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _radiusMarker;
        [SerializeField] private float _maxLength = 0.1f;
        [SerializeField] private AudioEffectPool _comboEffectPool;
        [SerializeField] private AudioSource _endComboEffect;

        private LineRenderer _lineRenderer;
        

        private List<Transform> _moveTransforms = new List<Transform>();
        
        private Transform _lastTransform;

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
        }

        public bool AddPoint(Transform addTransform)
        {
            if (_lastTransform && (addTransform.position-_lastTransform.position).magnitude > _maxLength) return false;
            if (_lastTransform)
            {
                //Debug.Log((addTransform.position-_lastTransform.position).magnitude);
                AudioEffect audioEffect = _comboEffectPool.Pool.Get();
                audioEffect.StartEffect(_moveTransforms.Count);
            }
            
            var newShape = _radiusMarker.shape;
            newShape.radius = _maxLength - 0.2f;
            _radiusMarker.Play();

            _moveTransforms.Add(addTransform);
            _lineRenderer.positionCount = _moveTransforms.Count;

            _lastTransform = addTransform;
            return true;
        }

        private void Update()
        {
            if(_moveTransforms.Count == 0) return;
            for (int i = 0; i < _moveTransforms.Count; i++)
            {
                _lineRenderer.SetPosition(i, _moveTransforms[i].position);
            }

            _radiusMarker.transform.position = _moveTransforms[^1].position;
        }

        public void ResetPoints()
        {
            if (_moveTransforms.Count > 1)
            {
                _endComboEffect.pitch = 1f + Random.Range(0f, 1f) + _moveTransforms.Count / 5f;
                _endComboEffect.Play();
            }
            _moveTransforms.Clear();
            _lineRenderer.positionCount = 0;
            _lastTransform = null;
            _radiusMarker.Stop();
            
            
        }
    }
}