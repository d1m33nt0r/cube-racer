public class SessionDiamondCounter
{
    private int count = 0;

    public void AddDiamond()
    {
        count++;
    }
    
    public int GetCount()
    {
        return count;
    }
}