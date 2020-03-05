using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealPoint4 : MonoBehaviour
{
    Rigidbody rb;
    hapticsSensor2 haptics;

    public bool control = false;
    public bool show_demo = true;
    public RLRDW_Agent4 agent;
    public float max_velocity = 3;
    public Transform local;
    public float flag = 0;
    public float time = 0;
    float rad;

    private Vector3 satellite, stellar;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        haptics = GetComponent<hapticsSensor2>();
        rb.maxAngularVelocity = 100;

        stellar = local.position;

        time = 0;
        rad = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //print(string.Format("realPointV:{0}", rb.velocity));
        //print(string.Format("realPointAv:{0}", rb.angularVelocity));

        Vector3 targetV = Vector3.zero, targetAv = Vector3.zero;

        if (control)
        {
            targetV = 5 * new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            targetAv = 1 * new Vector3(0, Input.GetAxis("Rotation"), 0);
            rb.AddForce(transform.TransformVector(targetV) - rb.velocity, ForceMode.VelocityChange);
            rb.AddTorque(targetAv - rb.angularVelocity, ForceMode.VelocityChange);
        }
        
        else
        {
            if (time == 0)
            {
                float randomFloat = Random.onUnitSphere.x;
                rb.AddTorque(2 * new Vector3(0, randomFloat, 0) - rb.angularVelocity, ForceMode.VelocityChange);
            }

            rb.AddForce(15 * Vector3.Normalize(satellite - transform.position));

            if (time > 4)
            {
                rb.AddForce(20 * Vector3.Normalize(stellar - transform.position));
            }
            if (time > 10)
            {
                time = 0;
            }
            

            rad += flag > 0.5 ? Time.deltaTime / 2 : -Time.deltaTime / 2;
            if (Mathf.Abs(rad) > 2 * Mathf.PI) rad = 0;

            satellite = local.position + 10 * Mathf.Max(local.localScale.x, local.localScale.z) * Mathf.Sqrt(2) * new Vector3(Mathf.Cos(rad), 0, Mathf.Sin(rad));
        }

        time += Time.deltaTime;


        if (rb.velocity.magnitude > max_velocity)
        {
            rb.AddForce(max_velocity * rb.velocity.normalized - rb.velocity, ForceMode.VelocityChange);
        }
    }
}
