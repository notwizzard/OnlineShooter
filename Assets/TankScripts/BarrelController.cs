using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelController : MonoBehaviour
{
    [SerializeField] private float collisionHealth;
    [SerializeField] private float explosionForce;
    [SerializeField] private float explosionDamage, explosionImpulseForce;
    [SerializeField] private float explosionRadius;
    [SerializeField] private GameObject destructedObjectPrefab;
    [SerializeField] private GameObject explosionParticle;


    private void Update()
    {
        if (collisionHealth <= 0)
        {
            Explosion();
            Destruction();
        }
    }

    void OnCollisionEnter(Collision collider)
    {
        if (collisionHealth < 0) return;
        Vector3 collisionForceInVector = collider.impulse / Time.fixedDeltaTime;

        float collisionForce = Mathf.Sqrt(Mathf.Pow(collisionForceInVector.x, 2) + Mathf.Pow(collisionForceInVector.y, 2) + Mathf.Pow(collisionForceInVector.z, 2));
        collisionHealth -= collisionForce;

        if (collisionHealth <= 0)
        {
            Explosion();
            Destruction();
        }
    }


    private void Explosion()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (var hitCollider in hitColliders)
        {
            Ray ray = new Ray(transform.position, hitCollider.transform.position);
            float distanceToCollider = (transform.position - hitCollider.transform.position).magnitude;
            float hitsCount = Physics.RaycastAll(ray, distanceToCollider).Length;
            
            if (hitsCount < 2)
            {
                ExplosionDamage(hitCollider, distanceToCollider);
            }

            /*if (Physics.Raycast(ray, out hit, explosionRadius))
            {
                float proportionalHeight = (hoverHeight - hit.distance) / hoverHeight;
                Vector3 appliedHoverForce = Vector3.up * proportionalHeight * hoverForce;
                tankRigidbody.AddForce(appliedHoverForce, ForceMode.Acceleration);
            }*/
        }
    }
        

    private void ExplosionDamage(Collider collider, float distance)
    {
        float damage = explosionDamage * (explosionRadius - distance) / explosionRadius;
        Mathf.Clamp(damage, 0f, explosionDamage);

        if (collider.gameObject.tag.Substring(0, 4) == "Tank")
        {
            collider.transform.root.gameObject.GetComponent<TankHealthManager>().TakeDamageFromBarrel(damage);
            try
            {
                Rigidbody rb = collider.transform.root.gameObject.GetComponent<Rigidbody>();
                rb.AddExplosionForce(explosionImpulseForce, transform.position, explosionRadius);
            }
            catch
            {

            }
        }
        else if (collider.gameObject.tag == "DestructableObject")
        {
            collider.gameObject.GetComponent<DestructionManager>().TakeDamageFromBarrel(damage);
            try
            {
                Rigidbody rb = collider.gameObject.GetComponent<Rigidbody>();
                rb.AddExplosionForce(explosionImpulseForce, transform.position, explosionRadius);
            }
            catch
            {

            }
        }
        else if (collider.gameObject.tag == "Barrel")
        {
            Rigidbody rb = collider.gameObject.GetComponent<Rigidbody>();
            rb.AddExplosionForce(explosionImpulseForce, transform.position, explosionRadius);
            collider.gameObject.GetComponent<BarrelController>().TakeDamageFromBarrel(damage);
        }
        else
        {
            try
            {
                collider.gameObject.GetComponent<DestructionManager>().TakeDamageFromBarrel(damage);
            }
            catch
            {

            }
            try
            {
                Rigidbody rb = collider.gameObject.GetComponent<Rigidbody>();
                rb.AddExplosionForce(explosionImpulseForce, transform.position, explosionRadius);
            }
            catch
            {

            }
        }
    }

    public void TakeDamageFromBarrel(float damage)
    {
        collisionHealth -= damage;
        Debug.Log(damage);
    }

    private void Destruction()
    {
        Transform spawnTransform = transform;
        gameObject.SetActive(false);
        Destroyer(PhotonNetwork.Instantiate(explosionParticle.name, spawnTransform.position, spawnTransform.rotation), 5f);
        GameObject destructedObject = PhotonNetwork.Instantiate(destructedObjectPrefab.name, spawnTransform.position, spawnTransform.rotation);
        foreach (Transform child in destructedObject.transform)
        {
            child.gameObject.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, destructedObject.transform.position, 2f);
        }
    }
    private IEnumerator Destroyer(GameObject go, float time)
    {
        yield return new WaitForSeconds(time);
        PhotonNetwork.Destroy(go);
    }
}
