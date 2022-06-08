using UnityEngine;
using System;

using UniRx;
using Zenject;

namespace ZeroOne
{
    public interface IMovementEventsSource
    {
        IObservable<Vector3> OnMoveObservable { get; }
    }

    public class MovementController : MonoBehaviour
    {
        #region FIELDS

        [Inject] private IMovementEventsSource _eventsSource = null;

        [Header("CONFIGURATIONS")]
        [SerializeField] private float _frontalSpeed = default;
        [SerializeField] private float _backwardSpeed = default;
        [SerializeField] private float _sideSpeed = default;

        private CompositeDisposable _disposables = new CompositeDisposable();
        private Rigidbody _rigidbody = null;

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

        #endregion

        #region METHODS

        private void Initialize()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _eventsSource.OnMoveObservable.Subscribe(Move).AddTo(_disposables);
        }

        private void Clean()
        {
            _disposables.Dispose();
        }

        private void Move(Vector3 direction)
        {
            direction.x *= _sideSpeed;
            direction.z *= direction.z < 0 ? _backwardSpeed : _frontalSpeed;

            var newPosition = _rigidbody.position + direction * Time.fixedDeltaTime;
            _rigidbody.MovePosition(newPosition);
        }

        #endregion
    }
}
