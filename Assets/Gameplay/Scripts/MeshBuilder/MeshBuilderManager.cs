using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Gameplay.Scripts.Tools;
using UnityEngine;

namespace Gameplay.Scripts.MeshBuilder
{
    public class MeshBuilderManager : MonoBehaviour
    {
        [SerializeField] private CustomLineRenderer _lineRendererPrefab;
        
        public async UniTask<CustomLineRenderer> CreateLineRenderer()
        {
            var lineRenderer = Instantiate(_lineRendererPrefab,transform);
            await UniTask.DelayFrame(1);
            return lineRenderer;
        }
        
    }
}