using DefaultNamespace;
using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    [SerializeField] private BoxController boxController;
    [SerializeField] private StartingRoad startingRoad;


    public override void InstallBindings()
    {
        BindBoxController();
        BindStartingRoad();
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