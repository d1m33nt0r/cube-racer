using DefaultNamespace;
using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    [SerializeField] private BoxController boxController;
    [SerializeField] private StartingRoad startingRoad;
    [SerializeField] private BoxAudioController boxAudioController;

    public override void InstallBindings()
    {
        BindBoxController();
        BindStartingRoad();
        BindBoxAudioController();
    }

    private void BindBoxAudioController()
    {
        Container
            .Bind<BoxAudioController>()
            .FromInstance(boxAudioController)
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