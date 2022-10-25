using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkManager : MonoBehaviour, IPunObservable
{

    private PhotonView photonView;


    // Start is called before the first frame update
    void Start()
    {
        photonView = PhotonView.Get(this);
    }


    /// <summary>
    /// When player clicks on a boardpiece inform photon to inform everyone to run method RPC_NotifySelectBoardPiece
    /// </summary>
    /// <param name="gameObject">the board piece clicked</param>
    public void NotifySelectBoardPiece(GameObject gameObject)
    {
        if((int)GetComponent<GameManager>().currentActivePlayer.id == PhotonNetwork.LocalPlayer.ActorNumber)
            photonView.RPC("RPC_NotifySelectBoardPiece", RpcTarget.All, gameObject.name);
    }

    /// <summary>
    /// this method is automatically called by Photon
    /// </summary>
    /// <param name="gameObjectName">the name of the board piece clicked</param>
    [PunRPC]
    public void RPC_NotifySelectBoardPiece(string gameObjectName)
    {
        GetComponent<GameManager>().SelectBoardPiece(GameObject.Find(gameObjectName));
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }



}
