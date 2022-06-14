using UnityEngine;
using System;

using Zenject;
using UniRx;

namespace ZeroOne
{
    public interface IAimEventsSource
    {
        IObservable<Vector2> OnAimObservable { get; }
    }

    public class AimController : MonoBehaviour
    {
        #region FIELDS

        [Inject] private IAimEventsSource _eventsSource = null;

        [Header("CONFIGURATIONS")]
        [SerializeField] private Transform _head = null;

        [Header("CONFIGURATIONS")]
        [SerializeField] private Vector2 _sensitivity = Vector2.one;
        [SerializeField] private float _maxVerticalAngle = default;

        private CompositeDisposable _disposables = new CompositeDisposable();
        private Vector2 _currentRotation = default;

        #endregion

        #region UNITY

        private void Start()
        {
            Initialize();
        }

        #endregion

        #region METHODS

        private void Initialize()
        {
            _eventsSource.OnAimObservable.Subscribe(RotateAim).AddTo(_disposables);
        }

        private void RotateAim(Vector2 addedRotation)
        {
            _currentRotation = AddAndFixRotation(_currentRotation, addedRotation);

            _head.localEulerAngles = new Vector3(_currentRotation.x, 0);
            transform.localEulerAngles = new Vector3(0, _currentRotation.y);
        }

        private Vector2 AddAndFixRotation(Vector2 currentRotation, Vector2 addedRotation)
        {
            currentRotation += addedRotation * _sensitivity;
            currentRotation.x = Mathf.Clamp(currentRotation.x, -_maxVerticalAngle, _maxVerticalAngle);

            return currentRotation;
        }

        #endregion
    }
}
