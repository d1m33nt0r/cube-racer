using System;
using UnityEngine;

namespace DefaultNamespace.Services.ShopData
{
    public class PlayerShopAnimation : MonoBehaviour
    {
        private void Update()
        {
            if (transform.childCount > 0) Destroy(transform.GetChild(0));
        }
    }
}