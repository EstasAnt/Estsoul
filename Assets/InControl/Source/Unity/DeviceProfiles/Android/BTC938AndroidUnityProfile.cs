namespace InControl.UnityDeviceProfiles
{
	// @cond nodoc
	[Preserve] [UnityInputDeviceProfile]
	public class BTC938AndroidUnityProfile : InputDeviceProfile
	{
		public override void Define()
		{
			base.Define();

			DeviceName = "BTC-938";
			DeviceNotes = "BTC-938 on Android";

			DeviceClass = InputDeviceClass.Controller;

			IncludePlatforms = new[] {
				"Android"
			};

			Matchers = new[] { new InputDeviceMatcher { NameLiteral = "BTC-938" } };

			ButtonMappings = new[] {
				new InputControlMapping {
					Name = "A",
					Target = InputControlType.Action1,
					Source = Button( 0 )
				},
				new InputControlMapping {
					Name = "B",
					Target = InputControlType.Action2,
					Source = Button( 1 )
				},
				new InputControlMapping {
					Name = "X",
					Target = InputControlType.Action3,
					Source = Button( 2 )
				},
				new InputControlMapping {
					Name = "Y",
					Target = InputControlType.Action4,
					Source = Button( 3 )
				},
				new InputControlMapping {
					Name = "L1",
					Target = InputControlType.LeftBumper,
					Source = Button( 4 )
				},
				new InputControlMapping {
					Name = "L2",
					Target = InputControlType.LeftTrigger,
					Source = Button( 6 )
				},
				new InputControlMapping {
					Name = "R1",
					Target = InputControlType.RightBumper,
					Source = Button( 5 )
				},
				new InputControlMapping {
					Name = "R2",
					Target = InputControlType.RightTrigger,
					Source = Button( 7 )
				},
				new InputControlMapping {
					Name = "Left Stick Button",
					Target = InputControlType.LeftStickButton,
					Source = Button( 8 )
				},
				new InputControlMapping {
					Name = "Right Stick Button",
					Target = InputControlType.RightStickButton,
					Source = Button( 9 )
				},
				new InputControlMapping {
					Name = "Select",
					Target = InputControlType.Select,
					Source = Button( 11 )
				},
				new InputControlMapping {
					Name = "Start",
					Target = InputControlType.Start,
					Source = Button( 10 )
				}
			};

			AnalogMappings = new[] {
				LeftStickLeftMapping( 0 ),
				LeftStickRightMapping( 0 ),
				LeftStickUpMapping( 1 ),
				LeftStickDownMapping( 1 ),

				RightStickLeftMapping( 2 ),
				RightStickRightMapping( 2 ),
				RightStickUpMapping( 3 ),
				RightStickDownMapping( 3 ),

				DPadLeftMapping( 4 ),
				DPadRightMapping( 4 ),
				DPadUpMapping( 5 ),
				DPadDownMapping( 5 ),
			};
		}
	}
	// @endcond
}
