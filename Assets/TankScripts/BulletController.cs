using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private GameObject bulletParticle;

    PhotonView photonView;

    private void Start()
    {
        photonView = gameObject.GetComponent<PhotonView>();
        if (!photonView.IsMine) return;
        StartCoroutine(Destroyer(gameObject, 5f));
        gameObject.GetComponent<Rigidbody>().AddRelativeForce(0f, 0f, speed);
    }

    private void OnCollisionEnter(Collision coll)
    {
        if (!photonView.IsMine) return;
        /*PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("BulletDestroy", RpcTarget.MasterClient, gameObject, bulletParticle);*/
        StartCoroutine(Destroyer(PhotonNetwork.Instantiate(bulletParticle.name, transform.position, transform.rotation), 1f));
        StartCoroutine(Destroyer(gameObject, 0.02f));
    }

    private IEnumerator Destroyer(GameObject go, float time)
    {
        yield return new WaitForSeconds(time);
        if (go != null)
        {
            PhotonNetwork.Destroy(go);
        }
        
    }
}
