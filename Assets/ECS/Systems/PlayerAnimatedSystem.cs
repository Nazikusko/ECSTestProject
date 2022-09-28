using System.Collections;
using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;

public class PlayerAnimatedSystem : IEcsRunSystem, IEcsInitSystem
{

    private EcsWorld _world;
    private EcsFilter _filter;
    private EcsPool<AnimatedCharacterComponent> _animatedCharacterComponents;
    private EcsPool<MovableComponent> _movableComponents;

    public void Run(IEcsSystems systems)
    {
        foreach (int entity in _filter)
        {
            ref AnimatedCharacterComponent animatedCharacterComponent = ref _animatedCharacterComponents.Get(entity);
            ref MovableComponent movableComponent = ref _movableComponents.Get(entity);
            animatedCharacterComponent.animator.SetBool("IsMoving", movableComponent.isMoving);
        }
    }

    public void Init(IEcsSystems systems)
    {
        if (_world == null)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<AnimatedCharacterComponent>().Inc<MovableComponent>().End();

            _animatedCharacterComponents = _world.GetPool<AnimatedCharacterComponent>();
            _movableComponents = _world.GetPool<MovableComponent>();
        }
    }
}
