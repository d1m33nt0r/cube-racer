using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    [SerializeField] private BoxController boxController;
    [SerializeField] private StartingRoad startingRoad;
    [SerializeField] private BoxAudioController boxAudioController;
    [SerializeField] private GameController gameController;
    [SerializeField] private HandController handController;
    
    public override void InstallBindings()
    {
        BindBoxController();
        BindStartingRoad();
        BindBoxAudioController();
        BindGameController();
        BindHandController();
    }

    private void BindHandController()
    {
        Container
            .Bind<HandController>()
            .FromInstance(handController)
            .AsSingle();
    }
    
    private void BindGameController()
    {
        Container
            .Bind<GameController>()
            .FromInstance(gameController)
            .AsSingle();
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