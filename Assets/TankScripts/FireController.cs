using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireController : MonoBehaviourPun
{
    [SerializeField] private GameObject muzzleEnd, bullet;
    [SerializeField] private float timeToFire;
    [SerializeField] private Animator muzzleAnimation;

    private PhotonView photonView;
    /*[SerializeField]
    AudioClip clip, shooted;

    private AudioSource audio;*/
    private float lastFireTime;

    private void Start()
    {
        lastFireTime = Time.time;
        //audio = gameObject.GetComponent<AudioSource>();
        photonView = transform.root.gameObject.GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (!photonView.IsMine) return;
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (Time.time - timeToFire >= lastFireTime)
            {
                //audio.PlayOneShot(clip);
                lastFireTime = Time.time;
                muzzleAnimation.Play("MuzzlePush");
                GameObject bulletForDestroying = PhotonNetwork.Instantiate(bullet.name, muzzleEnd.transform.position, muzzleEnd.transform.rotation);
            }
        }
    }

    
}
