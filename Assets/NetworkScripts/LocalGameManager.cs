using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalGameManager : MonoBehaviour
{

    [SerializeField] private GameObject PlayerPrefab;
    [SerializeField] private GameObject environment;

    private GameObject Player;
    private PhotonView photonView;
    private Camera camera;

    void Start()
    {
        Player = PhotonNetwork.Instantiate(PlayerPrefab.name, new Vector3(0f, 10f, 0f), Quaternion.identity);
        photonView = Player.GetComponent<PhotonView>();
        if (photonView.Owner.IsMasterClient)
        {
            PhotonNetwork.Instantiate(environment.name, new Vector3(0f, 0f, 0f), Quaternion.Euler(0f, 0f, 0f));
            Debug.Log("Is Master");
        }


    }

    void Update()
    {
        
    }
}
