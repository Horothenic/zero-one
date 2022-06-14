using System;
using UniRx;
using UnityEngine;

using Zenject;

namespace ZeroOne
{
    public partial class PlayerDataSource
    {
        #region FIELDS

        [Inject] private PlayerInputSource _playerInputSource = null;

        #endregion
    }

    public partial class PlayerDataSource : IMovementEventsSource
    {
        IObservable<Vector3> IMovementEventsSource.OnMoveObservable => _playerInputSource.OnMoveObservable;
    }

    public partial class PlayerDataSource : IJumpEventsSource
    {
        IObservable<Unit> IJumpEventsSource.OnJumpObservable => _playerInputSource.OnJumpObservable;
        IObservable<Unit> IJumpEventsSource.OnReleaseJumpObservable => _playerInputSource.OnReleaseJumpObservable;
    }

    public partial class PlayerDataSource : IAimEventsSource
    {
        IObservable<Vector2> IAimEventsSource.OnAimObservable => _playerInputSource.OnAimObservable;
    }
}
