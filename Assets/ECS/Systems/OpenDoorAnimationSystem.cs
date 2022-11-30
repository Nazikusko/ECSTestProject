using Leopotam.EcsLite;
using UnityEngine;

public class OpenDoorAnimationSystem : IEcsInitSystem , IEcsRunSystem
{
    private EcsWorld _world;
    private EcsFilter _filter;
    private EcsPool<OpenDoorAnimationComponent> _openDoorAnimationComponents;
    private EcsPool<RotatableComponent> _rotatableComponents;
    
    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _filter = _world.Filter<OpenDoorAnimationComponent>().End();
        _openDoorAnimationComponents = _world.GetPool<OpenDoorAnimationComponent>();
        _rotatableComponents = _world.GetPool<RotatableComponent>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (int entity in _filter)
        {
            ref OpenDoorAnimationComponent openDoorAnimationComponent = ref _openDoorAnimationComponents.Get(entity);
            ref RotatableComponent rotatableComponent = ref _rotatableComponents.Get(entity);

            Debug.Log(openDoorAnimationComponent.doorState);

            if (openDoorAnimationComponent.doorState == DoorSate.Opening)
            {
                openDoorAnimationComponent.currentRotateAngle += Time.deltaTime * OpenDoorAnimationComponent.OPEN_DOOR_SPEED;

                if (openDoorAnimationComponent.currentRotateAngle > 90f)
                {
                    openDoorAnimationComponent.doorState = DoorSate.Open;
                    return;
                }
                rotatableComponent.rotation = openDoorAnimationComponent.startRotation
                                              * Quaternion.AngleAxis(openDoorAnimationComponent.currentRotateAngle, Vector3.up);
            }
        }
    }
}
