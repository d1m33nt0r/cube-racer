using System;
using DefaultNamespace.Helpers;


namespace DefaultNamespace.ObjectPool
{
    [Serializable]
    public class SerializablePool : SerializableDictionary<string, Pool>
    {
        
    }
}