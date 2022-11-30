using System;
using Leopotam.EcsLite;
using UnityEngine;

public class ButtonTriggerSystem : IEcsRunSystem, IEcsInitSystem
{
    private EcsWorld _world;
    private EcsFilter _filterForButtonTrigger;
    private EcsFilter _filterForPlayer;
    private EcsFilter _filterForDoor;
    private EcsPool<ButtonTriggerComponent> _buttonTriggerComponents;
    private EcsPool<MovableComponent> _movableComponents;
    private EcsPool<OpenDoorAnimationComponent> _openDoorAnimationComponents;


    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _filterForButtonTrigger = _world.Filter<ButtonTriggerComponent>().End();
        _filterForPlayer = _world.Filter<MovableComponent>().Inc<PointToMoveComponent>().End();
        _filterForDoor = _world.Filter<OpenDoorAnimationComponent>().End();

        _buttonTriggerComponents = _world.GetPool<ButtonTriggerComponent>();
        _movableComponents = _world.GetPool<MovableComponent>();
        _openDoorAnimationComponents = _world.GetPool<OpenDoorAnimationComponent>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entityPlayer in _filterForPlayer)
        {
            Vector3 playerPosition = _movableComponents.Get(entityPlayer).position;
            foreach (int entityButton in _filterForButtonTrigger)
            {
                ref ButtonTriggerComponent buttonTriggerComponent = ref _buttonTriggerComponents.Get(entityButton);

                if (Vector3.Distance(buttonTriggerComponent.buttonPosition, playerPosition) < buttonTriggerComponent.buttonRadius)
                {
                    SetDoorStateByIndex(DoorSate.Opening, buttonTriggerComponent.index);
                }
                else
                {
                    SetDoorStateByIndex(DoorSate.Stop, buttonTriggerComponent.index);
                }
            }
        }
    }

    private void SetDoorStateByIndex(DoorSate state, int index)
    {
        foreach (var entityDoor in _filterForDoor)
        {
            ref OpenDoorAnimationComponent openDoorAnimationComponent = ref _openDoorAnimationComponents.Get(entityDoor);

            if (openDoorAnimationComponent.doorState == DoorSate.Open) continue;

            if (index == openDoorAnimationComponent.index)
            {
                openDoorAnimationComponent.doorState = state;
            }
        }
    }
}
