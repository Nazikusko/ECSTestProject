using Leopotam.EcsLite;
using UnityEngine;

public class Loader : MonoBehaviour
{
    EcsWorld _world;
    IEcsSystems _systems;

    void Start()
    {
        _world = new EcsWorld();
        _systems = new EcsSystems(_world);
        _systems.Add(new InitSystem());
        _systems.Add(new PlayerInputSystem());
        _systems.Add(new PlayerMoveSystem());
        _systems.Add(new PlayerAnimatedSystem());
        _systems.Add(new ButtonTriggerSystem());
        _systems.Add(new OpenDorAnimationSystem());
        _systems.Init();
    }

    void Update()
    {
        _systems?.Run();
    }

    void OnDestroy()
    {
        if (_systems != null)
        {
            _systems.Destroy();
            _systems = null;
        }

        if (_world != null)
        {
            _world.Destroy();
            _world = null;
        }
    }
}
