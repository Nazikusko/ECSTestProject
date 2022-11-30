using Leopotam.EcsLite;
using UnityEngine;

public class InputSystem : IEcsRunSystem, IEcsInitSystem
{
    private EcsWorld _world;
    private EcsFilter _inputComponentFilter;
    private EcsFilter _pointToMoveComponentFilter;
    
    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _inputComponentFilter = _world.Filter<InputComponent>().End();
        _pointToMoveComponentFilter = _world.Filter<PointToMoveComponent>().End();
    }

    public void Run(IEcsSystems systems)
    {
        if (Input.GetMouseButton(0))
        {
            foreach (int entity in _inputComponentFilter)
            {
                var inputComponents = _world.GetPool<InputComponent>();
                ref InputComponent inputComponent = ref inputComponents.Get(entity);

                Ray ray = inputComponent.mainCamera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hitPoint, Mathf.Infinity, LayerMask.GetMask("PointableGround")))
                {
                    foreach (var pointEntity in _pointToMoveComponentFilter)
                    {
                        var pointToMoveComponents = _world.GetPool<PointToMoveComponent>();
                        ref PointToMoveComponent pointToMove = ref pointToMoveComponents.Get(pointEntity);
                        pointToMove.pointToMove = hitPoint.point;
                    }
                }
            }
        }
    }
}
