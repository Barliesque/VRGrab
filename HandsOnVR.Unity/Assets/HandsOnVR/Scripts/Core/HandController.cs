﻿using System;
using UnityEngine;


namespace HandsOnVR
{

	[Flags]
	public enum Hand { Left = 1, Right = 2 }


	/// <summary>
	/// Match this transform to an Oculus Touch controller and monitor the states of its buttons.
	/// </summary>
	public class HandController : MonoBehaviour
	{
		[SerializeField] Hand _hand;
		public Hand Hand { get { return _hand; } }

		// Note: Open an Inspector panel in debug mode to monitor these ButtonState values at runtime

		public ButtonState Grip { get; private set; } = new ButtonState();
		public ButtonState Trigger { get; private set; } = new ButtonState();
		public ButtonState AorX { get; private set; } = new ButtonState();
		public ButtonState BorY { get; private set; } = new ButtonState();
		public ButtonState ThumbRest { get; private set; } = new ButtonState();


		Transform _xform;
		Transform Xform
		{
			get {
				if (_xform == null) _xform = GetComponent<Transform>();
				return _xform;
			}
		}

		public Vector3 Position => Xform.position;
		public Quaternion Rotation => Xform.rotation;


		private void Update()
		{
			var controller = _hand == Hand.Left ? OVRInput.Controller.LTouch : OVRInput.Controller.RTouch;
			if (OVRInput.IsControllerConnected(controller))
			{
				// Match this transform to the controller's position & rotation
				Xform.localPosition = OVRInput.GetLocalControllerPosition(controller);
				_xform.localRotation = OVRInput.GetLocalControllerRotation(controller);

				// Update the states of the Grip and Trigger analog buttons
				Grip.Update(OVRInput.Get(_hand == Hand.Left ? OVRInput.RawAxis1D.LHandTrigger : OVRInput.RawAxis1D.RHandTrigger, controller));
				Trigger.Update(OVRInput.Get(_hand == Hand.Left ? OVRInput.RawAxis1D.LIndexTrigger : OVRInput.RawAxis1D.RIndexTrigger, controller));

				// Update the A/X button, interpreting a touch with a value that can be seen in the ButtonState's analog value
				if (OVRInput.Get(_hand == Hand.Left ? OVRInput.RawButton.X : OVRInput.RawButton.A, controller))
				{
					AorX.Update(1f);
				}
				else if (OVRInput.Get(_hand == Hand.Left ? OVRInput.RawTouch.X : OVRInput.RawTouch.A, controller))
				{
					AorX.Update(0.25f);
				}
				else
				{
					AorX.Update(0f);
				}

				// Update the B/Y button, interpreting a touch with a value that can be seen in the ButtonState's analog value
				if (OVRInput.Get(_hand == Hand.Left ? OVRInput.RawButton.Y : OVRInput.RawButton.B, controller))
				{
					BorY.Update(1f);
				}
				else if (OVRInput.Get(_hand == Hand.Left ? OVRInput.RawTouch.Y : OVRInput.RawTouch.B, controller))
				{
					BorY.Update(0.25f);
				}
				else
				{
					BorY.Update(0f);
				}

				// Update the state of the ThumbRest
				if (OVRInput.Get(_hand == Hand.Left ? OVRInput.RawTouch.LThumbRest : OVRInput.RawTouch.RThumbRest, controller))
				{
					ThumbRest.Update(1f);
				}
				else
				{
					ThumbRest.Update(0f);
				}

			}
		}

	}
}