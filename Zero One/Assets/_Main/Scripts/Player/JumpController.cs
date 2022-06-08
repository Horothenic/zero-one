using UnityEngine;
using System;

using UniRx;
using Zenject;

namespace ZeroOne
{
    public interface IJumpEventsSource
    {
        IObservable<Unit> OnJumpObservable { get; }
        IObservable<Unit> OnReleaseJumpObservable { get; }
    }

    public class JumpController : MonoBehaviour
    {
        #region FIELDS

        [Inject] private IJumpEventsSource _eventsSource = null;

        [Header("CONFIGURATIONS")]
        [SerializeField] private float _jumpSpeed = 10;
        [SerializeField] private LayerMask _floorLayer = default;
        [Range(1, 10)][SerializeField] private float _canceledJumpGravity = 3f;
        [Range(1, 10)][SerializeField] private float _extraFallGravity = 3f;

        private CompositeDisposable _disposables = new CompositeDisposable();
        private Rigidbody _rigidbody = null;
        private bool _jumped = false;
        private bool _canceledJump = false;

        #endregion

        #region UNITY

        private void Start()
        {
            Initialize();
        }

        private void OnDestroy()
        {
            Clean();
        }

        private void FixedUpdate()
        {
            CanceledJumpExtraGravity();
        }

        private void OnCollisionEnter(Collision collision)
        {
            CheckCollision(collision);
        }

        #endregion

        #region METHODS

        private void Initialize()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _eventsSource.OnJumpObservable.Subscribe(Jump).AddTo(_disposables);
            _eventsSource.OnReleaseJumpObservable.Subscribe(ReleaseJump).AddTo(_disposables);
            ResetJump();
        }

        private void Clean()
        {
            _disposables.Dispose();
        }

        private void Jump()
        {
            if (_jumped) return;

            _jumped = true;
            _rigidbody.velocity = Vector3.up * _jumpSpeed;
        }

        private void ReleaseJump()
        {
            if (!_jumped) return;

            _canceledJump = true;
        }

        private void CanceledJumpExtraGravity()
        {
            var extraGravity = (_jumped && _canceledJump) ? _canceledJumpGravity : _extraFallGravity;
            _rigidbody.velocity += Vector3.up * Physics.gravity.y * (extraGravity - 1) * Time.fixedDeltaTime;
        }

        private void CheckCollision(Collision collision)
        {
            if (_floorLayer.ContainsLayer(collision.gameObject.layer))
            {
                ResetJump();
            }
        }

        private void ResetJump()
        {
            _jumped = false;
            _canceledJump = false;
        }

        #endregion
    }
}
