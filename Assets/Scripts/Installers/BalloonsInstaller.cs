using UnityEngine;
using Zenject;

public class BalloonsInstaller : MonoInstaller
{
    [SerializeField] private OpenBalloonsCounter openBalloonsCounter;
    
    public override void InstallBindings()
    {
        BindBalloondsCounter();
    }

    private void BindBalloondsCounter()
    {
        Container
            .Bind<OpenBalloonsCounter>()
            .FromInstance(openBalloonsCounter)
            .AsSingle();
    }
}