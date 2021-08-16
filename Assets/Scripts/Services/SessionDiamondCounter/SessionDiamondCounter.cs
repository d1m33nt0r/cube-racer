public class SessionDiamondCounter
{
    private int count = 0;

    public void AddDiamond()
    {
        count++;
    }

    public void AddDiamond(int count)
    {
        this.count += count;
    }
    
    public int GetCount()
    {
        return count;
    }
}