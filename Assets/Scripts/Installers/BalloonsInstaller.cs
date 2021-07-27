using Zenject;

public class BalloonsInstaller : MonoInstaller
{
    private OpenBalloonsCounter openBalloonsCounter;
    
    public override void InstallBindings()
    {
        BindBalloondsCounter();
    }

    private void BindBalloondsCounter()
    {
        var balloonsCounter = new OpenBalloonsCounter();

        Container
            .Bind<OpenBalloonsCounter>()
            .FromInstance(balloonsCounter)
            .AsSingle();
    }
}