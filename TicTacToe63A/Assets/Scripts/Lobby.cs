using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

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

    List<RoomInfo> availableRooms = new List<RoomInfo>();

    UnityEngine.Events.UnityAction buttonCallback;

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

    public void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = 2;

        PhotonNetwork.JoinOrCreateRoom(InputRoomName.GetComponent<TMP_InputField>().text, roomOptions, TypedLobby.Default);
    }

    public override void OnCreatedRoom()
    {
        print("OnCreatedRoom");

        //set the player name
        PhotonNetwork.NickName = InputPlayerName.GetComponent<TMP_InputField>().text;
    }

    public override void OnJoinedRoom()
    {
        print("joined room");
        PanelLobby.SetActive(false);
        PanelWaitingForPlayer.SetActive(true);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        availableRooms = roomList;
        UpdateRoomList();
    }

    private void UpdateRoomList()
    {
        foreach(RoomInfo roomInfo in availableRooms)
        {
            GameObject rowRoom = Instantiate(RowRoom); //an instance of the prefab
            rowRoom.transform.parent = ScrollViewContent.transform;
            rowRoom.transform.localScale = Vector3.one;

            rowRoom.transform.Find("RoomName").GetComponent<TextMeshProUGUI>().text = roomInfo.Name;
            rowRoom.transform.Find("RoomPlayers").GetComponent<TextMeshProUGUI>().text = roomInfo.PlayerCount.ToString();

            buttonCallback = () => OnClickJoinRoom(roomInfo.Name);
            rowRoom.transform.Find("BtnJoin").GetComponent<Button>().onClick.AddListener(buttonCallback);


        }
    }

    public void OnClickJoinRoom(string roomName)
    {
        //set our player nickname
        PhotonNetwork.NickName = InputPlayerName.GetComponent<TMP_InputField>().text;
        //join the room
        PhotonNetwork.JoinRoom(roomName);
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        LoadMainGame();
    }

    private void LoadMainGame()
    {
        if(PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            //start the game by loading the MainGame scene
            PhotonNetwork.LoadLevel("MainGame"); //for this to work, make sure that this scene is added to the build settings
        }
    }

    private void OnGUI()
    {
        Status.GetComponent<TextMeshProUGUI>().text = "Status:" + PhotonNetwork.NetworkClientState.ToString();
    }
}
