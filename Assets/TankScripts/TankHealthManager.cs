using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class TankHealthManager : MonoBehaviour
{
    [SerializeField] private float tankStartHealth;
    [SerializeField] private float headDamageRate, bodyDamageRate, roobarDamageRate;
    [SerializeField] private float globalRate;
    [SerializeField] private Image healthBar;
    [SerializeField] private float minDamageToShow;
    [SerializeField] private GameObject damageTMPObject;
    [SerializeField] private Transform[] damageUIPoints;
    [SerializeField] private Transform canvasForDamage;
    [SerializeField] private float damagePointSleepTime;

    private float tankHealth;

    private void Start()
    {
        tankHealth = tankStartHealth;
        canvasForDamage = GameObject.Find("CanvasForDamage").transform;
        healthBar = GameObject.Find("Bar").GetComponent<Image>();
    }

    private void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        /*foreach (ContactPoint contactedCollider in collision.contacts)
        {
            if (contactedCollider.thisCollider.gameObject.tag == "TankHead")
            {
                Debug.Log("Worked");
            }
        }*/
        if (collision.gameObject.tag == "Bullet")
            return;

        CountDamage(collision);
    }

    public void TakeDamageFromBullet(Collision collision)
    {
        CountDamage(collision);
    }

    private void CountDamage(Collision collision)
    {
        Vector3 vectorDamage = collision.impulse / Time.fixedDeltaTime;

        TakeDamage(vectorDamage, collision.contacts[0].thisCollider.gameObject.tag, collision.contacts[0].point);
    }

    public void TakeDamageFromBarrel(float damage)
    {
        tankHealth -= damage;
        DamageUI(damage, new Vector3(0f, 0f, 0f));
        HealthUI();
    } 


    private void TakeDamage(Vector3 vectorDamage, string bodyPart, Vector3 point)
    {
        float damage = CountFloatDamageFromVector(vectorDamage);
        
        if (bodyPart == "TankHead")
        {
            damage *= headDamageRate;
        }
        else if (bodyPart == "TankBody")
        {
            damage *= bodyDamageRate;
        }
        else if (bodyPart == "TankRooBar")
        {
            damage *= roobarDamageRate;
        }

        tankHealth -= damage;
        DamageUI(damage, point);

        HealthUI();
    }

    private float CountFloatDamageFromVector(Vector3 vectorDamage)
    {
        float damage = Mathf.Sqrt(Mathf.Pow(vectorDamage.x, 2) + Mathf.Pow(vectorDamage.y, 2) + Mathf.Pow(vectorDamage.z, 2));
        return damage * globalRate;
    }


    private void HealthUI ()
    {
        Mathf.Clamp(tankHealth, 0f, tankStartHealth);
        healthBar.fillAmount = tankHealth / tankStartHealth;
    }

    private void DamageUI(float damage, Vector3 point)
    {
        if (damage <= minDamageToShow) return;

        int numberOfNearestPoint = 0;
        float minPointDistance = 100;

        for(int i = 0; i < damageUIPoints.Length; i ++)
        {
            float distance = (damageUIPoints[i].position - point).magnitude;
            if (distance < minPointDistance && damageUIPoints[i].gameObject.activeSelf)
            {
                minPointDistance = distance;
                numberOfNearestPoint = i;
            }
        }

        GameObject instantiatedObject = Instantiate(damageTMPObject, damageUIPoints[numberOfNearestPoint].position, damageUIPoints[numberOfNearestPoint].rotation);
        instantiatedObject.transform.SetParent(canvasForDamage);
        StartCoroutine(DamagePointsActivator(numberOfNearestPoint));
        instantiatedObject.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = Mathf.Round(damage).ToString();
    }

    private IEnumerator DamagePointsActivator (int number)
    {
        damageUIPoints[number].gameObject.SetActive(false);
        yield return new WaitForSeconds(damagePointSleepTime);
        damageUIPoints[number].gameObject.SetActive(true);
    }
}
