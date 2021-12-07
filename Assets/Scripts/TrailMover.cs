using System.Collections;
using System.Collections.Generic;
using Ara;
using DefaultNamespace;
using DefaultNamespace.ThemeManager;
using UnityEngine;
using Zenject;

public class TrailMover : MonoBehaviour
{
    [Inject]
    private void Construct(ThemeManager _themeManager)
    {
        GetComponent<AraTrail>().materials = new[] {_themeManager.GetCurrentTrailTheme()};
    }
    
    void Update()
    {
        transform.position =
            new Vector3(transform.parent.GetComponentInChildren<PlayerMover>().transform.position.x, 
                transform.position.y, transform.parent.GetComponentInChildren<PlayerMover>().transform.position.z);
    }
}
