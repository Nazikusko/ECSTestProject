using Leopotam.EcsLite;

public class UpdateTransformRotationSystem: IEcsRunSystem, IEcsInitSystem
{
    private EcsWorld _world;
    private EcsFilter _filter;
    private EcsPool<RotatableComponent> _rotatableComponents;
    private EcsPool<TransformRotationComponent> _transformRotationComponents;

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _filter = _world.Filter<RotatableComponent>().End();
        _rotatableComponents = _world.GetPool<RotatableComponent>();
        _transformRotationComponents = _world.GetPool<TransformRotationComponent>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (int entity in _filter)
        {
            ref RotatableComponent movableComponent = ref _rotatableComponents.Get(entity);
            ref TransformRotationComponent transformPositionComponent = ref _transformRotationComponents.Get(entity);

            transformPositionComponent.transform.rotation = movableComponent.rotation;
        }
    }
}
