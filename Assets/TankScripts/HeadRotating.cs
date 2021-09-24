using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadRotating : MonoBehaviour
{
    [SerializeField] private float turnSpeed;
    [SerializeField] private float verticalViewRange;
    [SerializeField] private GameObject head;

    private PhotonView photonView;

    float xRot = 0f, yRot = 0f;

    private void Start()
    {
        Cursor.visible = false;
        photonView = transform.root.gameObject.GetComponent<PhotonView>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!photonView.IsMine) return;
        xRot += -Input.GetAxis("Mouse Y") * turnSpeed;
        yRot += Input.GetAxis("Mouse X") * turnSpeed;

        xRot = Mathf.Clamp(xRot, -verticalViewRange, verticalViewRange);
        head.transform.rotation = Quaternion.Euler(xRot, yRot, 0f);
    }
}
