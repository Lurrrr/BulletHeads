using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Text;
using Photon.Realtime;
using TMPro;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public class LobbyJoinSceneManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    [SerializeField]
    TMP_InputField InputRoomName;
    [SerializeField]
    TMP_Text textRoomList;
    [SerializeField]
    TMP_InputField InputPlayerName;




    void Start()
    {
        Debug.Log("网络是否已连接" + PhotonNetwork.IsConnected);

        //connection
        if (PhotonNetwork.IsConnected == false)
        {
            PhotonNetwork.LoadLevel("StartScence");
        }
        else
        {
            if (PhotonNetwork.CurrentLobby == null | !PhotonNetwork.InLobby)
            {
                PhotonNetwork.JoinLobby();
            }
            else
            {
                //UpdateRoomInfo(PhotonNetwork.);
            }
        }
    }
    private void Update()
    {


    }



    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"❌ 房间创建失败！错误码: {returnCode}, 原因: {message}");
    }

    public void ReenterLobby()
    {
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to master");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Successfully joined the lobby");
    }

    //Get room name and clean it
    public string GetRoomName()
    {
        string roomName = InputRoomName.text;
        return roomName.Trim();
    }

    public string GetPlayerName()
    {
        string playerName = InputPlayerName.text;
        return playerName.Trim();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Successfully join the room");
        PhotonNetwork.LoadLevel("RoomScene");
        Debug.Log("Room Created.");
        Debug.Log("现在的房间：" + PhotonNetwork.CurrentRoom);

    }

    public void OnClickJoinRoom()
    {
        string roomName = GetRoomName();
        string playerName = GetPlayerName();
        if (roomName.Length > 0)
        {
            if (playerName.Length > 0)
            {
                PhotonNetwork.JoinRoom(roomName);
                PhotonNetwork.LocalPlayer.NickName = playerName;
            }
            else
            {
                Debug.Log("Invalid Name");
            }
        }
        else
        {
            Debug.Log("Invalid room name");
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("failed join" + returnCode + "Reason" + message);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {

        Debug.Log("Room list有变化");
        Debug.Log("现在有的房间数量：" + roomList.Count);
        UpdateRoomInfo(roomList);
        ///Debug.Log("Roomlist updated");
    }

    public void OnClickButtonBack()
    {
        PhotonNetwork.LeaveLobby();
        PhotonNetwork.LoadLevel("StartScene");
    }


    public void UpdateRoomInfo(List<RoomInfo> roomList)
    {
        StringBuilder roomName = new StringBuilder();
        foreach (RoomInfo roomInfo in roomList)
        {
            if (roomInfo.IsOpen)
            {
                if (roomInfo.PlayerCount > 0)
                {
                    roomName.AppendLine("-> " + roomInfo.Name + "   " + roomInfo.PlayerCount.ToString() + "/" + roomInfo.MaxPlayers.ToString());
                    Debug.Log("成功添加");
                }
                Debug.Log("现在有的房间数量：" + roomList.Count);
            }
            else
            {
                if (roomInfo.PlayerCount > 0)
                {
                    roomName.AppendLine("->Locked   " + roomInfo.Name + "   " + roomInfo.PlayerCount + "/" + roomInfo.MaxPlayers.ToString());
                }
            }
        }
        textRoomList.text = roomName.ToString();
    }
}
