using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructionManager : MonoBehaviour
{
    [SerializeField] private float collisionHealth;
    [SerializeField] private float explosionForce;
    [SerializeField] private GameObject destructedObjectPrefab;

    PhotonView photonView;

    private void Start()
    {
        transform.SetParent(null);
        photonView = gameObject.GetComponent<PhotonView>();
    }


    private void Update()
    {
        if (!photonView.IsMine) return;
        if (collisionHealth <= 0)
        {
            Destruction();
        }
    }

    void OnCollisionEnter(Collision collider)
    {
        if (!photonView.IsMine) return;

        if (collisionHealth < 0) return;
        Vector3 collisionForceInVector = collider.impulse / Time.fixedDeltaTime;

        float collisionForce = Mathf.Sqrt(Mathf.Pow(collisionForceInVector.x, 2) + Mathf.Pow(collisionForceInVector.y, 2) + Mathf.Pow(collisionForceInVector.z, 2));
        collisionHealth -= collisionForce;

        if (collisionHealth <= 0)
        {
            Destruction();
        }
    }

    private void Destruction ()
    {
        Transform spawnTransform = transform;
        PhotonNetwork.Destroy(gameObject);
        GameObject destructedObject = PhotonNetwork.Instantiate(destructedObjectPrefab.name, spawnTransform.position, spawnTransform.rotation);
        foreach(Transform child in destructedObject.transform)
        {
            child.gameObject.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, destructedObject.transform.position, 2f);
        }
    }

    public void TakeDamageFromBarrel(float damage)
    {
        if (!photonView.IsMine) return;
        collisionHealth -= damage;
    }
}
