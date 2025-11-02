using UnityEngine;
using System.Collections;

public class DemoWarningCanvas : MonoBehaviour {

	IEnumerator Start() {
		yield return new WaitForEndOfFrame();
		Destroy(this.gameObject);
	}
}
