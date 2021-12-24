using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenCubeDisappearing : MonoBehaviour
{
    [SerializeField] private float maxLifeTime, minLifeTime;
    [SerializeField] private string animationName;
     
    void Start()
    {
        StartCoroutine("Disappearing");
    }

    private IEnumerator Disappearing ()
    {
        float lifeTime = Random.Range(minLifeTime, maxLifeTime);
        yield return new WaitForSeconds(lifeTime);
        gameObject.GetComponent<Animator>().Play(animationName);
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
