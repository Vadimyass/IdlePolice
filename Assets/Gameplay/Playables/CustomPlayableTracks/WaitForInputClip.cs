using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Gameplay.Playables.CustomPlayableTracks
{
    [System.Serializable]
    public class WaitForInputClip : PlayableAsset, ITimelineClipAsset
    {
        public ClipCaps clipCaps => ClipCaps.None;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            return ScriptPlayable<WaitForInputPlayable>.Create(graph);
        }
    }
}