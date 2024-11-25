using UnityEngine;
using System.Collections;

//开头的场景移动
public class MovieCamera : MonoBehaviour {

    public float speed = 10;

    private float endZ = -20;

	// Update is called once per frame
	void Update () {
        if (transform.position.z < endZ) {
            transform.Translate( Vector3.forward*speed*Time.deltaTime);
        }
	}
}
