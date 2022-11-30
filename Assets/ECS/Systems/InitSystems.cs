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
        SetupInput();
        SetupCamera();

        for (int i = 0; i < 5; i++)
        {
            SetupButton(i);
            SetupDoor(i);
        }
    }

    private void SetupInput()
    {
        var inputEntity = _world.NewEntity();
        var inputEventComponent = _world.GetPool<InputComponent>();
        inputEventComponent.Add(inputEntity);
        ref InputComponent input = ref inputEventComponent.Get(inputEntity);
        input.mainCamera = mainCamera;
    }

    private void SpawnAndSetupPlayer()
    {
        var playerEntity = _world.NewEntity();

        var transformPositionComponent = _world.GetPool<TransformPositionComponent>();
        var transformRotationComponent = _world.GetPool<TransformRotationComponent>();
        var movableComponent = _world.GetPool<MovableComponent>();
        var rotatableComponent = _world.GetPool<RotatableComponent>();
        var pointToMoveComponent = _world.GetPool<PointToMoveComponent>();
        var animatedCharacterComponent = _world.GetPool<AnimationCharacterComponent>();

        transformPositionComponent.Add(playerEntity);
        transformRotationComponent.Add(playerEntity);
        movableComponent.Add(playerEntity);
        rotatableComponent.Add(playerEntity);
        pointToMoveComponent.Add(playerEntity);
        animatedCharacterComponent.Add(playerEntity);

        var playerInitData = PlayerInitData.LoadFromAsset();
        var spawnedPlayerPrefab = GameObject.Instantiate(playerInitData.playerPrefab, Vector3.zero, Quaternion.identity);
        animatedCharacterComponent.Get(playerEntity).animator = spawnedPlayerPrefab.GetComponent<Animator>();

        ref MovableComponent movable = ref movableComponent.Get(playerEntity);
        movable.moveSpeed = playerInitData.defaultSpeed;
        movable.position = spawnedPlayerPrefab.transform.position;

        ref RotatableComponent rotatable = ref rotatableComponent.Get(playerEntity);
        rotatable.rotationSpeed = playerInitData.defaultRotateSpeed;
        rotatable.rotation = spawnedPlayerPrefab.transform.rotation;

        ref TransformPositionComponent position = ref transformPositionComponent.Get(playerEntity);
        position.transform = spawnedPlayerPrefab.transform;

        ref TransformRotationComponent rotation = ref transformRotationComponent.Get(playerEntity);
        rotation.transform = spawnedPlayerPrefab.transform;

        ref PointToMoveComponent point = ref pointToMoveComponent.Get(playerEntity);
        point.pointToMove = spawnedPlayerPrefab.transform.position;
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
        position.transform = mainCamera.transform;
        
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

    private void SetupDoor(int index)
    {
        var doorEntity = _world.NewEntity();
        var openDoorAnimationComponent = _world.GetPool<OpenDoorAnimationComponent>();
        var transformRotationComponent = _world.GetPool<TransformRotationComponent>();
        var rotatableComponent = _world.GetPool<RotatableComponent>();

        openDoorAnimationComponent.Add(doorEntity);
        transformRotationComponent.Add(doorEntity);
        rotatableComponent.Add(doorEntity);

        var doorGameObject = GameObject.Find($"Doors/DoorPrefab{index}");
        if (doorGameObject == null)
        {
            Debug.LogError($"DoorPrefab{index} - not found in scene");
            return;
        }

        ref OpenDoorAnimationComponent door = ref openDoorAnimationComponent.Get(doorEntity);
        doorGameObject.GetComponentInChildren<MeshRenderer>().material.color = ColorByIndex(index);
        door.index = index;
        door.doorState = DoorSate.Stop;
        door.startRotation = doorGameObject.transform.rotation;

        ref RotatableComponent rotatable = ref rotatableComponent.Get(doorEntity);
        rotatable.rotationSpeed = OpenDoorAnimationComponent.OPEN_DOOR_SPEED;
        rotatable.startRotation = rotatable.rotation = doorGameObject.transform.rotation;

        ref TransformRotationComponent rotation = ref transformRotationComponent.Get(doorEntity);
        rotation.transform = doorGameObject.transform;
    }

    private Color ColorByIndex(int index) => Color.HSVToRGB(index * 0.2f, 0.9f, 0.8f);
}
