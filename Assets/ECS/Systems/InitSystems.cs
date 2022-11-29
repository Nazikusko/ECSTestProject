using Leopotam.EcsLite;
using UnityEngine;

public class InitSystems : IEcsInitSystem
{
    private EcsWorld _world;
    private Camera mainCamera;
    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();

        mainCamera = GameObject.FindObjectOfType<Camera>();

        if (mainCamera == null)
        {
            Debug.LogError("Camera not found in scene");
            return;
        }

        SpawnAndSetupPlayer();
        SetupCamera();

        for (int i = 0; i < 5; i++)
        {
            SetupButton(i);
            SetupDor(i);
        }
    }

    private void SpawnAndSetupPlayer()
    {
        var playerEntity = _world.NewEntity();

        var transformPositionComponent = _world.GetPool<TransformPositionComponent>();
        var transformRotationComponent = _world.GetPool<TransformRotationComponent>();
        var movableComponent = _world.GetPool<MovableComponent>();
        var rotatableComponent = _world.GetPool<RotatableComponent>();


        var animatedCharacterComponent = _world.GetPool<AnimatedCharacterComponent>();
        var inputEventComponent = _world.GetPool<InputComponent>();

        movableComponent.Add(playerEntity);
        rotatableComponent.Add(playerEntity);
        transformPositionComponent.Add(playerEntity);
        transformRotationComponent.Add(playerEntity);

        animatedCharacterComponent.Add(playerEntity);
        inputEventComponent.Add(playerEntity);

        var playerInitData = PlayerInitData.LoadFromAsset();
        var spawnedPlayerPrefab = GameObject.Instantiate(playerInitData.playerPrefab, Vector3.zero, Quaternion.identity);
        animatedCharacterComponent.Get(playerEntity).animator = spawnedPlayerPrefab.GetComponent<Animator>();

        ref InputComponent input = ref inputEventComponent.Get(playerEntity);
        input.mainCamera = mainCamera;

        ref MovableComponent movable = ref movableComponent.Get(playerEntity);
        movable.moveSpeed = playerInitData.defaultSpeed;
        movable.position = spawnedPlayerPrefab.transform.position;

        ref RotatableComponent rotatable = ref rotatableComponent.Get(playerEntity);
        rotatable.rotationSpeed = playerInitData.defaultRotateSpeed;
        rotatable.rotation = spawnedPlayerPrefab.transform.rotation;

        ref TransformPositionComponent position = ref transformPositionComponent.Get(playerEntity);
        position.SetTransform(spawnedPlayerPrefab.transform);

        ref TransformRotationComponent rotation = ref transformRotationComponent.Get(playerEntity);
        rotation.SetTransform(spawnedPlayerPrefab.transform);
    }

    private void SetupCamera()
    {
        var cameraEntity = _world.NewEntity();
        var transformPositionComponent = _world.GetPool<TransformPositionComponent>();
        var movableComponent = _world.GetPool<MovableComponent>();
        var camFollowComponent = _world.GetPool<CameraFollowComponent>();

        transformPositionComponent.Add(cameraEntity);
        movableComponent.Add(cameraEntity);
        camFollowComponent.Add(cameraEntity);

        ref MovableComponent movable = ref movableComponent.Get(cameraEntity);
        movable.moveSpeed = 1;
        movable.position = mainCamera.transform.position;

        ref TransformPositionComponent position = ref transformPositionComponent.Get(cameraEntity);
        position.SetTransform(mainCamera.transform);
        
        ref CameraFollowComponent camFollow =  ref camFollowComponent.Get(cameraEntity);
        camFollow.positionOffset = mainCamera.transform.position;
    }

    private void SetupButton(int index)
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
        buttonTrigger.buttonPosition = buttonGameObject.transform.position;
        buttonTrigger.buttonRadius = buttonGameObject.GetComponent<MeshFilter>().mesh.bounds.max.x;
        buttonTrigger.index = index;
    }

    private void SetupDor(int index)
    {
        var dorEntity = _world.NewEntity();
        var openDorAnimationComponent = _world.GetPool<OpenDorAnimationComponent>();
        var transformRotationComponent = _world.GetPool<TransformRotationComponent>();
        var rotatableComponent = _world.GetPool<RotatableComponent>();

        openDorAnimationComponent.Add(dorEntity);
        transformRotationComponent.Add(dorEntity);
        rotatableComponent.Add(dorEntity);

        var dorGameObject = GameObject.Find($"Doors/DorPrefab{index}");
        if (dorGameObject == null)
        {
            Debug.LogError($"DorPrefab{index} - not found in scene");
            return;
        }

        ref OpenDorAnimationComponent dor = ref openDorAnimationComponent.Get(dorEntity);
        dorGameObject.GetComponentInChildren<MeshRenderer>().material.color = ColorByIndex(index);
        dor.index = index;
        dor.startRotation = dorGameObject.transform.rotation;

        ref RotatableComponent rotatable = ref rotatableComponent.Get(dorEntity);
        rotatable.rotationSpeed = OpenDorAnimationComponent.OPEN_DOR_SPEED;
        rotatable.startRotation = rotatable.rotation = dorGameObject.transform.rotation;

        ref TransformRotationComponent rotation = ref transformRotationComponent.Get(dorEntity);
        rotation.SetTransform(dorGameObject.transform);
    }

    private Color ColorByIndex(int index) => Color.HSVToRGB(index * 0.2f, 0.9f, 0.8f);

}
