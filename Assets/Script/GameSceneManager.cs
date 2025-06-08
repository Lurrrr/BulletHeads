using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using TMPro;
using HashTable = ExitGames.Client.Photon.Hashtable;
using Unity.VisualScripting;
using ExitGames.Client.Photon.Encryption;
using UnityEngine.UI;

public class GameSceneManager : MonoBehaviourPunCallbacks
{

    private PhotonView PV;
    public GameObject BigCharacterSpawnPosition;
    public GameObject SmallCharacterSpawnPosition;
    public float TimeRemain = 5f;
    public GameObject spawnpoint;
    Coroutine coro;
    [SerializeField] TMP_Text TimeText;
    public GameObject gameoverpanel;
    public TMP_Text gamoverstatus;

    public TMP_Text player1name;
    public TMP_Text player2name;

    Player master;
    Player client;

    public string mastercharacter;
    public string clientcharacter;

    Slider masterSlider;
    Slider clientSlider;


    // Start is called before the first frame update
    void Start()
    {
        //关闭胜利页面
        gameoverpanel.SetActive(false);


        //获取必要组件
        PV = GetComponent<PhotonView>();
        masterSlider = GameObject.Find("血条_P1").GetComponent<Slider>();
        clientSlider = GameObject.Find("血条_P1 (1)").GetComponent<Slider>();

        Spawn();

        if (PhotonNetwork.IsMasterClient)
        {
            PV.RPC("StartCount", RpcTarget.All);
            //StartCoroutine("IESpawnEnemy");
        }

        //SetName();

    }


    // Update is called once per frame
    void Update()
    {
        //SetUI();
    }

    [PunRPC]
    public void StartCount()
    {
        coro = StartCoroutine("GameCount");
    }

    IEnumerator GameCount()
    {
        while (TimeRemain > 0.1f)
        {
            yield return new WaitForSeconds(1f);
            TimeRemain -= 1;
            TimeText.text = TimeRemain.ToString();
        }
        win();

    }

    private void SetName()
    {
        player1name = GameObject.Find("Text (TMP)_Master").GetComponent<TMP_Text>();

        player2name = GameObject.Find("Text (TMP)_Client").GetComponent<TMP_Text>();

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            HashTable currentroom = PhotonNetwork.CurrentRoom.CustomProperties;
            if (player.IsMasterClient)
            {
                master = player;
                player1name.text = master.NickName.ToString();
                mastercharacter = currentroom[$"{master.NickName}Character"].ToString() + "(Clone)";
            }
            else
            {
                client = player;
                player2name.text = client.NickName.ToString();
                clientcharacter = currentroom[$"{client.NickName}Character"].ToString() + "(Clone)";

            }
        }


    }

    private void SetUI()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (!player.IsMasterClient)
            {
                //设置非主机玩家血条
                GameObject playerobject = (GameObject)player.TagObject;
                if (playerobject?.GetComponent<BigPlayer>() != null)
                {
                    clientSlider.value = playerobject.GetComponent<BigPlayer>().HP;
                }
                else if (playerobject?.GetComponent<SmallPlayer>() != null)
                {
                    clientSlider.value = playerobject.GetComponent<SmallPlayer>().HP;
                }
            }
            else
            {
                //设置主机玩家血条
                GameObject playerobject = (GameObject)player.TagObject;

                if (playerobject?.GetComponent<BigPlayer>() != null)
                {
                    masterSlider.value = playerobject.GetComponent<BigPlayer>().HP;
                }
                else if (playerobject?.GetComponent<SmallPlayer>() != null)
                {
                    masterSlider.value = playerobject.GetComponent<SmallPlayer>().HP;
                }
            }
        }
    }

    private void UpdateUI()
    {
        if (mastercharacter == "BigCharacter")
        {

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
                PhotonNetwork.MasterClient.TagObject = PhotonNetwork.Instantiate($"Character/{character}", SmallCharacterSpawnPosition.transform.position, Quaternion.identity);
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

                    string character = PhotonNetwork.CurrentRoom.CustomProperties[$"{p2.NickName}Character"].ToString();
                    if (character == "BigCharacter")
                    {

                        player.TagObject = PhotonNetwork.Instantiate($"Character/{character}", BigCharacterSpawnPosition.transform.position, Quaternion.identity);
                    }
                    if (character == "SmallCharacter")
                    {
                        player.TagObject = PhotonNetwork.Instantiate($"Character/{character}", SmallCharacterSpawnPosition.transform.position, Quaternion.identity);
                    }

                }
            }


        }


    }

    [PunRPC]
    private void PunWin()
    {
        print("Win");
        //关闭时间
        Time.timeScale = 0;
        //暂停动画
        Animator[] animators = FindObjectsOfType<Animator>();
        foreach (var animator in animators)
        {
            animator.enabled = false;
        }
        //暂停物理计算
        Physics.autoSimulation = false;

        gameoverpanel.SetActive(true);
        gamoverstatus.text = "Win";


    }

    private void win()
    {
        Debug.Log("胜利");
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("主机执行到这");
            PV.RPC("PunWin", RpcTarget.All);
        }
    }

    [PunRPC]
    private void GameOver()
    {
        print("GameEnd");
        //关闭时间
        Time.timeScale = 0;
        //暂停动画
        Animator[] animators = FindObjectsOfType<Animator>();
        foreach (var animator in animators)
        {
            animator.enabled = false;
        }
        //暂停物理计算
        Physics.autoSimulation = false;

        gameoverpanel.SetActive(true);
        gamoverstatus.text = "Lose";
    }

    public void OnClickBack()
    {
        PhotonNetwork.Disconnect();
        PhotonNetwork.LoadLevel("StartScene");
    }


    IEnumerator IESpawnEnemy()
    {
        int i = 8;
        while (i >= 1)
        {
            yield return new WaitForSeconds(0.5f);
            PhotonNetwork.Instantiate("Enemy/ZigZagEnemy/ZigZagEnemy", spawnpoint.transform.position, Quaternion.identity);
            i -= 1;
        }
    }


    public override void OnRoomPropertiesUpdate(HashTable propertiesThatChanged)
    {
        //两个角色都死了
        if (propertiesThatChanged.ContainsKey("BigCharacterdead") || propertiesThatChanged.ContainsKey("SmallCharacterdead"))
        {
            HashTable currentRoom = PhotonNetwork.CurrentRoom.CustomProperties;
            if (currentRoom.ContainsKey("BigCharacterdead") && currentRoom.ContainsKey("SmallCharacterdead"))
            {
                if ((bool)currentRoom["BigCharacterdead"] == true && (bool)currentRoom["SmallCharacterdead"] == true)
                {
                    if (PhotonNetwork.IsMasterClient)
                    {
                        //输了
                        PV.RPC("GameOver", RpcTarget.All);
                    }
                }
            }
        }
    }
}
