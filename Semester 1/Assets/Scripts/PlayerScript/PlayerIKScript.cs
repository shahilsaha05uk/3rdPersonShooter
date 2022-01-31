using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerIKScript : MonoBehaviour
{
    protected Animator m_animator;
    
    public bool ikActive = false;
    public Transform rightHandObj = null;
    public Transform leftHandObj = null;

    public Vector3 rightPos;
    public Vector3 leftPos;

    [Header("Hand Weights")]
    [Range(0, 1)] public float rightHandWeight;
    [Range(0, 1)] public float leftHandWeight;


    [Header("Hand Parts")]
    public Transform rightHand;
    public Transform rightElbow;
    public Transform rightShoulder;
    [Space(4f)]
    public Transform leftHand;
    public Transform leftElbow;
    public Transform leftShoulder;

    public Camera cam;
    Ray ray;
    private void Start()
    {
        m_animator = GetComponent<Animator>();

        rightHandObj = null;
        leftHandObj = null;
    }

    private void Update()
    {
        try
        {
            rightPos = rightHandObj.position;
            leftPos = leftHandObj.position;

        }
        catch (System.Exception)
        {

        }


        if (rightPos == null || leftPos == null)
        {
            ikActive = false;
        }
        else
        {
            ikActive = true;
        }
    }

    private void OnAnimatorIK()
    {
        if (m_animator)
        {
            if (ikActive)
            {
                
                if (rightHandObj != null && leftHandObj != null)
                {
                    m_animator.SetIKPositionWeight(AvatarIKGoal.RightHand, rightHandWeight);
                    m_animator.SetIKRotationWeight(AvatarIKGoal.RightHand, rightHandWeight);

                    m_animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftHandWeight);
                    m_animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, leftHandWeight);


                    m_animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandObj.position);
                    m_animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandObj.rotation);

                    m_animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandObj.position);
                    m_animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandObj.rotation);
                }
            }

            else
            {
                m_animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0f);
                m_animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0f);

                m_animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0f);
                m_animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0f);

                m_animator.SetLookAtWeight(0f);
            }
        }
    }
}
