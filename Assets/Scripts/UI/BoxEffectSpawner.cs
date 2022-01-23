using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using DefaultNamespace.ObjectPool;
using UnityEngine;
using Zenject;

namespace UI
{
    public class BoxEffectSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject effect;

        private DiamondUI diamondUI;
        private PoolManager m_poolManager;
        
        [Inject]
        private void Construct(DiamondUI _diamondUI, PoolManager _poolManager)
        {
            diamondUI = _diamondUI;
            m_poolManager = _poolManager;
        }
        
        private void SpawnEffect(List<Vector3> effectPositions)
        {
            var rotation = transform.parent.rotation;

            foreach (var position in effectPositions)
            {
                //Instantiate(effect, new Vector3(position.x, position.y + 0.2f, position.z), rotation, transform);
                var temp = m_poolManager.GetObject("box_effect");
                temp.SetActive(true);
                temp.transform.SetPositionAndRotation(new Vector3(position.x, position.y + 0.2f, position.z), rotation);
            }
                
            diamondUI.CreatePlusOneEffect(ConvertListWorldPositionsToScreenPositions(effectPositions));
        }

        private List<Vector2> ConvertListWorldPositionsToScreenPositions(List<Vector3> worldPositions)
        {
            var convertedPositions = new List<Vector2>();
            var camera = Camera.main;
            var positions = new List<Vector3>();
            
            foreach (var position in worldPositions)
                positions.Add(new Vector3(position.x, position.y + 0.2f, position.z));

            foreach (var position in positions)
                convertedPositions.Add(camera.WorldToScreenPoint(position));
            
            return convertedPositions;
        }
        
        public void Cal(int countEffects)
        {
            List<Vector3> collectBoxEffectPositions = new List<Vector3>();

            var boxController = GetComponent<BoxController>();
            var pos = boxController.GetBoxPositionXYZ();

            for (var i = 0; i < countEffects; i++)
                collectBoxEffectPositions.Add(new Vector3(pos.x, pos.y + i * boxController.heightBox, pos.z));

            SpawnEffect(collectBoxEffectPositions);
        }
    }
}