using Leopotam.EcsLite;

class CameraFollowSystem : IEcsRunSystem, IEcsInitSystem
{
    private EcsWorld _world;
    private EcsFilter _cameraFilter;
    private EcsFilter _playerFilter;
    private EcsPool<MovableComponent> _movableComponents;
    private EcsPool<CameraFollowComponent> _cameraFollowComponents;

    public void Run(IEcsSystems systems)
    {
        foreach (var cameraEntity in _cameraFilter)
        {
            ref MovableComponent movableComponent = ref _movableComponents.Get(cameraEntity);
            ref CameraFollowComponent cameraFollowComponent = ref _cameraFollowComponents.Get(cameraEntity);

            foreach (var playerEntity in _playerFilter)
            {
                ref MovableComponent playerMovableComponent = ref _movableComponents.Get(playerEntity);
                
                movableComponent.position.x = playerMovableComponent.position.x + cameraFollowComponent.positionOffset.x;
                movableComponent.position.z = playerMovableComponent.position.z + cameraFollowComponent.positionOffset.z;
                movableComponent.position.y = cameraFollowComponent.positionOffset.y;
            }
        }
    }

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _cameraFilter = _world.Filter<CameraFollowComponent>().End();
        _playerFilter = _world.Filter<MovableComponent>().Inc<InputComponent>().End();

        _movableComponents = _world.GetPool<MovableComponent>();
        _cameraFollowComponents = _world.GetPool<CameraFollowComponent>();
    }
}
