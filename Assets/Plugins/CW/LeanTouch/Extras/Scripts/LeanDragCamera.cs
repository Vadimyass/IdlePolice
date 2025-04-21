using UnityEngine;
using Lean.Common;
using CW.Common;

namespace Lean.Touch
{
	/// <summary>This component allows you to move the current GameObject (e.g. Camera) based on finger drags and the specified ScreenDepth.</summary>
	[HelpURL(LeanTouch.HelpUrlPrefix + "LeanDragCamera")]
	[AddComponentMenu(LeanTouch.ComponentPathPrefix + "Drag Camera")]
	public class LeanDragCamera : MonoBehaviour
	{
		
		/// <summary>The method used to find fingers to use with this component. See LeanFingerFilter documentation for more information.</summary>
		public LeanFingerFilter Use = new LeanFingerFilter(true);

		/// <summary>The method used to find world coordinates from a finger. See LeanScreenDepth documentation for more information.</summary>
		public LeanScreenDepth ScreenDepth = new LeanScreenDepth(LeanScreenDepth.ConversionType.DepthIntercept);

		/// <summary>The movement speed will be multiplied by this.
		/// -1 = Inverted Controls.</summary>
		public float Sensitivity { set { sensitivity = value; } get { return sensitivity; } } [SerializeField] private float sensitivity = 1.0f;

		/// <summary>If you want this component to change smoothly over time, then this allows you to control how quick the changes reach their target value.
		/// -1 = Instantly change.
		/// 1 = Slowly change.
		/// 10 = Quickly change.</summary>
		public float Damping { set { damping = value; } get { return damping; } } [SerializeField] private float damping = -1.0f;

		/// <summary>This allows you to control how much momentum is retained when the dragging fingers are all released.
		/// NOTE: This requires <b>Damping</b> to be above 0.</summary>
		public float Inertia { set { inertia = value; } get { return inertia; } } [SerializeField] [Range(0.0f, 1.0f)] private float inertia;

		/// <summary>This allows you to set the target position value when calling the <b>ResetPosition</b> method.</summary>
		public Vector3 DefaultPosition { set { defaultPosition = value; } get { return defaultPosition; } } [SerializeField] private Vector3 defaultPosition;

		private Vector3 _maxBorder;
		private Vector3 _minBorder;
		
		[SerializeField]
		private Vector3 remainingDelta;

		private Vector3 _maxBorder2;
		private Vector3 _minBorder2;
		private float _zoomRatio;

		public void SetZoomRatio(float zoomRatio)
		{
			_zoomRatio = zoomRatio;
		}
		
		/// <summary>This method resets the target position value to the <b>DefaultPosition</b> value.</summary>
		[ContextMenu("Reset Position")]
		public virtual void ResetRotation()
		{
			remainingDelta = defaultPosition - transform.position;
		}

		/// <summary>This method moves the current GameObject to the center point of all selected objects.</summary>
		[ContextMenu("Move To Selection")]
		public virtual void MoveToSelection()
		{
			var center = default(Vector3);
			var count  = 0;

			foreach (var selectable in LeanSelectable.Instances)
			{
				if (selectable.IsSelected == true)
				{
					center += selectable.transform.position;
					count  += 1;
				}
			}

			if (count > 0)
			{
				var oldPosition = transform.localPosition;

				transform.position = center / count;

				remainingDelta += transform.localPosition - oldPosition;

				transform.localPosition = oldPosition;
			}
		}

		public void SetBorders(Vector3 maxBorder, Vector3 minBorder, Vector3 maxBorder2, Vector3 minBorder2)
		{
			_minBorder2 = minBorder2;
			_maxBorder2 = maxBorder2;
			_maxBorder = maxBorder;
			_minBorder = minBorder;
		}
		
		/// <summary>If you've set Use to ManuallyAddedFingers, then you can call this method to manually add a finger.</summary>
		public void AddFinger(LeanFinger finger)
		{
			Use.AddFinger(finger);
		}

		/// <summary>If you've set Use to ManuallyAddedFingers, then you can call this method to manually remove a finger.</summary>
		public void RemoveFinger(LeanFinger finger)
		{
			Use.RemoveFinger(finger);
		}

		/// <summary>If you've set Use to ManuallyAddedFingers, then you can call this method to manually remove all fingers.</summary>
		public void RemoveAllFingers()
		{
			Use.RemoveAllFingers();
		}

#if UNITY_EDITOR
		protected virtual void Reset()
		{
			Use.UpdateRequiredSelectable(gameObject);
		}
#endif

		protected virtual void Awake()
		{
			Use.UpdateRequiredSelectable(gameObject);
		}

		protected virtual void LateUpdate()
		{
			// Get the fingers we want to use
			var fingers = Use.UpdateAndGetFingers();

			// Get the last and current screen point of all fingers
			var lastScreenPoint = LeanGesture.GetLastScreenCenter(fingers);
			var screenPoint = LeanGesture.GetScreenCenter(fingers);

			// Get the world delta of them after conversion
			var worldDelta = ScreenDepth.ConvertDelta(lastScreenPoint, screenPoint, gameObject);

			// Store the current position
			var oldPosition = transform.position;

			var newPosition = transform.position - worldDelta * sensitivity;

			var currentMinBorder = new Vector3(_minBorder.x  + (_minBorder2.x - _minBorder.x) * _zoomRatio, 0, _minBorder.z  + (_minBorder2.z - _minBorder.z) * _zoomRatio);
			var currentMaxBorder = new Vector3(_maxBorder.x  + (_maxBorder2.x - _maxBorder.x) * _zoomRatio, 0, _maxBorder.z  + (_maxBorder2.z - _maxBorder.z) * _zoomRatio);

			bool onBorder = false;
			
			if (newPosition.x < currentMinBorder.x)
			{
				onBorder = true;
				newPosition = new Vector3(currentMinBorder.x, transform.position.y, newPosition.z);
			}
			if (newPosition.z < currentMinBorder.z)
			{
				onBorder = true;
				newPosition = new Vector3(newPosition.x, transform.position.y, currentMinBorder.z);
			}
			if (newPosition.x > currentMaxBorder.x)
			{
				onBorder = true;
				newPosition = new Vector3(currentMaxBorder.x, transform.position.y, newPosition.z);
			}
			if (newPosition.z > currentMaxBorder.z)
			{
				onBorder = true;
				newPosition = new Vector3(newPosition.x, transform.position.y, currentMaxBorder.z);
			}
			
			
			// Pan the camera based on the world delta
			transform.position = newPosition;


			// Add to remainingDelta
			remainingDelta += transform.position - oldPosition;

			// Get t value
			var factor = CwHelper.DampenFactor(damping, Time.deltaTime);

			// Dampen remainingDelta
			var newRemainingDelta = Vector3.Lerp(remainingDelta, Vector3.zero, factor);

			var newNextPosition = oldPosition + remainingDelta - newRemainingDelta;
			if (newNextPosition.x < currentMinBorder.x || newNextPosition.z < currentMinBorder.z)
			{
				remainingDelta = Vector3.zero; 
				return;
			}
			if(newNextPosition.x > currentMaxBorder.x || newNextPosition.z > currentMaxBorder.z) 
			{
				remainingDelta = Vector3.zero; 
				return;
			}

			transform.position = newNextPosition;
			
			if (fingers.Count == 0 && inertia > 0.0f && damping > 0.0f)
			{
				newRemainingDelta = Vector3.Lerp(newRemainingDelta, remainingDelta, inertia);
			}

			// Update remainingDelta with the dampened value
			remainingDelta = newRemainingDelta;
			
			// Shift this position by the change in delta
		}
	}
}

#if UNITY_EDITOR
namespace Lean.Touch.Editor
{
	using UnityEditor;
	using TARGET = LeanDragCamera;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET), true)]
	public class LeanDragCamera_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			Draw("Use");
			Draw("ScreenDepth");
			Draw("sensitivity", "The movement speed will be multiplied by this.\n\n-1 = Inverted Controls.");
			Draw("damping", "If you want this component to change smoothly over time, then this allows you to control how quick the changes reach their target value.\n\n-1 = Instantly change.\n\n1 = Slowly change.\n\n10 = Quickly change.");
			Draw("inertia", "This allows you to control how much momentum is retained when the dragging fingers are all released.\n\nNOTE: This requires <b>Damping</b> to be above 0.");
			Draw("defaultPosition", "This allows you to set the target position value when calling the <b>ResetPosition</b> method.");
		}
	}
}
#endif