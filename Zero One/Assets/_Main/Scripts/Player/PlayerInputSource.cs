using UnityEngine;
using System;

using UniRx;

namespace ZeroOne
{
    public class PlayerInputSource : MonoBehaviour
    {
        #region FIELDS

        private Subject<Vector3> _onMoveSubject = new Subject<Vector3>();
        private Subject<Unit> _onJumpSubject = new Subject<Unit>();
        private Subject<Unit> _onReleaseJumpSubject = new Subject<Unit>();
        private Subject<Vector2> _onAimObservable = new Subject<Vector2>();

        public IObservable<Vector3> OnMoveObservable => _onMoveSubject.AsObservable();
        public IObservable<Unit> OnJumpObservable => _onJumpSubject.AsObservable();
        public IObservable<Unit> OnReleaseJumpObservable => _onReleaseJumpSubject.AsObservable();
        public IObservable<Vector2> OnAimObservable => _onAimObservable.AsObservable();

        private Vector3 _currentMoveDirection = default;

        #endregion

        #region UNITY

        private void Start()
        {
            Initialize();
        }

        private void Update()
        {
            CheckMoveInput();
            CheckJumpInput();
            CheckAimInput();
        }

        private void FixedUpdate()
        {
            SendMoveInput();
        }

        #endregion

        #region METHODS

        private void Initialize()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void CheckMoveInput()
        {
            var horizontal = Input.GetAxisRaw("Horizontal");
            var vertical = Input.GetAxisRaw("Vertical");

            _currentMoveDirection = new Vector3(horizontal, 0, vertical);
        }

        private void CheckJumpInput()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SendJumpInput();
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                SendReleaseJumpInput();
            }
        }

        private void CheckAimInput()
        {
            var x = Input.GetAxis("Mouse X");
            var y = Input.GetAxis("Mouse Y");

            _onAimObservable.OnNext(new Vector2(y, x));
        }

        private void SendMoveInput()
        {
            _onMoveSubject.OnNext(_currentMoveDirection);
        }

        private void SendJumpInput()
        {
            _onJumpSubject.OnNext();
        }

        private void SendReleaseJumpInput()
        {
            _onReleaseJumpSubject.OnNext();
        }

        #endregion
    }
}
