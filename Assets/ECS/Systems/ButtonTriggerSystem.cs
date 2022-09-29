using System;
using Leopotam.EcsLite;
using UnityEngine;

public class ButtonTriggerSystem : IEcsRunSystem, IEcsInitSystem
{
    public static event Action<int> ButtonPressed;
    private EcsWorld _world;
    private EcsFilter _filterForButtonTrigger;
    private EcsFilter _filterForMovable;
    private EcsFilter _filterForDor;
    private EcsPool<ButtonTriggerComponent> _buttonTriggerComponents;
    private EcsPool<MovableComponent> _movableComponents;
    private EcsPool<OpenDorAnimationComponent> _openDorAnimationComponents;

    public void Run(IEcsSystems systems)
    {
        foreach (var entityMovable in _filterForMovable)
        {
            Vector3 position = _movableComponents.Get(entityMovable).transform.position;
            foreach (int entityButton in _filterForButtonTrigger)
            {
                ref ButtonTriggerComponent buttonTriggerComponent = ref _buttonTriggerComponents.Get(entityButton);

                if (Vector3.Distance(buttonTriggerComponent.transform.position, position) < buttonTriggerComponent.buttonRadius)
                {
                    foreach (var entityDor in _filterForDor)
                    {
                        if (buttonTriggerComponent.index == _openDorAnimationComponents.Get(entityDor).index)
                            ButtonPressed?.Invoke(buttonTriggerComponent.index);
                    }
                    buttonTriggerComponent.inTrigger = true;
                }
                else
                {
                    buttonTriggerComponent.inTrigger = false;
                }
            }
        }
    }

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _filterForButtonTrigger = _world.Filter<ButtonTriggerComponent>().End();
        _filterForMovable = _world.Filter<MovableComponent>().End();
        _filterForDor = _world.Filter<OpenDorAnimationComponent>().End();
        _buttonTriggerComponents = _world.GetPool<ButtonTriggerComponent>();
        _movableComponents = _world.GetPool<MovableComponent>();
        _openDorAnimationComponents = _world.GetPool<OpenDorAnimationComponent>();
    }
}
