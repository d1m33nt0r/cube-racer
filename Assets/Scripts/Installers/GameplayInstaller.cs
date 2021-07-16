using DefaultNamespace;
using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    [SerializeField] private BoxController boxController;
    [SerializeField] private StartingRoad startingRoad;
    [SerializeField] private PhysicsManipulator physicsManipulator;

    public override void InstallBindings()
    {
        BindBoxController();
        BindStartingRoad();
        BindPhysicsManipulator();
    }

    private void BindPhysicsManipulator()
    {
        Container
            .Bind<PhysicsManipulator>()
            .FromInstance(physicsManipulator)
            .AsSingle();
    }

    private void BindStartingRoad()
    {
        Container
            .Bind<StartingRoad>()
            .FromInstance(startingRoad)
            .AsSingle();
    }

    private void BindBoxController()
    {
        Container
            .Bind<BoxController>()
            .FromInstance(boxController)
            .AsSingle();
    }
}