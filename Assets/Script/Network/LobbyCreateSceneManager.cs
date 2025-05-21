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

public class LobbyCreateSceneManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    [SerializeField]
    TMP_InputField InputRoomName;
    [SerializeField]
    TMP_Text textRoomList;
    [SerializeField]
    TMP_InputField InputPlayerName;
    Vector3 InputfieldTransform;
    private float updateTimer = 0f;


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
        }
    }
    private void Update()
    {

        
    }
    

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"❌ 房间创建失败！错误码: {returnCode}, 原因: {message}");
    }



    public void RefreashingRoomList()
    {
        updateTimer += Time.deltaTime;
        if (updateTimer >= 5f) // 每 5 秒刷新一次房间列表
        {
            Debug.Log("🔄 请求 Lobby 更新...");
            PhotonNetwork.JoinLobby();  // 强制重新加入 Lobby，触发 `OnRoomListUpdate()`
            updateTimer = 0f;
            Debug.Log("现在房间列表" +  PhotonNetwork.GetCustomRoomList(TypedLobby.Default, ""));
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
    public void OnClickCreateRoom()
    {
        string RoomName = GetRoomName();
        string playerName = GetPlayerName();
        if (PhotonNetwork.IsConnectedAndReady) 
        {
            if (RoomName.Length > 0)
            {
                if (playerName.Length > 0)
                {
                    RoomOptions roomOptions = new RoomOptions()
                    {
                        IsVisible = true,
                        IsOpen = true,
                        MaxPlayers = 2
                        
                        
                    };
                    PhotonNetwork.CreateRoom(RoomName, roomOptions);
                    PhotonNetwork.LocalPlayer.NickName = playerName;
                    
                    
                }
                else
                {
                    Debug.Log("Invalid Name");
                }
            }
            else
            {
                Debug.Log("Enter Name");
            }
        }
       
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
        StringBuilder roomName = new StringBuilder();
        Debug.Log("Room list有变化");
        Debug.Log("现在有的房间数量：" + roomList.Count);
        foreach (RoomInfo roomInfo in roomList) 
        {
            if (roomInfo.IsOpen)
            {
                if (roomInfo.PlayerCount > 0)
                {
                    roomName.AppendLine("RoomName " + roomInfo.Name + "   " + roomInfo.PlayerCount.ToString() + "/"+"2" );
                    Debug.Log("成功添加");
                }
                Debug.Log("现在有的房间数量：" + roomList.Count);
            }
            else
            {
                if (roomInfo.PlayerCount > 0)
                {
                    roomName.AppendLine("RoomLocked   " + roomInfo.Name + "   " + roomInfo.PlayerCount + "/" + "2");
                }
            }
        }
        textRoomList.text = roomName.ToString();
        ///Debug.Log("Roomlist updated");
    }


    public void OnClickButtonBack() 
    {
        PhotonNetwork.LeaveLobby();
        PhotonNetwork.LoadLevel("StartScene");
    }


}
