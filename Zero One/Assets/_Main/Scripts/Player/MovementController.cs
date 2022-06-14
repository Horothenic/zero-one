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

        private void Move(Vector3 inputDirection)
        {
            inputDirection.x *= _sideSpeed;
            inputDirection.z *= inputDirection.z < 0 ? _backwardSpeed : _frontalSpeed;

            Vector3 realDirection = Quaternion.AngleAxis(transform.eulerAngles.y, Vector3.up) * inputDirection;

            var newPosition = _rigidbody.position + realDirection * Time.fixedDeltaTime;
            _rigidbody.MovePosition(newPosition);
        }

        #endregion
    }
}
