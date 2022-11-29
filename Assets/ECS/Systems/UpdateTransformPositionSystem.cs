using Leopotam.EcsLite;

class UpdateTransformPositionSystem : IEcsRunSystem, IEcsInitSystem
{
    private EcsWorld _world;
    private EcsFilter _filter;
    private EcsPool<MovableComponent> _movableComponents;
    private EcsPool<TransformPositionComponent> _transformPositionComponents;

    public void Run(IEcsSystems systems)
    {
        foreach (int entity in _filter)
        {
            ref MovableComponent movableComponent = ref _movableComponents.Get(entity);
            ref TransformPositionComponent transformPositionComponent = ref _transformPositionComponents.Get(entity);

            transformPositionComponent.SetPosition(movableComponent.position);
        }
    }

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _filter = _world.Filter<MovableComponent>().Inc<TransformPositionComponent>().End();
        _movableComponents = _world.GetPool<MovableComponent>();
        _transformPositionComponents = _world.GetPool<TransformPositionComponent>();
    }
}

