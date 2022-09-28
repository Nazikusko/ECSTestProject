using System.Collections;
using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;

public class InitSystem : IEcsInitSystem
{
    private EcsWorld _world;
    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();


        //Player character
        var playerEntity = _world.NewEntity();
        var movableComponent = _world.GetPool<MovableComponent>();
        var animatedCharacterComponent = _world.GetPool<AnimatedCharacterComponent>();
        var inputEventComponent = _world.GetPool<InputEventComponent>();
        movableComponent.Add(playerEntity);
        animatedCharacterComponent.Add(playerEntity);
        inputEventComponent.Add(playerEntity);


        var playerInitData = PlayerInitData.LoadFromAsset();
        var spawnedPlayerPrefab = GameObject.Instantiate(playerInitData.playerPrefab, Vector3.zero, Quaternion.identity);
        animatedCharacterComponent.Get(playerEntity).animator = spawnedPlayerPrefab.GetComponent<Animator>();

        ref MovableComponent movable = ref movableComponent.Get(playerEntity);
        movable.moveSpeed = playerInitData.defaultSpeed;
        movable.rotateSpeed = playerInitData.defaultRotateSpeed;
        movable.transform = spawnedPlayerPrefab.transform;

        for (int i = 0; i < 5; i++)
        {
            CreateButton(i);
            CreateDor(i);
        }
    }

    private void CreateButton(int index)
    {
        var buttonEntity = _world.NewEntity();
        var buttonTigerComponent = _world.GetPool<ButtonTriggerComponent>();
        buttonTigerComponent.Add(buttonEntity);

        var spawnedButton = GameObject.Instantiate(Resources.Load<GameObject>("ButtonPrefab"),
            new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f)), Quaternion.identity);

        ref ButtonTriggerComponent buttonTrigger = ref buttonTigerComponent.Get(buttonEntity);
        spawnedButton.GetComponent<MeshRenderer>().material.color = ColorByIndex(index);
        buttonTrigger.transform = spawnedButton.transform;
        buttonTrigger.index = index;
    }

    private void CreateDor(int index)
    {
        var dorEntity = _world.NewEntity();
        var openDorAnimationComponent = _world.GetPool<OpenDorAnimationComponent>();
        openDorAnimationComponent.Add(dorEntity);

        var dorInitData = DorInitData.LoadFromAsset();
        var spawnedDor = GameObject.Instantiate(dorInitData.dorPrefab,
            new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f)), Quaternion.identity);

        ref OpenDorAnimationComponent dor = ref openDorAnimationComponent.Get(dorEntity);

        spawnedDor.GetComponentInChildren<MeshRenderer>().material.color = ColorByIndex(index);
        dor.openDorSpeed = dorInitData.defaultRotateSpeed;
        dor.transform = spawnedDor.transform;
        dor.index = index;
    }

    private Color ColorByIndex(int index) => Color.HSVToRGB(index * 0.2f, 0.9f, 0.8f);

}
