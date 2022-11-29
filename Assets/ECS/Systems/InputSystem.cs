using Leopotam.EcsLite;
using UnityEngine;

public class InputSystem : IEcsRunSystem, IEcsInitSystem
{

    private EcsWorld _world;
    EcsFilter _filter;


    public void Run(IEcsSystems systems)
    {
        if (Input.GetMouseButton(0))
        {
            foreach (int entity in _filter)
            {
                var inputComponents = _world.GetPool<InputComponent>();
                ref InputComponent inputComponent = ref inputComponents.Get(entity);

                Ray ray = inputComponent.mainCamera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hitPoint, Mathf.Infinity, LayerMask.GetMask("PointableGround")))
                {
                    inputComponent.pointDirection = hitPoint.point;
                }
            }
        }
    }

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _filter = _world.Filter<InputComponent>().End();
    }
}
