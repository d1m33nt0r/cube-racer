using DefaultNamespace;
using Diamond;
using UI;
using UI.GlobalUI.DiamondCounter;
using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    [SerializeField] private BoxController boxController;
    [SerializeField] private StartingRoad startingRoad;
    [SerializeField] private BoxAudioController boxAudioController;
    [SerializeField] private GameController gameController;
    [SerializeField] private HandController handController;
    [SerializeField] private GameplayStarter gameplayStarter;
    [SerializeField] private Level level;
    [SerializeField] private DiamondAudioController diamondAudioController;
    
    private DiamondUI diamondUI;
    private DiamondCounter diamondCounter => 
        diamondUI.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<DiamondCounter>();

    [Inject]
    private void Construct(DiamondUI diamondUI)
    {
        this.diamondUI = diamondUI;
    }
    
    public override void InstallBindings()
    {
        BindBoxController();
        BindStartingRoad();
        BindBoxAudioController();
        BindGameController();
        BindHandController();
        BindGameplayStarter();
        BindDiamondCounter();

        BindDiamondAudioController();
        
        BindLevel();
    }

    private void BindDiamondAudioController()
    {
        Container
            .Bind<DiamondAudioController>()
            .FromInstance(diamondAudioController)
            .AsSingle();
    }

    private void BindLevel()
    {
        Container
            .Bind<Level>()
            .FromInstance(level)
            .AsSingle();
    }

    private void BindDiamondCounter()
    {
        Container
            .Bind<DiamondCounter>()
            .FromInstance(diamondCounter)
            .AsSingle();
    }

    private void BindGameplayStarter()
    {
        Container
            .Bind<GameplayStarter>()
            .FromInstance(gameplayStarter)
            .AsSingle();
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