using System;
using System.Collections;
using System.Collections.Generic;
using Thalmic.Myo;
using UnityEngine;
using Pose = Thalmic.Myo.Pose;

public class SimpleBallScript : MonoBehaviour
{
    [SerializeField] private ThalmicMyo myo;
    [SerializeField] private float poseChangeThreshold = 0.3f;
    [SerializeField] private float projectionForce = 1000;
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private GameObject hand;

    private GameObject m_ballObject = null;
    private bool m_ballAttached = false;

    private Thalmic.Myo.Pose m_prevPose;
    private Thalmic.Myo.Pose m_tempPose;
    private float m_poseTimer = 0;

    private Action<Thalmic.Myo.Pose> OnPoseChanged;

    void Start()
    {
        OnPoseChanged += OnFist;
    }

    void OnDestroy()
    {
        
    }

    void Update()
    {
        if (CatchPoseChange(out var newPose)) {
            OnPoseChanged?.Invoke(newPose);
            m_prevPose = newPose;

            Debug.Log($"Pose Changed to {newPose}");
        }

        if (m_ballObject != null && m_ballAttached) {
            m_ballObject.transform.position = hand.transform.position;
        } 
    }

    /// <summary>
    /// add extra step to prevent spikes in pose change 
    /// </summary>
    private bool CatchPoseChange(out Thalmic.Myo.Pose pose) {
        var currPose = myo.pose;

        if (m_prevPose != currPose)
        {
            if (m_tempPose != currPose)
            {
                m_poseTimer = 0;
                m_tempPose = currPose;
            }
            else
            {
                if (m_poseTimer > poseChangeThreshold)
                {
                    pose = currPose;
                    return true;
                }
            }
        }
        else {
            m_tempPose = m_prevPose;
        }

        pose = Thalmic.Myo.Pose.Unknown;
        m_poseTimer += Time.deltaTime;

        return false;
    }

    private void OnFist(Pose pose) {
        if (pose != Pose.Fist) {
            if (m_ballObject != null) {
                ShootBall();

                m_ballObject = null;
                m_ballAttached = false;
            }
            return;
        }

        if(m_ballObject == null)
        {
            m_ballObject = Instantiate(ballPrefab);
            m_ballAttached = true;
        }
    }

    private void ShootBall() { 
        var rb = m_ballObject.GetComponent<Rigidbody>();

        rb.AddForce(hand.transform.forward * projectionForce, ForceMode.Impulse);
    }
}
