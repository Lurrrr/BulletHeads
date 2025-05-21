using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using HashTable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;

public class UISlot : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject prefabPosition;
    public int CurrentIndext;
    [SerializeField]
    GameObject currentcharacter;
    [SerializeField] List<string> CharacterList = new List<string> { "BigPlayer", "SmallPlayer" };
    public GameObject smallcharacter;
    public GameObject bigcharacter;
    public PhotonView PV;

    // Start is called before the first frame update
    void Start()
    {
        prefabPosition = gameObject.transform.Find("CharacterPrefabPosition").gameObject;
        smallcharacter = gameObject.transform.Find("Image_SmallPlayer").gameObject;
        bigcharacter = gameObject.transform.Find("Image_BigPlayer").gameObject;

        smallcharacter.SetActive(false);
        bigcharacter.SetActive(true);

        PV = gameObject.GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {

    }




    public void OnClickLeft()
    {
        PV.RPC("SwitchLeft",RpcTarget.All);
        //把bigcharacter存入房间
        HashTable character = new HashTable();
        character[$"{PhotonNetwork.LocalPlayer.NickName}Character"] = "BigCharacter";
        PhotonNetwork.CurrentRoom.SetCustomProperties(character);
    }

    public void OnClickRight()
    {
        PV.RPC("SwitchRight", RpcTarget.All);
        //把smallcharacter存入房间
        HashTable character = new HashTable();
        character[$"{PhotonNetwork.LocalPlayer.NickName}Character"] = "SmallCharacter";
        PhotonNetwork.CurrentRoom.SetCustomProperties(character);
    }

    [PunRPC]
    public void SwitchLeft()
    {

        if (CurrentIndext > 0)
        {

            CurrentIndext -= 1;
            bigcharacter.SetActive(true);
            smallcharacter.SetActive(false);


        }
    }

    [PunRPC]
    public void SwitchRight()
    {
        if (CurrentIndext == 0)
        {
            CurrentIndext += 1;
            bigcharacter.SetActive(false);
            smallcharacter.SetActive(true);



        }
    }




   


}
