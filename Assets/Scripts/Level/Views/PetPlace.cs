using Pets.Models;
using Pets.Views;
using UnityEngine;

namespace Level.Views
{
    public class PetPlace : MonoBehaviour
    {
        [SerializeField] protected PetView _petViewPrefab;
        [SerializeField] private Vector2 _offset;
        private PetView _currentPetView;
        protected PetModel _petModel;
        private Transform _transform;
        private SpriteRenderer _spriteRenderer;
        
        
        private void Awake()
        {
            _transform = transform;
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _currentPetView = Instantiate(_petViewPrefab, _transform.position + (Vector3)_offset, Quaternion.identity, _transform);
        }
        
        public virtual void SetPetModel(PetModel petModel)
        {
            _petModel = petModel;
            _currentPetView.SetPetModel(petModel);
        }

        public void SetLocalPosition(Vector3 newPosition)
        {
            _transform.localPosition = newPosition;
        }
        
        public Vector3 GetFlyPosition()
        {
            return _transform.position + new Vector3(0, _spriteRenderer.bounds.extents.y, 0);
        }
    }
}