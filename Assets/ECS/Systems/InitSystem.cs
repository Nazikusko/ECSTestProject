using Leopotam.EcsLite;
using UnityEngine;

public class InitSystem : IEcsInitSystem
{
    private EcsWorld _world;
    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();

        SpawnAndSetupPlayer();

        for (int i = 0; i < 5; i++)
        {
            SetButton(i);
            SetDor(i);
        }
    }

    private void SpawnAndSetupPlayer()
    {
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
        movable.rotationSpeed = playerInitData.defaultRotateSpeed;
        movable.transform = spawnedPlayerPrefab.transform;
    }

    private void SetButton(int index)
    {
        var buttonEntity = _world.NewEntity();
        var buttonTigerComponent = _world.GetPool<ButtonTriggerComponent>();
        buttonTigerComponent.Add(buttonEntity);

        var buttonGameObject = GameObject.Find($"Buttons/ButtonPrefab{index}");
        if (buttonGameObject == null)
        {
            Debug.LogError($"ButtonPrefab{index} - not found in scene");
            return;
        }

        ref ButtonTriggerComponent buttonTrigger = ref buttonTigerComponent.Get(buttonEntity);
        buttonGameObject.GetComponent<MeshRenderer>().material.color = ColorByIndex(index);
        buttonTrigger.transform = buttonGameObject.transform;
        buttonTrigger.buttonRadius = buttonTrigger.transform.GetComponent<MeshFilter>().mesh.bounds.extents.x;
        buttonTrigger.index = index;
    }

    private void SetDor(int index)
    {
        var dorEntity = _world.NewEntity();
        var openDorAnimationComponent = _world.GetPool<OpenDorAnimationComponent>();
        openDorAnimationComponent.Add(dorEntity);

        var dorGameObject = GameObject.Find($"Dors/DorPrefab{index}");
        if (dorGameObject == null)
        {
            Debug.LogError($"DorPrefab{index} - not found in scene");
            return;
        }

        ref OpenDorAnimationComponent dor = ref openDorAnimationComponent.Get(dorEntity);

        dorGameObject.GetComponentInChildren<MeshRenderer>().material.color = ColorByIndex(index);
        dor.transform = dorGameObject.transform;
        dor.startRotation = dorGameObject.transform.rotation;
        dor.index = index;
    }

    private Color ColorByIndex(int index) => Color.HSVToRGB(index * 0.2f, 0.9f, 0.8f);

}
