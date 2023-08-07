# ECSTestProject
Это тестовое задание (Onboarding) по ECS

На выполнение и доработку в сумме ушло времени: 24ч.

На сервере могут быть запущены следующие системы:
- ButtonTriggerSystem
- CameraFollowSystem
- OpenDoorAnimationSystem
- PlayerMoveSystem

На клиенте должны быть заущены следующие системы:
- InitSystems
- InputSystem
- PlayerAnimationSystem
- UpdateTransformPositionSystem
- UpdateTransformRotationSystem

Не некоторых серверных системах, из Unity API присутсвует Time.DeltaTime - подразумевается что при переносе на сервер этот метод заменится на какой либо серверный таймер.
