﻿using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;
using Zenject;

namespace UI
{
    public class BoxEffectSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject effect;

        private DiamondUI diamondUI;
        
        [Inject]
        private void Construct(DiamondUI _diamondUI)
        {
            diamondUI = _diamondUI;
        }
        
        private void SpawnEffect(List<Vector3> effectPositions)
        {
            var rotation = transform.parent.rotation;
            
            foreach (var position in effectPositions)
                Instantiate(effect, new Vector3(position.x, position.y - 0.1f, position.z), rotation, transform);

            diamondUI.CreatePlusOneEffect(ConvertListWorldPositionsToScreenPositions(effectPositions));
        }

        private List<Vector2> ConvertListWorldPositionsToScreenPositions(List<Vector3> worldPositions)
        {
            var convertedPositions = new List<Vector2>();
            var camera = Camera.main;
            var positions = new List<Vector3>();
            
            foreach (var position in worldPositions)
                positions.Add(new Vector3(position.x, position.y, position.z));

            foreach (var position in positions)
                convertedPositions.Add(camera.WorldToScreenPoint(position));
            
            return convertedPositions;
        }
        
        public void Cal(int countEffects)
        {
            List<Vector3> collectBoxEffectPositions = new List<Vector3>();

            var boxController = GetComponent<BoxController>();
            
            var pos = boxController.GetBoxPositionXZ();
            var pos2 = StartPoint.startPoint;

            for (var i = 1; i < countEffects + 1; i++)
                collectBoxEffectPositions.Add(new Vector3(pos.x, (pos2.y + (boxController.boxCount - countEffects) * 0.2f) + 0.2f * i, pos.z));

            SpawnEffect(collectBoxEffectPositions);
        }
    }
}