using System;
using UnityEngine;

namespace Game.Views
{
    public class CameraView : MonoBehaviour
    {
        [SerializeField] private Vector3 _offset;
        private Transform _currentTarget;

        private Transform _transform;

        private void Awake()
        {
            _transform = transform;
        }

        private void Update()
        {
            if (_currentTarget == null) return;
            _transform.position = Vector3.Lerp(_transform.position, _currentTarget.position + _offset, Time.deltaTime * 5f);
        }

        public void SetTarget(Transform transform)
        {
            _currentTarget = transform;
        }
    }
}