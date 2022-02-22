using DefaultNamespace;
using DefaultNamespace.ObjectPool;
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
    [SerializeField] private PlayerContainer playerContainer;
    [SerializeField] private PlayerMover playerMover;
    [SerializeField] private UIController uiController;
    //[SerializeField] private AdsManager adsManager;
    [SerializeField] private PoolManager poolManager;

    private TurnState _turnState;
    private DiamondUI diamondUI;
    private DiamondCounter diamondCounter => 
        diamondUI.transform.GetComponentInChildren<DiamondCounter>();

    [Inject]
    private void Construct(DiamondUI diamondUI)
    {
        this.diamondUI = diamondUI;
        diamondUI.settingsButton.GetComponent<SettingsButton>().Construct(gameController);
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
        BindSessionDiamondCounter();
        BindDiamondMultiplier();
        BindPlayerContainer();
        BindLevel();
        BindPlayerMover();
        BindUiController();
        BindPoolManager();

        BindTurnState();
    }

    private void BindTurnState()
    {
        _turnState = new TurnState();
        Container.Bind<TurnState>().FromInstance(_turnState).AsSingle();
    }


    private void BindUiController()
    {
        Container
            .Bind<UIController>()
            .FromInstance(uiController)
            .AsSingle();
    }
    
    private void BindPoolManager()
    {
        Container
            .Bind<PoolManager>()
            .FromInstance(poolManager)
            .AsSingle();
    }

    private void BindPlayerMover()
    {
        Container
            .Bind<PlayerMover>()
            .FromInstance(playerMover)
            .AsSingle();
    }

    private void BindPlayerContainer()
    {
        Container
            .Bind<PlayerContainer>()
            .FromInstance(playerContainer)
            .AsSingle();
    }

    private void BindDiamondMultiplier()
    {
        var diamondMultiplier = new DiamondMultiplier();

        Container
            .Bind<DiamondMultiplier>()
            .FromInstance(diamondMultiplier)
            .AsSingle();
    }

    private void BindSessionDiamondCounter()
    {
        var sessionDiamondCounter = new SessionDiamondCounter();
        
        Container
            .Bind<SessionDiamondCounter>()
            .FromInstance(sessionDiamondCounter)
            .AsSingle();
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