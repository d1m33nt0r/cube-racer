using UnityEngine;

namespace UI.Shop
{
    public class ShopTabsController
    {
        private bool firstTabIsActive;
        
        [SerializeField] private GameObject cubeDemo;
        [SerializeField] private GameObject characterDemo;
        [SerializeField] private Animator shopUIAnimator;
        
        public void SwitchTab(string tabName)
        {
            switch (tabName)
            {
                case "Characters":
                    
                    break;
                case "Cubes":
                    
                    break;
            }
        }
    }
}