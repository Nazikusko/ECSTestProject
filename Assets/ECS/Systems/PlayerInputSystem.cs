using System.Collections;
using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;

public class PlayerInputSystem : IEcsRunSystem
{
    private Camera _mainCamera;
    private EcsWorld _world;
    EcsFilter _filter;
    public void Run(IEcsSystems systems)
    {
        if (Input.GetMouseButton(0))
        {
            if (_mainCamera == null) _mainCamera = GameObject.FindObjectOfType<Camera>();
            if (_mainCamera == null) return;

            if (_world == null)
            {
                _world = systems.GetWorld();
                _filter = _world.Filter<InputEventComponent>().End();
            }

            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitPoint;

            if (Physics.Raycast(ray, out hitPoint, Mathf.Infinity, LayerMask.GetMask("PointableGround")))
            {
                var inputComponents = _world.GetPool<InputEventComponent>();

                foreach (int entity in _filter)
                {
                    ref InputEventComponent inputComponent = ref inputComponents.Get(entity);
                    inputComponent.pointDirection = hitPoint.point;
                }
            }
        }
    }
}
