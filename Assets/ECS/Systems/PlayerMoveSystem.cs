using Leopotam.EcsLite;
using UnityEngine;

public class PlayerMoveSystem : IEcsRunSystem, IEcsInitSystem
{
    private EcsWorld _world;
    private EcsFilter _filter;
    private EcsPool<InputComponent> _inputComponents;
    private EcsPool<MovableComponent> _movableComponents;
    private EcsPool<RotatableComponent> _rotatableComponents;

    public void Run(IEcsSystems systems)
    {
        foreach (int entity in _filter)
        {
            ref InputComponent inputComponent = ref _inputComponents.Get(entity);
            ref MovableComponent movableComponent = ref _movableComponents.Get(entity);
            ref RotatableComponent rotatableComponent = ref _rotatableComponents.Get(entity);

            movableComponent.position = Vector3.MoveTowards(movableComponent.position,
                inputComponent.pointDirection, Time.deltaTime * movableComponent.moveSpeed);

            movableComponent.isMoving = Mathf.Abs(
                Vector3.SqrMagnitude(movableComponent.position - inputComponent.pointDirection)) > 0.01f;

            if (movableComponent.isMoving)
            {
                Vector3 targetVector = inputComponent.pointDirection - movableComponent.position;
                rotatableComponent.rotation = Quaternion.Lerp(rotatableComponent.rotation, Quaternion.LookRotation(targetVector),
                    Time.deltaTime * rotatableComponent.rotationSpeed);
            }
        }
    }

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _filter = _world.Filter<InputComponent>().Inc<MovableComponent>().End();
        _inputComponents = _world.GetPool<InputComponent>();
        _movableComponents = _world.GetPool<MovableComponent>();
        _rotatableComponents = _world.GetPool<RotatableComponent>();
    }
}
