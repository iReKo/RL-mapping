using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MLAgents;


public class RLRDW_Agent4 : Agent
{
    public RLRDW_Academy academy;

    //現実空間とバーチャル空間関連の変数
    public Transform realPointTf, virtualPointTf;
    public Rigidbody realPointRb, virtualPointRb;
    public hapticsSensor2 realHapticsSensor, virtualHapticsSensor;
    public RealPoint4 realPoint;
    public Transform realDemo, virtualDemo;
    public Transform realObstacle, virtualObstacle;
    public Transform[] walls;
    public Vector3 outputV = Vector3.zero, outputAv = Vector3.zero;
    private Vector3 realPointInitPos, virtualPointInitPos;
    private Vector3 realLocalV = Vector3.zero, realLocalAv = Vector3.zero;
    private float[] realLocalDistances = null;

    //報酬関連の変数
    public float comfort_coef = 1, haptics_coef = 1;
    public float rewardOffset = 0.1f;
    public float interruption_thr = 0.05f;
    public float c_loss, h_loss;
    public float reward_sum = 0;
    private List<float> reward_log = Enumerable.Repeat<float>(0, 50).ToList();

    //経過時間の変数
    private float time;
    private float total_time = 0;


    public override void AgentReset()
    {
            realPointTf.localPosition = 3.3f * Vector3.right;
            float realLocalRotation = Random.Range(0, 360);
            realLocalRotation = 0;
            realPointTf.localRotation = Quaternion.Euler(0, realLocalRotation, 0);
            virtualPointTf.localPosition = 3.3f * Vector3.right;
            virtualPointTf.localRotation = realPointTf.localRotation;
            realPointRb.AddForce(Vector3.zero - realPointRb.velocity, ForceMode.VelocityChange);
            realPointRb.AddTorque(Vector3.zero - realPointRb.angularVelocity, ForceMode.VelocityChange);
            realLocalV = Vector3.zero;
            realLocalAv = Vector3.zero;
            realLocalDistances = null;
            realPoint.time = 0;
            realPoint.flag = Random.Range(0f, 1f);
            time = 0;

            reward_sum = 0;
            reward_log = Enumerable.Repeat<float>(0, 50).ToList();

        
    }

    public override void CollectObservations()
    {
        Vector3 localV = realPointTf.InverseTransformVector(realPointRb.velocity);
        AddVectorObs(localV.x);
        AddVectorObs(localV.z);
        AddVectorObs(realPointRb.angularVelocity.y);
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        outputV = new Vector3(vectorAction[0], 0, vectorAction[1]);
        outputAv = new Vector3(0, vectorAction[2], 0);
    }

    private void FixedUpdate()
    {
        if (time == 0)
        {
            realPointRb.velocity = Vector3.zero;
            realPointRb.angularVelocity = Vector3.zero;
            virtualPointRb.velocity = Vector3.zero;
            virtualPointRb.angularVelocity = Vector3.zero;
        }

        time += Time.deltaTime;
        if (time >= 8)
        {
            //Done();
        }

        float comfortLoss, hapticsLoss;

        Vector3 virtualLocalV = virtualPointTf.InverseTransformVector(virtualPointRb.velocity);
        Vector3 virtualLocalAv = virtualPointRb.angularVelocity;
        comfortLoss = -(Vector3.Magnitude(realLocalV - virtualLocalV) + Vector3.Magnitude(realLocalAv - virtualLocalAv)) * 0.02f;

        hapticsLoss = 0;
        for (int i = 0; i < 360; i++)
        {
            float rdis = realLocalDistances is null ? virtualHapticsSensor.distances[i]: realLocalDistances[i], vdis = virtualHapticsSensor.distances[i];
            if (rdis < 0.1f) rdis = 0.1f;
            if (vdis < 0.1f) vdis = 0.1f;
            hapticsLoss -= (Mathf.Pow(Mathf.Max(rdis, vdis) / Mathf.Min(rdis, vdis), 2) - 1) / Mathf.Pow(Mathf.Min(rdis, vdis), 0);
        }
        hapticsLoss /= 360f * 100;

        
        float reward = comfort_coef * comfortLoss + haptics_coef * hapticsLoss + rewardOffset;
        c_loss = comfort_coef * comfortLoss;
        h_loss = haptics_coef * hapticsLoss;
        SetReward(reward);
        reward_sum += reward;
        reward_log = reward_log.GetRange(0, reward_log.Count() - 1);
        reward_log.Insert(0, reward);

        realLocalV = realPointTf.InverseTransformVector(realPointRb.velocity);
        realLocalAv = realPointRb.angularVelocity;
        realLocalDistances = realHapticsSensor.distances;
    }
}
