using UnityEngine;
using UnityEngine.Timeline;

namespace Gameplay.Playables.CustomPlayableTracks
{
    [TrackClipType(typeof(WaitForInputClip))]
    [TrackBindingType(typeof(GameObject))]
    public class WaitForInputTrack : TrackAsset { }
}