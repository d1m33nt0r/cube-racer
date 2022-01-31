using System;
using System.Collections.Generic;
using DefaultNamespace.Helpers;
using UnityEngine;

namespace DefaultNamespace
{
    [Serializable]
    public class ColumnCubes
    {
        public List<GameObject> cubes = new List<GameObject>();
    }

    [Serializable]
    public class SerializableCubeLetsDictionary : SerializableDictionary<int, ColumnCubes>
    {
        
    }
}