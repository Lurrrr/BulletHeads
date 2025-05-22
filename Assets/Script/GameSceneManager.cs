using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

public class GameSceneManager : MonoBehaviourPunCallbacks
{

    private PhotonView PV;
    public GameObject BigCharacterSpawnPosition;
    public GameObject SmallCharacterSpawnPosition;
    // Start is called before the first frame update
    void Start()
    {

        Spawn();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Spawn()
    {

        Debug.Log("pvismine");
        //主机玩家
        if (PhotonNetwork.IsMasterClient)
        {
            //获取主机玩家角色名称
            Debug.Log("p1玩家生成角色");
            string character = PhotonNetwork.CurrentRoom.CustomProperties[$"{PhotonNetwork.MasterClient.NickName}Character"].ToString();
            if (character == "BigCharacter")
            {
                PhotonNetwork.Instantiate($"Character/{character}", BigCharacterSpawnPosition.transform.position, Quaternion.identity);
            }
            if (character == "SmallCharacter")
            {
                PhotonNetwork.Instantiate($"Character/{character}", SmallCharacterSpawnPosition.transform.position, Quaternion.identity);
            }

        }
        else
        {
            Debug.Log("p2玩家生成角色");
            Player p2 = null;
            //获取其他玩家角色名称
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                if (!player.IsMasterClient)
                {
                    p2 = player;
                    Debug.Log(p2);

                }
            }

            string character = PhotonNetwork.CurrentRoom.CustomProperties[$"{p2.NickName}Character"].ToString();
            if (character == "BigCharacter")
            {
                PhotonNetwork.Instantiate($"Character/{character}", BigCharacterSpawnPosition.transform.position, Quaternion.identity);
            }
            if (character == "SmallCharacter")
            {
                PhotonNetwork.Instantiate($"Character/{character}", SmallCharacterSpawnPosition.transform.position, Quaternion.identity);
            }
        }


    }
}
