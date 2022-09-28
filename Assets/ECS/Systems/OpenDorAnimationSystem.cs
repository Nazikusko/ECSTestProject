using Leopotam.EcsLite;
using UnityEngine;

public class OpenDorAnimationSystem : IEcsRunSystem, IEcsInitSystem
{
    private EcsWorld _world;
    private EcsFilter _filter;
    private EcsPool<OpenDorAnimationComponent> _openDorAnimationComponents;


    public void Run(IEcsSystems systems)
    {
        foreach (int entity in _filter)
        {
            ref OpenDorAnimationComponent openDorAnimationComponent = ref _openDorAnimationComponents.Get(entity);

            if (openDorAnimationComponent.tryPushToOpen && !openDorAnimationComponent.isOpened)
            {
                openDorAnimationComponent.currentRotateAngle += Time.deltaTime * OpenDorAnimationComponent.openDorSpeed;

                if (openDorAnimationComponent.currentRotateAngle > 90f)
                {
                    openDorAnimationComponent.isOpened = true;
                    return;
                }
                openDorAnimationComponent.transform.rotation = openDorAnimationComponent.startRotation
                                                               * Quaternion.AngleAxis(openDorAnimationComponent.currentRotateAngle, Vector3.up);
                openDorAnimationComponent.tryPushToOpen = false;
            }
        }
    }

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _filter = _world.Filter<OpenDorAnimationComponent>().End();
        _openDorAnimationComponents = _world.GetPool<OpenDorAnimationComponent>();
    }
}
