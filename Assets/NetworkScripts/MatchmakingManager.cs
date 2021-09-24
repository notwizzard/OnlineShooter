using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchmakingManager : MonoBehaviourPunCallbacks
{

    private void Start()
    {
        PhotonNetwork.NickName = PlayerPrefs.GetString("username", "Player") + Random.Range(1000000, 9999999).ToString("0000000");

        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = "0.1";
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        MenuManager.Instance.OpenMenu("title");
        Debug.Log("connected");
    }
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Tank");
    }

    public void CreateRoom ()
    {
        PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions { MaxPlayers = 3 });
    }

    public void JoinRandomRoom ()
    {
        PhotonNetwork.JoinRandomRoom();
    }
}
