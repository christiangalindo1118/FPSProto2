using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class ElevatorExit : MonoBehaviour {

	//This class is on the elevator gate gameobject. Elevator recognizes this component, that's how it finds if it's arrived at a gate.
	//Do not call the two functions below, as they only open the gate doors. Elevator's animation uses these functions from inside. (Check elevator animation keys)

	public Animator elevatorAnimator;
	public static string MAINDOOR_OPEN = "mainDoorOpen";
	public static string ELEVATOR_OPEN = "elevatorDoorOpen";

	void Start() {
		if (elevatorAnimator == null) {
			Debug.Log("Elevator's animator is not assigned for elevator main door (they won't open together)");
			Destroy(this);
		}
	}
		
	//TODO These are used externally (Elevator.cs calls them and they all open together. These two only open the wall gates)
	public void OpenElevatorDoor() {
		elevatorAnimator.SetBool(ELEVATOR_OPEN, true);
	}

	public void CloseElevatorDoor() {
		elevatorAnimator.SetBool(ELEVATOR_OPEN, false);
	}
}
