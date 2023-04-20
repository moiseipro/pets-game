using System;
using System.Collections;
using Ability.Data;
using DG.Tweening;
using Effects.Views;
using Game.Models;
using Level.Views;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Pool;
using Random = UnityEngine.Random;
using Sequence = DG.Tweening.Sequence;

namespace Ability.Views
{
    
    public class FoodView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerClickHandler
    {
        public IObjectPool<FoodView> pool;
        private FoodType _initialFoodType;
        [SerializeField] private FoodType _foodType;
        [SerializeField] private Sprite[] _foodSprites;
        public FoodType foodType => _foodType;
        private Camera _camera;
        private Transform _transform;
        private PolygonCollider2D _polygonCollider2D;
        private Rigidbody2D _rigidbody2D;
        private SpriteRenderer _spriteRenderer;

        private LineView _lineView;
        private ComboCheckerModel _comboCheckerModel;
        private ParticleEffectPool _deathEffectPool;
        private PetFlask _petFlask;
        private GameInfo _gameInfo;
        private bool _isSelect;
        private bool _isSpoiling;
        public bool isSpoiling => _isSpoiling;
        private float _spoilingTime;
        private float _spoilingTimer;
        
        private Sequence _sequence;
        
        public void Awake()
        {
            _camera = Camera.main;
            _transform = transform;
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        public void Initialize(LineView lineView, ComboCheckerModel comboCheckerModel, ParticleEffectPool deathEffectPool, PetFlask petFlask, GameInfo gameInfo)
        {
            _lineView = lineView;
            _comboCheckerModel = comboCheckerModel;
            _deathEffectPool = deathEffectPool;
            _petFlask = petFlask;
            _gameInfo = gameInfo;
        }

        public void Respawn(Vector3 newPosition, float spoilingTime, FoodType foodType)
        {
            _transform.position = newPosition;
            _spoilingTime = spoilingTime;
            _foodType = foodType;
            _initialFoodType = _foodType;
            _spriteRenderer.sprite = _foodSprites[(int)_foodType];
            _polygonCollider2D = _spriteRenderer.AddComponent<PolygonCollider2D>();
            _rigidbody2D.AddForce(_petFlask.GetFlyPosition()-_transform.position, ForceMode2D.Impulse);
            _spriteRenderer.sortingOrder += 1;
            if (!(_foodType is FoodType.Bomb or FoodType.Magnet or FoodType.StarStick))
            {
                _spoilingTimer = Time.time + _spoilingTime;
            }

            _gameInfo.OnLoseGame += Death;
            _gameInfo.OnWinGame += Death;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            Debug.Log("Start Draw!");
            Transform beginDragTransform = eventData.pointerDrag.transform;
            
            if (!_comboCheckerModel.isStarted && !_isSelect && _lineView.AddPoint(beginDragTransform))
            {
                _isSelect = true;
                _comboCheckerModel.StartCombo(_foodType);
                _comboCheckerModel.AddFood(this);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Debug.Log("Stop Draw!");
            _lineView.ResetPoints();
            _comboCheckerModel.FinishLine();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("Entered!");
            Transform enterTransform = eventData.pointerEnter.transform;
            if (_comboCheckerModel.isStarted && _comboCheckerModel.CheckFoodType(_foodType) && _isSelect == false && _lineView.AddPoint(enterTransform))
            {
                _isSelect = true;
                _comboCheckerModel.AddFood(this);
            }
        }

        public void Unselect()
        {
            _isSelect = false;
        }

        public void TakeItem(int index)
        {
            _spoilingTimer = 0;
            _rigidbody2D.simulated = false;
            _spriteRenderer.sortingOrder += 1;
            Vector2 newPosition = _camera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
            newPosition.y -= (index / 5) / 3f + 1f;
            newPosition.x -= (index - (index / 5) * 5) / 3f + 1f;
            Debug.Log(newPosition);
            _sequence = DOTween.Sequence();
            _sequence
                .Append(_transform.DOScale(_transform.localScale * 1.5f, 0.2f))
                .Insert(0f, _transform.DORotate(Vector3.zero, 0.2f))
                .Append(_transform.DOMove(newPosition, Random.Range(1f, 2f)))
                .Insert(0.2f, _transform.DOScale(Vector3.one * 0.5f, _sequence.Duration()));
        }

        public void BombItem(int index)
        {
            _spoilingTimer = 0;
            _rigidbody2D.simulated = false;
            _spriteRenderer.sortingOrder += 1;
            _sequence = DOTween.Sequence();
            _sequence
                .Append(_transform.DOScale(_transform.localScale * 1.5f, 0.2f))
                .Insert(0f, _transform.DORotate(Vector3.zero, 0.2f))
                .Append(_transform.DOMove(_transform.position + Vector3.down * 5f, 1f))
                .AppendCallback(Death);
        }

        public void FeedItem(int index)
        {
            _sequence.Kill();
            _sequence = DOTween.Sequence();
            _sequence
                .Append(_transform.DOJump(_petFlask.GetFlyPosition(), 1f, 1, 1f))
                .AppendCallback(Death)
                .SetDelay(index/8f);
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (_isSpoiling && !_isSelect)
            {
                Death();
            }
        }

        private void SpoilingTimer()
        {
            if (_spoilingTimer > 0 && !_isSpoiling)
            {
                var colorMultiply = (_spoilingTimer-Time.time)/_spoilingTime;
                
                _spriteRenderer.color = new Color(0.3f+colorMultiply, 0.3f+colorMultiply, 0.3f+colorMultiply, 1);
                if (_spoilingTimer < Time.time)
                {
                    _isSpoiling = true;
                    _spriteRenderer.color = new Color(0.3f, 0.3f, 0.3f, 0.6f);
                    _foodType = FoodType.Ruined;
                    _spoilingTimer = 0;
                }
            }
        }

        public void MakeFresh()
        {
            _foodType = _initialFoodType;
            _spoilingTimer = 0;
            _spriteRenderer.color = Color.white;
        }
        
        private void FixedUpdate()
        {
            SpoilingTimer();
        }

        private void Death()
        {
            _sequence.Kill();
            _gameInfo.OnLoseGame -= Death;
            _gameInfo.OnWinGame -= Death;
            _spoilingTimer = 0;
            _spriteRenderer.color = Color.white;
            ParticleEffect particleEffect = _deathEffectPool.Pool.Get();
            particleEffect.StartEffect(_transform.position);
            _rigidbody2D.simulated = true;
            _transform.localScale = Vector3.one;
            _spriteRenderer.sortingOrder -= 1 + (_isSelect ? 1 : 0);
            _isSelect = false;
            _isSpoiling = false;
            Destroy(_polygonCollider2D);
            pool.Release(this);
        }
    }
}