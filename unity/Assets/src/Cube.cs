using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		var rigidbody = gameObject.GetComponent<Rigidbody>();
		var v = rigidbody.velocity;
		if (Input.GetKey(KeyCode.LeftArrow)) {
			v.x = -1;
		}
		if (Input.GetKey(KeyCode.RightArrow)) {
			v.x = 1;
		}
		rigidbody.velocity = v;
	}
}
