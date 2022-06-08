using UnityEngine;

using Zenject;

namespace ZeroOne
{
    public class GameInstaller : MonoInstaller
    {
        [Header("COMPONENTS")]
        [SerializeField] private PlayerInputSource _movementInputSource = null;

        public override void InstallBindings()
        {
            BindDataSources();
            BindInput();
        }

        private void BindDataSources()
        {
            Container.BindInterfacesAndSelfTo<PlayerDataSource>().AsSingle();
        }

        private void BindInput()
        {
            Container.BindInterfacesAndSelfTo<PlayerInputSource>().FromInstance(_movementInputSource).AsSingle();
        }
    }
}
