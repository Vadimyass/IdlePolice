using System;
using Gameplay.Scripts.Buildings;
using ModestTree.Util;
using UnityEngine.Events;

namespace UI.Scripts.OfficerChoiceWidget
{
    public class OfficerChoiceWidgetArguments : UIArguments
    {
        public UnityAction<string, bool> Action { get; private set; }
        public Building Building { get; private set; }

        public OfficerChoiceWidgetArguments(Building building, UnityAction<string, bool> action)
        {
            Action = action;
            Building = building;
        }
    }
}