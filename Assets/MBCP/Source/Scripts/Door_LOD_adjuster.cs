using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Door_LOD_adjuster : MonoBehaviour {

	//Animation only moves LOD0, in order for LOD1 to move together with the door, this class silently adjusts its rotation.
	//Does it in the editor window when editing, too


	public Transform Door_LOD0;
	public Transform Door_LOD1;

	void Update() {
		if (Door_LOD0 != null && Door_LOD1 != null) {
			if (Door_LOD1.rotation != Door_LOD0.rotation) {
				Door_LOD1.rotation = Door_LOD0.rotation;
			}
			if (Door_LOD1.position != Door_LOD0.position) {
				Door_LOD1.position = Door_LOD0.position;
			}
		}
	}
}
