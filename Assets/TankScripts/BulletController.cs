using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private GameObject bulletParticle;

    private void Start()
    {
        Destroy(gameObject, 5f);
        gameObject.GetComponent<Rigidbody>().AddRelativeForce(0f, 0f, speed);
    }

    private void OnCollisionEnter(Collision collision)
    {        
        if (collision.gameObject.tag != "Player")
        {
            DestructionManager dm = collision.gameObject.GetComponent<DestructionManager>();
            if (dm)
            {
                dm.TakeDamageFromBullet(collision.impulse);
            }
        }
        else
        {
            TankHealthManager thm = collision.gameObject.GetComponent<TankHealthManager>();
            if (thm)
            {
                thm.TakeDamageFromBullet(collision);
            }
        }
        

        Destroy(Instantiate(bulletParticle, transform.position, transform.rotation), 1f);
        Destroy(gameObject, 0.02f);
    }
}
