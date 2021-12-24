using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverMotor : MonoBehaviour
{
    [SerializeField] private float hoverForce = 65f;
    [SerializeField] private float normalHoverHeight = 1f;
    [SerializeField] private float raisedHoverHeight = 2f;
    [SerializeField] private float timeToRaise = 5f;
    [SerializeField] private float RaiseTime = 3f;
    [SerializeField] private Transform centerOfMass;

    [SerializeField] private Transform[] hovers;

    private Rigidbody tankRigidbody;
    private bool isRaised = false;
    private float hoverHeight;
    private float raisedPastTime = 0;

    private void Awake ()
    {
        tankRigidbody = GetComponent<Rigidbody>();
        tankRigidbody.centerOfMass = centerOfMass.localPosition;
        hoverHeight = normalHoverHeight;
    }

    private void Update()
    {
        Raise();
    }

    private void FixedUpdate ()
    {
        for (int i = 0; i < hovers.Length; i ++)
        {
            Ray ray = new Ray(hovers[i].position, -transform.up);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, hoverHeight))
            {
                float proportionalHeight = (hoverHeight - hit.distance) / hoverHeight;
                Vector3 appliedHoverForce = Vector3.up * proportionalHeight * hoverForce;
                tankRigidbody.AddForce(appliedHoverForce, ForceMode.Acceleration);
            }
        }
    }

    private void Raise()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isRaised = true;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isRaised = false;
        }
        if (!isRaised)
        {
            raisedPastTime += Time.deltaTime / timeToRaise;
            raisedPastTime = Mathf.Clamp(raisedPastTime, 0f, 1f);
            hoverHeight = normalHoverHeight;
        }
        if (isRaised)
        {
            raisedPastTime -= Time.deltaTime / RaiseTime;
            raisedPastTime = Mathf.Clamp(raisedPastTime, 0f, 1f);
            if (raisedPastTime > 0.01f)
            {
                hoverHeight = raisedHoverHeight;
            }
            else
            {
                hoverHeight = normalHoverHeight;
                isRaised = false;
            }
        }
    }


}
