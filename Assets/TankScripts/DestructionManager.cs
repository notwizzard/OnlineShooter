using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructionManager : MonoBehaviour
{
    [SerializeField] private float collisionHealth;
    [SerializeField] private float explosionForce;
    [SerializeField] private GameObject destructedObjectPrefab;

    private void Start()
    {
        transform.SetParent(null);
    }


    private void Update()
    {
        if (collisionHealth <= 0)
        {
            Destruction();
        }
    }

    void OnCollisionEnter(Collision collider)
    {
        if (collider.gameObject.tag == "Bullet") return;

        if (collisionHealth < 0) return;

        CountDamage(collider.impulse);
    }

    private void Destruction ()
    {
        Transform spawnTransform = transform;
        Destroy(gameObject);
        GameObject destructedObject = Instantiate(destructedObjectPrefab, spawnTransform.position, spawnTransform.rotation);
        foreach(Transform child in destructedObject.transform)
        {
            child.gameObject.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, destructedObject.transform.position, 2f);
        }
    }

    public void TakeDamageFromBarrel(float damage)
    {
        collisionHealth -= damage;
    }

    public void TakeDamageFromBullet(Vector3 impulse)
    {
        CountDamage(impulse);
    }

    private void CountDamage(Vector3 impulse)
    {
        Vector3 collisionForceInVector = impulse / Time.fixedDeltaTime;

        float collisionForce = Mathf.Sqrt(Mathf.Pow(collisionForceInVector.x, 2) + Mathf.Pow(collisionForceInVector.y, 2) + Mathf.Pow(collisionForceInVector.z, 2));
        collisionHealth -= collisionForce;

        if (collisionHealth <= 0)
        {
            Destruction();
        }

    }
}
