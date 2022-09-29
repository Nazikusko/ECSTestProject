using Leopotam.EcsLite;
using UnityEngine;

public class OpenDorAnimationSystem : IEcsInitSystem
{
    private EcsWorld _world;
    private EcsFilter _filter;
    private EcsPool<OpenDorAnimationComponent> _openDorAnimationComponents;

    private void OpeningDor(int index)
    {
        foreach (int entity in _filter)
        {
            ref OpenDorAnimationComponent openDorAnimationComponent = ref _openDorAnimationComponents.Get(entity);

            if (openDorAnimationComponent.index == index && !openDorAnimationComponent.isOpened)
            {
                openDorAnimationComponent.currentRotateAngle += Time.deltaTime * OpenDorAnimationComponent.OPEN_DOR_SPEED;

                if (openDorAnimationComponent.currentRotateAngle > 90f)
                {
                    openDorAnimationComponent.isOpened = true;
                    return;
                }
                openDorAnimationComponent.transform.rotation = openDorAnimationComponent.startRotation
                                                               * Quaternion.AngleAxis(openDorAnimationComponent.currentRotateAngle, Vector3.up);
            }
        }
    }

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _filter = _world.Filter<OpenDorAnimationComponent>().End();
        _openDorAnimationComponents = _world.GetPool<OpenDorAnimationComponent>();
        ButtonTriggerSystem.ButtonPressed += OpeningDor;
    }
}
