using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Gameplay.Scripts.Buildings
{
    public class BuildingHologram : MonoBehaviour
    {
        [SerializeField] private Transform _hologram;
        [SerializeField] private List<MeshRenderer> _meshRenderers;
        private Vector3 _scale;

        private void Awake()
        {
            _scale = _hologram.localScale;

            foreach (var meshRenderer in _meshRenderers)
            {
                meshRenderer.sortingOrder = 15;
            }
        }

        public async void ScaleHologram()
        {
            return;
            _hologram.gameObject.SetActive(true);
            await _hologram.DOScale(_scale * 1.25f, 0.2f).From(_scale).SetEase(Ease.OutCirc).AsyncWaitForCompletion();
            _hologram.gameObject.SetActive(false);
        }
    }
}