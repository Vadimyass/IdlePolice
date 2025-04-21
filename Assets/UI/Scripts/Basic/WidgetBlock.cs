using System;
using System.Collections.Generic;
using TypeReferences;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Scripts.MainScreen
{
    public abstract class WidgetBlock : MonoBehaviour
    {
        [SerializeField] protected List<CanvasGroup> _objects;
        public IReadOnlyList<CanvasGroup> Objects => _objects;
    }
}