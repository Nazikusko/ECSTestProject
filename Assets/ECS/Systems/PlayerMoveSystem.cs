using Leopotam.EcsLite;
using UnityEngine;

public class PlayerMoveSystem : IEcsRunSystem, IEcsInitSystem
{
    private EcsWorld _world;
    private EcsFilter _filter;
    private EcsPool<PointToMoveComponent> _pointToMoveComponents;
    private EcsPool<MovableComponent> _movableComponents;
    private EcsPool<RotatableComponent> _rotatableComponents;

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _filter = _world.Filter<PointToMoveComponent>().Inc<MovableComponent>().End();
        _pointToMoveComponents = _world.GetPool<PointToMoveComponent>();
        _movableComponents = _world.GetPool<MovableComponent>();
        _rotatableComponents = _world.GetPool<RotatableComponent>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (int entity in _filter)
        {
            ref PointToMoveComponent pointToMoveComponent = ref _pointToMoveComponents.Get(entity);
            ref MovableComponent movableComponent = ref _movableComponents.Get(entity);
            ref RotatableComponent rotatableComponent = ref _rotatableComponents.Get(entity);

            movableComponent.position = Vector3.MoveTowards(movableComponent.position,
                pointToMoveComponent.pointToMove, Time.deltaTime * movableComponent.moveSpeed);

            movableComponent.isMoving = Mathf.Abs(
                Vector3.SqrMagnitude(movableComponent.position - pointToMoveComponent.pointToMove)) > 0.01f;

            if (movableComponent.isMoving)
            {
                Vector3 targetVector = pointToMoveComponent.pointToMove - movableComponent.position;
                rotatableComponent.rotation = Quaternion.Lerp(rotatableComponent.rotation, Quaternion.LookRotation(targetVector),
                    Time.deltaTime * rotatableComponent.rotationSpeed);
            }
        }
    }
}
