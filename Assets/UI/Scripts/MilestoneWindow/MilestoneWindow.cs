using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Scripts.MilestoneWindow
{
    public class MilestoneWindow : UIScreen
    {
        [field: SerializeField] public List<MilestoneRewardView> MilestoneRewardViews { get; private set; }
        [field: SerializeField] public List<Image> MilestoneRewardLinks { get; private set; }
        [field: SerializeField] public MilestoneRewardView CurrentMilestoneRewardView { get; private set; }
        [field: SerializeField] public Button CloseButton { get; private set; }
        [field: SerializeField] public Button CollectButton { get; private set; }
        [field: SerializeField] public Sprite LootedSprite { get; private set; }
        [field: SerializeField] public Sprite NotLootedSprite { get; private set; }
    }
}