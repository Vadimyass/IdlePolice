using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Scripts.ChoseLevelWindow
{
    public class ChoseLevelWindow : UIScreen
    {
        [field: SerializeField] public List<LevelView> LevelViews { get; private set; }
        [field: SerializeField] public Button CloseButton { get; private set; }
        [field: SerializeField] public List<Image> LevelLinks { get; private set; }
        [field: SerializeField] public Sprite ActiveSpriteLink { get; private set; }
        [field: SerializeField] public Sprite UnactiveSpriteLink { get; private set; }
        [field: SerializeField] public Transform BackgroundTransform { get; private set; }
    }
}