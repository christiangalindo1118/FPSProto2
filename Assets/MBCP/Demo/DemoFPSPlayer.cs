using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class DemoFPSPlayer : MonoBehaviour {

	//This code is written for the demo scene to work with 'Player' tagged default Unity's charactercontroller prefab

	Elevator elevator;
	public Button goButton;
	public Text displayText;

	public GameObject canvas;

	void Start() {
		elevator = FindObjectOfType<Elevator>();
	}

	void OnControllerColliderHit(ControllerColliderHit other) {
		if (!other.collider.GetComponent<Elevator>()) {
			if (canvas.activeSelf)	canvas.SetActive(false);
			return;
		}
		else {
			canvas.SetActive(true);

			foreach ( Button b in FindObjectsOfType<Button>()) {
				if (elevator.move)		b.interactable = false;
				else 					b.interactable = true;
			}
		}
	}

	//Determines whether player is between a height range
	private bool isPlayerBetween(float min, float max) {
		if (min < 0 && max < 0) {
			if (this.transform.position.y < min && this.transform.position.y > max)
				return true;
			else 
				return false;
		}
		else {
			if (this.transform.position.y > min && this.transform.position.y < max)
				return true;
			else 
				return false;
		}
	}

	//TODO hardcoded height values to determine players location (Done this way so Elevator class is intact and you can use that your way)
	public void GoToArmory() {
		if (isPlayerBetween(0, 1.5f))				elevator.numberOfFloors = 1;
		else if (isPlayerBetween(-14.5f, -16.5f)) 	elevator.numberOfFloors = 5;
		else if (isPlayerBetween(15.5f, 17.5f))		elevator.numberOfFloors = -3;
		else 										elevator.numberOfFloors = 0;
		displayText.text = "Armory";
	}

	public void GoToInfirmary() {
		if (isPlayerBetween(0, 1.5f))				elevator.numberOfFloors = 4;
		else if (isPlayerBetween(-14.5f, -16.5f))	elevator.numberOfFloors = 8;
		else if (isPlayerBetween(4, 7))				elevator.numberOfFloors = 3;
		else 										elevator.numberOfFloors = 0;
		displayText.text = "Infirmary";
	}

	public void GoToHall() {
		if (isPlayerBetween(15.5f, 17.5f))			elevator.numberOfFloors = -4;
		else if (isPlayerBetween(-14.5f, -16.5f))	elevator.numberOfFloors = 4;
		else if (isPlayerBetween(4, 7))				elevator.numberOfFloors = -1;
		else 										elevator.numberOfFloors = 0;
		displayText.text = "Main hall room";
	}

	public void GoToPrison() {
		if (isPlayerBetween(0, 1.5f))				elevator.numberOfFloors = -4;
		else if (isPlayerBetween(15.5f, 17.5f))		elevator.numberOfFloors = -8;
		else if (isPlayerBetween(4, 7))				elevator.numberOfFloors = -5;
		else 										elevator.numberOfFloors = 0;
		displayText.text = "Prison";
	}

}
