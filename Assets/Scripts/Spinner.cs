using UnityEngine;
using System.Collections;

public class Spinner : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(1000f*Vector3.up * Time.deltaTime, Space.World);
	}
}
