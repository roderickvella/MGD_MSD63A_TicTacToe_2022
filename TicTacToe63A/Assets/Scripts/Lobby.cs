using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class Lobby : MonoBehaviourPunCallbacks
{
    [Tooltip("Content Object")]
    public GameObject ScrollViewContent;

    [Tooltip("UI ROW Prefab containing the room details")]
    public GameObject RowRoom;

    [Tooltip("Player Name")]
    public GameObject InputPlayerName;

    [Tooltip("Room Name")]
    public GameObject InputRoomName;

    [Tooltip("Status Message")]
    public GameObject Status;

    [Tooltip("Button Create Room")]
    public GameObject BtnCreateRoom;

    [Tooltip("Panel Lobby")]
    public GameObject PanelLobby;

    [Tooltip("Panel waiting for other player")]
    public GameObject PanelWaitingForPlayer;


    // Start is called before the first frame update
    void Start()
    {

        PhotonNetwork.AutomaticallySyncScene = true;

        if (!PhotonNetwork.IsConnected)
        {
            //set the app version
            PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion = "1.0";

            //connect to the photon master-server
            PhotonNetwork.ConnectUsingSettings();
        }
	}

    public override void OnConnectedToMaster()
    {
        print("OnConnectedToMaster");
        //After we connected to Master server, join the lobby
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        print("OnFailedToConnectToPhoton Status Code:" + cause.ToString() + "Server Address:" + PhotonNetwork.ServerAddress);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
