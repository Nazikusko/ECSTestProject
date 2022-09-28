using System.Collections;
using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;

public class PlayerMoveSystem : IEcsRunSystem
{
    private EcsWorld _world;
    private EcsFilter _filter;
    private EcsPool<InputEventComponent> _inputComponents;
    private EcsPool<MovableComponent> _movableComponents;

    public void Run(IEcsSystems systems)
    {
        if (_world == null)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<InputEventComponent>().Inc<MovableComponent>().End();

            _inputComponents = _world.GetPool<InputEventComponent>();
            _movableComponents = _world.GetPool<MovableComponent>();
        }

        foreach (int entity in _filter)
        {
            ref InputEventComponent inputComponent = ref _inputComponents.Get(entity);
            ref MovableComponent movableComponent = ref _movableComponents.Get(entity);

            movableComponent.transform.position = Vector3.MoveTowards(movableComponent.transform.position,
                inputComponent.pointDirection, Time.deltaTime * movableComponent.moveSpeed);

            movableComponent.isMoving = Mathf.Abs(
                Vector3.SqrMagnitude(movableComponent.transform.position - inputComponent.pointDirection)) > 0.01f;

            if (movableComponent.isMoving)
            {
                Vector3 targetVector = inputComponent.pointDirection - movableComponent.transform.position;
                movableComponent.transform.forward = Vector3.Slerp(movableComponent.transform.forward,
                    targetVector, Time.deltaTime * movableComponent.rotateSpeed);
            }
        }
    }
}
