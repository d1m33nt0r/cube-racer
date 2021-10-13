using Zenject;

public class DiamondMultiplier
{
    private int multiplier;
    private SessionDiamondCounter sessionDiamondCounter;
    
    [Inject]
    private void Construct(SessionDiamondCounter sessionDiamondCounter)
    {
        this.sessionDiamondCounter = sessionDiamondCounter;
    }

    public void SetMultiplier(int multiplier)
    {
        this.multiplier = multiplier;
    }

    public int GetMultiplier()
    {
        return multiplier;
    }
}