using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensors : MonoBehaviour { 
    private float AccelerometerUpdateInterval = 1.0f / 60.0f;
    private float LowPassKernelWidthInSeconds = 1.0f;

    private float LowPassFilterFactor;
    private Vector3 lowPassValue = Vector3.zero;
    
    private GameObject picframe;
    private WebCamTexture camTexture = null;

    // Initialization
    void Start()
    {
        LowPassFilterFactor = AccelerometerUpdateInterval / LowPassKernelWidthInSeconds;
        lowPassValue = Input.acceleration;

        PAProximity.onProximityChange += OnProximity;
        OnProximity(PAProximity.proximity);

        //camTexture = new WebCamTexture(); 
        //picframe = GameObject.Find("PicturePlane");
        //picframe.GetComponent<Renderer>().material.SetTexture(0, camTexture);
        //camTexture.Play();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnGUI()
    {
        GUILayout.Label("Magnetometer reading: " + Input.compass.rawVector.ToString());
        GUILayout.Label("Proximity reading: " + PAProximity.proximity.ToString());
        GUILayout.Label("Accelerometer reading: " + LowPassFilterAccelerometer().ToString());
        
        Touch[] myTouches = Input.touches;
        for (int i = 0; i < Input.touchCount; i++)
        {
            GUILayout.Label("Touch " + i + Input.GetTouch(i).phase.ToString());
        }
        //GUILayout.Label("Location Enabled? " + Input.location.isEnabledByUser);
        //StartCoroutine("StartIt");
        //GUILayout.Label("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
    }

    void OnProximity(PAProximity.Proximity arg)
    {
        if (arg == PAProximity.Proximity.NEAR)
        {
            //Debug.Log("Proximity Near");
        }
    }

    public Vector3 LowPassFilterAccelerometer()
    {
        lowPassValue = Vector3.Lerp(lowPassValue, Input.acceleration, LowPassFilterFactor);
        return lowPassValue;
    }

    IEnumerator StartIt()
    {
        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
            yield break;

        // Start service before querying location
        Input.location.Start();

        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            print("Timed out");
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            print("Unable to determine device location");
            yield break;
        }
        else
        {
            // Access granted and location value could be retrieved
            print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
        }

        // Stop service if there is no need to query location updates continuously
        Input.location.Stop();
    }

}
