using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using TMPro;

public class GameSceneManager : MonoBehaviourPunCallbacks
{

    private PhotonView PV;
    public GameObject BigCharacterSpawnPosition;
    public GameObject SmallCharacterSpawnPosition;
    public float TimeRemain = 90f;
    Coroutine coro;
    TMP_Text TimeText;
    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        if (PhotonNetwork.IsMasterClient)
        {
            PV.RPC("StartCount", RpcTarget.All);

        }
        Spawn();
        TimeText = GameObject.Find("Text (TMP)_Time").GetComponent<TMP_Text>();
    }


    // Update is called once per frame
    void Update()
    {

    }

    [PunRPC]
    public void StartCount()
    {
        coro = StartCoroutine("GameCount");
    }

    IEnumerator GameCount()
    {
        while(TimeRemain>0.1f)
        {
            yield return new WaitForSeconds(1f);
            TimeRemain -= 1;
            TimeText.text = TimeRemain.ToString();
        }
        
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


    private void SpawnEnemy()
    {

    }
}
