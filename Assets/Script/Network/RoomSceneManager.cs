using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Realtime;
using HashTable = ExitGames.Client.Photon.Hashtable;
using System.Diagnostics;
public class RoomSceneManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    TMP_Text RoomName;
    [SerializeField]
    TMP_Text PlayerList;
    [SerializeField]
    Button ButtonStartGame;
    [SerializeField]
    Button ButtonReadyGame;
    [SerializeField]
    bool RoomIsReady;


    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.CurrentRoom == null)
        {
            PhotonNetwork.LoadLevel("LobbyRoomScene");
        }
        else
        {
            RoomName.text = PhotonNetwork.CurrentRoom.Name;
            UpdatePlayerList();
        }
        ButtonStartGame.gameObject.SetActive(PhotonNetwork.IsMasterClient);
    }
    private void Update()
    {
        if (PhotonNetwork.CurrentRoom != null && PhotonNetwork.InRoom)
        {
            SetRoomOpenorNot();
        }
    }

    public void SetRoomOpenorNot() 
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
        else
        {
            PhotonNetwork.CurrentRoom.IsOpen = true;
        }
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        ButtonStartGame.interactable = PhotonNetwork.IsMasterClient;
    }

    public void UpdatePlayerList()
    {
        StringBuilder playerList = new StringBuilder();
        foreach (var keyval in PhotonNetwork.CurrentRoom.Players)
        {
            playerList.AppendLine(keyval.Value.NickName.ToString());
        }
        PlayerList.text = playerList.ToString();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerList(); 
    }

    public void OnClickLeaveRoom()
    {

        PhotonNetwork.LeaveRoom(this);


    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel("StartScene");
    }

    public void OnClickStartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("CharacterPickScene");
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
        

    }

}
