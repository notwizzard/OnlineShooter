using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageUIController : MonoBehaviour
{

    [SerializeField] float lifeTime;

    private Transform camera;
    

    void Start()
    {
        Destroy(gameObject, lifeTime);
        camera = Camera.allCameras[0].transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(camera);
        transform.Rotate(0f, 180f, 0f);
    }
}
