using UnityEngine;
using System.Collections;

public class Elevator : MonoBehaviour {

	//Feed numberOfFloors variable of this class to move the elevator up and down (negative numbers to go down) One unit is exactly one floor.

	Animator exitAnimator;
	Vector3 target;
	[HideInInspector]
	public bool move = false;
	bool canBeOpened = true;
	bool atGate = false;
	bool moveInitiated = false;

	GameObject playersParent;
	public AudioClip[] elevatorDoorSound;
	public AudioClip elevatorMoving;

	[HideInInspector]
	public int numberOfFloors = 0;
	[HideInInspector]
	public bool safeToOpen;
	[HideInInspector]
	public ElevatorExit currentGateStored;

	void Start() {
		playersParent = GameObject.FindGameObjectWithTag("Player").transform.parent.gameObject;
	}

	//Detect if the elevator is at gate
	void OnTriggerEnter(Collider other) {
		if (other.GetComponent<ElevatorExit>()) {
			atGate = true;
			currentGateStored = other.GetComponent<ElevatorExit>();
		}
	}

	//Check if left gate
	void OnTriggerExit(Collider other) {
		if (other.GetComponent<ElevatorExit>()) {
			atGate = false;
		}
	}

	void FixedUpdate() {
		//Run once only - open elevator and gate doors if parked
		if (safeToOpen && canBeOpened) {
			OpenDoors(currentGateStored);
			canBeOpened = false;
		}
		//Move to given floor (floor number set in coroutine)
		if (move) {
			float distance = Vector3.Distance(transform.position, target);
			transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime / 1.7f);

			//linear interpolation fix, also a little bump for the elevator when reaching the desired floor
			//FPS controller is parented to a dummy gameobject (father in demo scene) so rotation won't be reset when parenting to elevator.
			//This way, transition is smooth and unnoticed
			if (distance < 0.2F) {
				move = false;
				transform.position = target;
				atGate = true;
				canBeOpened = true;
				playersParent.transform.SetParent(null, true);
			}
		}
		//if not moving and at gate, safe to open
		else if (atGate) {
			safeToOpen = true;
		}
		//otherwise not safe to open doors
		else {
			safeToOpen = false;
		}
	}

	//Called from event (gui button press by user)
	public void Go() {
		StartCoroutine(StartElevator());
	}

	//return targetfloor int
	int targetFloor(int numberOfFloors) {
		return numberOfFloors * 4;
	}

	//Animates gate doors and elevator doors together with delay
	public void CloseDoors(ElevatorExit currentGate) {
		exitAnimator = currentGate.GetComponent<Animator>();
		AudioSource.PlayClipAtPoint(elevatorDoorSound[Random.Range(0, elevatorDoorSound.Length)], transform.position);
		exitAnimator.SetBool(ElevatorExit.MAINDOOR_OPEN, false);
	}

	public void OpenDoors(ElevatorExit currentGate) {
		exitAnimator = currentGate.GetComponent<Animator>();
		AudioSource.PlayClipAtPoint(elevatorDoorSound[Random.Range(0, elevatorDoorSound.Length)], transform.position);
		exitAnimator.SetBool(ElevatorExit.MAINDOOR_OPEN, true);
	}

	//parent controller, wait until doors close and interpolate to the next level. set move to true, and safe to open to false
	IEnumerator StartElevator() {
		if (!numberOfFloors.Equals(0) && !moveInitiated) {
			moveInitiated = true;
			safeToOpen = false;
			CloseDoors(currentGateStored);
			playersParent.transform.SetParent(this.transform, true);
			yield return new WaitForSeconds(3.1f);
			AudioSource.PlayClipAtPoint(elevatorMoving, transform.position);
			target = new Vector3(transform.position.x, transform.position.y + targetFloor(numberOfFloors), transform.position.z);
			numberOfFloors = 0;
			move = true;
			moveInitiated = false;
		}
	}

	void OnGUI() {
		if (move) {
			GUI.Label(new Rect(10, Screen.height - 50, 200, 30), "Cruisin'...");
		}
	}
	                  
}
