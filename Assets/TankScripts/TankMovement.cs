using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMovement : MonoBehaviour
{

    [SerializeField] private float movementSpeed;
    [SerializeField] private float normalStateMaxVelocity;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float maxTurnVelocity;

    [SerializeField] private GameObject backHoverEffect;
    [SerializeField] private GameObject leftHoverEffect, rightHoverEffect;
    [SerializeField] private Camera myCamera;

    private float nowMaxVelocity;
    private Rigidbody tankRigidbody;
    private float moveInput;
    private float turnInput;

    private PhotonView photonView;

    private void Awake ()
    {           
        tankRigidbody = GetComponent<Rigidbody>();
        photonView = gameObject.GetComponent<PhotonView>();
        nowMaxVelocity = normalStateMaxVelocity;
    }

    private void Update ()
    {
        myCamera.gameObject.SetActive(photonView.IsMine);
        if (!photonView.IsMine)
        {
            return;
        }
        moveInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");

        HoverEffectManager();

    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine) return;
        if (Mathf.Sqrt(Mathf.Pow(tankRigidbody.velocity.x, 2) + Mathf.Pow(tankRigidbody.velocity.z, 2)) < nowMaxVelocity)
            tankRigidbody.AddRelativeForce(0f, 0f, moveInput * movementSpeed);

        /* if (tankRigidbody.angularVelocity.y < maxTurnVelocity)
             tankRigidbody.AddTorque(0f, turnSpeed * turnInput, 0f);*/

        //transform.rotation = Quaternion.Euler(0f, transform.rotation.y * 3.14f, 0f);
        transform.Rotate(new Vector3(0f, turnSpeed * turnInput, 0f));

    }

    private void HoverEffectManager()
    {
        if (moveInput > 0)
        {
            backHoverEffect.SetActive(true);
        }
        else
        {
            backHoverEffect.SetActive(false);
        }

        if (turnInput > 0)
        {
            rightHoverEffect.SetActive(false);
            leftHoverEffect.SetActive(true);
        }
        else if (turnInput < 0)
        {
            rightHoverEffect.SetActive(true);
            leftHoverEffect.SetActive(false);
        }
        else
        {
            rightHoverEffect.SetActive(false);
            leftHoverEffect.SetActive(false);
        }
    }
}
