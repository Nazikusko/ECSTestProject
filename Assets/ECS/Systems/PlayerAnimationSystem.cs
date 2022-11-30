using Leopotam.EcsLite;

public class PlayerAnimationSystem : IEcsRunSystem, IEcsInitSystem
{
    private EcsWorld _world;
    private EcsFilter _filter;
    private EcsPool<AnimationCharacterComponent> _animatedCharacterComponents;
    private EcsPool<MovableComponent> _movableComponents;

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _filter = _world.Filter<AnimationCharacterComponent>().Inc<MovableComponent>().End();

        _animatedCharacterComponents = _world.GetPool<AnimationCharacterComponent>();
        _movableComponents = _world.GetPool<MovableComponent>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (int entity in _filter)
        {
            ref AnimationCharacterComponent animationCharacterComponent = ref _animatedCharacterComponents.Get(entity);
            ref MovableComponent movableComponent = ref _movableComponents.Get(entity);
            animationCharacterComponent.animator.SetBool("IsMoving", movableComponent.isMoving);
        }
    }
}
