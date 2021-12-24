using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireController : MonoBehaviour
{
    [SerializeField] private GameObject muzzleEnd, bullet;
    [SerializeField] private float timeToFire;
    [SerializeField] private Animator muzzleAnimation;

    /*[SerializeField]
    AudioClip clip, shooted;

    private AudioSource audio;*/
    private float lastFireTime;

    private void Start()
    {
        lastFireTime = Time.time;
        //audio = gameObject.GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (Time.time - timeToFire >= lastFireTime)
            {
                //audio.PlayOneShot(clip);
                lastFireTime = Time.time;
                muzzleAnimation.Play("MuzzlePush");
                GameObject bulletForDestroying = Instantiate(bullet, muzzleEnd.transform.position, muzzleEnd.transform.rotation);
            }
        }
    }

}
