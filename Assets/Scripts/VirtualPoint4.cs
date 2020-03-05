using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualPoint4 : MonoBehaviour
{
    public RLRDW_Agent4 agent;
    public Transform realPointTf;
    public Rigidbody realPointRb;

    Vector3 targetV, targetAv;
    Rigidbody rb;
    float time = 0;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = 100;
    }

    void FixedUpdate()
    {
        Vector3 realLocalV = realPointTf.InverseTransformVector(realPointRb.velocity);
        Vector3 realLocalAv = realPointRb.angularVelocity;

        targetV = realLocalV + 10 * agent.outputV;
        targetAv = realLocalAv + 1 * agent.outputAv;

        
        rb.AddForce(transform.TransformVector(targetV) - rb.velocity, ForceMode.VelocityChange);
        rb.AddTorque(targetAv - rb.angularVelocity, ForceMode.VelocityChange);


        time += Time.deltaTime;
    }
}
