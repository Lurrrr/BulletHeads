using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartSceneManager : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        Debug.Log("网络是否已连接" + PhotonNetwork.IsConnected);
        if (!PhotonNetwork.IsConnected) 
        {
            PhotonNetwork.ConnectUsingSettings();
        }   
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected!!");
    }

    public void OnClickButtonCreateRoom() 
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.LoadLevel("LobbyCreateScene");
        }
        
    }
    public void OnClickButtonJoinRoom()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.LoadLevel("LobbyJoinScene");
        }
        
    }


}
