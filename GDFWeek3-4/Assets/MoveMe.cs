using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMe : MonoBehaviour {
    private GameObject cam;
    private int wincon = 0;

	// Use this for initialization
	void Start () {
        cam = GameObject.Find("Main Camera");
	}
	
	// Update is called once per frame
	void Update () {
        var x = Input.GetAxis("Horizontal") * 0.1f;
        var z = Input.GetAxis("Vertical") * 0.1f;

        if(!cam.GetComponent<Sensors>().LowPassFilterAccelerometer().y.AlmostEquals(0.0f, 0.0001f) && !cam.GetComponent<Sensors>().LowPassFilterAccelerometer().y.AlmostEquals(0.0f, -0.0001f))
            transform.position = Vector3.Lerp(transform.position, transform.position + transform.forward * (cam.GetComponent<Sensors>().LowPassFilterAccelerometer().y), Time.deltaTime * 5.0f);

        if (!cam.GetComponent<Sensors>().LowPassFilterAccelerometer().x.AlmostEquals(0.0f, 0.0001f) && !cam.GetComponent<Sensors>().LowPassFilterAccelerometer().x.AlmostEquals(0.0f, -0.0001f))
            transform.Rotate(transform.up, cam.GetComponent<Sensors>().LowPassFilterAccelerometer().x);

        transform.Translate(x, 0, z);
        cam.transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);

        if (Input.GetKeyDown(KeyCode.Space) || Input.touchCount > 0 || PAProximity.proximity == PAProximity.Proximity.NEAR)
        {
            if (++wincon >= 42)
                cam.GetComponent<NetworkManager>().LeaveRoom();
            else
                cam.GetComponent<NetworkManager>().SpawnActor("Projectile", transform.position + transform.forward * 2.0f, Quaternion.identity, 99.0f);
        }
    }
}
