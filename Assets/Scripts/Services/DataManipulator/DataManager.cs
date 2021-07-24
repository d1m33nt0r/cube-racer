using System.Collections.Generic;
using Services.ProgressController;
using Services.ProgressController.Interfaces;

public class DataManager<T> where T: IDataManipulator
{
    private List<IDataManipulator> maganers;

    public DataManager()
    {
        maganers = new List<IDataManipulator>();
        maganers.Add(new DiamondCountManager());
    }
    
    public void ReadData()
    {
        foreach (var maganer in maganers)
        {
            maganer.ReadData();
        }
    }

    public void WriteData()
    {
        foreach (var maganer in maganers)
        {
            maganer.WriteData();
        }
    }
}
