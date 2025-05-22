using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using HashTable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;

public class CharacterPickSceneManager : MonoBehaviourPunCallbacks
{
    private PhotonView PV;
    [SerializeField] List<string> CharacterList = new List<string> { "BigPlayer", "SmallPlayer" };
    [SerializeField] int CurrentIndext = 0;
    public Button StartButton;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        //pv.RPC("SetUI",RpcTarget.All);
        SetUI();

        if (!PhotonNetwork.IsMasterClient)
        {
            StartButton.gameObject.SetActive(false);
        }
        else
        {
            StartButton.interactable = false;
        }
        //发送目前默认角色
        if (PV.IsMine)
        {
            HashTable character = new HashTable();
            character[$"{PhotonNetwork.LocalPlayer.NickName}Character"] = "BigCharacter";
            PhotonNetwork.CurrentRoom.SetCustomProperties(character);
            Debug.Log("查找本地玩家的角色：");
            Debug.Log(PhotonNetwork.CurrentRoom.CustomProperties[$"{PhotonNetwork.LocalPlayer.NickName}Character"]);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }



    private void SetUI()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            //房主端 设置自己名字
            GameObject player1UI = GameObject.Find("Player1");

            var playername = player1UI.transform.Find("Text (TMP)_PlayerName");
            TMP_Text text = playername.GetComponent<TMP_Text>();
            text.text = PhotonNetwork.MasterClient.NickName;
            //房主端 设置其他玩家名字
            GameObject player2UI = GameObject.Find("Player2");

            var playername2 = player2UI.transform.Find("Text (TMP)_PlayerName");
            Button buttonleft = player2UI.transform.Find("Button_Left").GetComponent<Button>();
            Button buttonright = player2UI.transform.Find("Button_Right").GetComponent<Button>();

            buttonleft.interactable = false;
            buttonright.interactable = false;


            TMP_Text text2 = playername2.GetComponent<TMP_Text>();
            foreach (var player in PhotonNetwork.PlayerList)
            {
                if (!player.IsMasterClient)
                {
                    text2.text = player.NickName;
                }
            }
        }
        else
        {
            //其他玩家端 设置主机名字
            GameObject player1UI = GameObject.Find("Player1");


            var playername = player1UI.transform.Find("Text (TMP)_PlayerName");
            Button buttonleft = player1UI.transform.Find("Button_Left").GetComponent<Button>();
            Button buttonright = player1UI.transform.Find("Button_Right").GetComponent<Button>();



            TMP_Text text = playername.GetComponent<TMP_Text>();
            text.text = PhotonNetwork.MasterClient.NickName;
            //主机玩家处按钮关闭
            buttonleft.interactable = false;
            buttonright.interactable = false;


            //其他玩家端 设置自己名字
            GameObject player2UI = GameObject.Find("Player2");
            var playername2 = player2UI.transform.Find("Text (TMP)_PlayerName");
            TMP_Text text2 = playername2.GetComponent<TMP_Text>();
            foreach (var player in PhotonNetwork.PlayerList)
            {
                if (!player.IsMasterClient)
                {
                    text2.text = player.NickName;
                }
            }
        }
    }


    public void OnClickStart()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("GameScene");
        }
    }

    public override void OnRoomPropertiesUpdate(HashTable propertiesThatChanged)
    {
        Player p2 = null;
        //获取非主机玩家
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (PhotonNetwork.MasterClient != player)
            {
                p2 = player;
                Debug.Log(p2.NickName);
            }
        }

        if (propertiesThatChanged.ContainsKey($"{PhotonNetwork.MasterClient.NickName}Character") || propertiesThatChanged.ContainsKey($"{p2.NickName}Character"))
        {
            //获取目前所有玩家的角色
            if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey($"{PhotonNetwork.MasterClient.NickName}Character") && PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey($"{p2.NickName}Character"))
            {
                string p1character = PhotonNetwork.CurrentRoom.CustomProperties[$"{PhotonNetwork.MasterClient.NickName}Character"].ToString();
                string p2character = PhotonNetwork.CurrentRoom.CustomProperties[$"{p2.NickName}Character"].ToString();

                if (p1character != p2character && PhotonNetwork.IsMasterClient)
                {
                    StartButton.interactable = true;
                    Debug.Log($"目前两个玩家角色：玩家1{p1character}   玩家2：{p2character}");
                }
                else
                {
                    StartButton.interactable = false;
                    Debug.Log($"目前两个玩家角色：玩家1{p1character}   玩家2：{p2character}");
                }
            }

        }
    }


}
