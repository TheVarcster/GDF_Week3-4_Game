using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : Photon.PunBehaviour {
    
    // Use this for initialization
    void Start () {
        PhotonNetwork.ConnectUsingSettings("alpha 0.1");
	}

	// Update is called once per frame
	void Update () {
    }

    void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected...");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {
        GameObject Player = PhotonNetwork.Instantiate("Character", Vector3.zero, Quaternion.identity, 0);
        Player.GetComponent<MoveMe>().enabled = true;
        //((MonoBehaviour)Player.GetComponent("Mo")).enabled = true;
    }

    public void SpawnActor(string name, Vector3 loc, Quaternion rot, float force)
    {
        GameObject Shot = PhotonNetwork.Instantiate(name, loc, rot, 0);
        
    }

    public void RemoveActor(GameObject target)
    {
        PhotonNetwork.Destroy(target);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.Disconnect();
    }

    public void OnPhotonRandomJoinFailed()
    {
        Debug.Log("Creating Room...");
        PhotonNetwork.CreateRoom(null);
    }

    public override void OnFailedToConnectToPhoton(DisconnectCause cause)
    {
        string errorMsg = "";
        switch (cause)
        {
            case DisconnectCause.DisconnectByServerUserLimit: errorMsg = "Server forced a disconnect"; break;
            case DisconnectCause.ExceptionOnConnect: errorMsg = "Local server not running"; break;
            case DisconnectCause.DisconnectByServerTimeout: errorMsg = "Timeout disconnect by server"; break;
            case DisconnectCause.DisconnectByServerLogic: errorMsg = "Server's send buffer full"; break;
            case DisconnectCause.Exception: errorMsg = "Unknown exception"; break;
            case DisconnectCause.InvalidAuthentication: errorMsg = "Rejected AppID"; break;
            case DisconnectCause.MaxCcuReached: errorMsg = "Too many active users"; break;
            case DisconnectCause.InvalidRegion: errorMsg = "Invalid Region"; break;
            case DisconnectCause.SecurityExceptionOnConnect: errorMsg = "Invalid security settings"; break;
            case DisconnectCause.DisconnectByClientTimeout: errorMsg = "Client Timeout disconnect"; break;
            case DisconnectCause.InternalReceiveException: errorMsg = "Socket error, check internet settings"; break;
            case DisconnectCause.AuthenticationTicketExpired: errorMsg = "Old Authentication Ticket"; break;
            default: errorMsg = "Unknown Exception" + System.Convert.ToInt32(cause).ToString(); break;
        }
        Debug.Log(errorMsg);
    }
}
