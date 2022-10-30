using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(Animator))]

public class ClawIKController : MonoBehaviour
{

    protected Animator animator;
    protected int ikLayerIndex;
    protected float targetWeight;
    protected float weight;

    public string ikWeightLayer = "";
    public string ikWeightAnimation = "";
    public string ikAnimationParameter = "Attack";
    public Transform rightHandObj = null;
    public Transform leftHandObj = null;

    public Transform lookObj = null;

    void Start()
    {
        animator = GetComponent<Animator>();
        ikLayerIndex = animator.GetLayerIndex(ikWeightLayer);
    }

    //a callback for calculating IK
    void OnAnimatorIK()
    {
        if (animator)
        {
            float targetWeight = animator.GetFloat(ikAnimationParameter);
            weight = Mathf.MoveTowards(weight, targetWeight, Time.deltaTime / 0.2f);


            var clips = animator.GetCurrentAnimatorStateInfo(ikLayerIndex);
          
            /*foreach (var clip in clips)
            {

                Debug.LogWarning(clip.weight);
            }*/

            // Set the look target position, if one has been assigned
            if (lookObj != null)
            {
                animator.SetLookAtWeight(weight);
                animator.SetLookAtPosition(lookObj.position);
            }

            // Set the right hand target position and rotation, if one has been assigned
            if (rightHandObj != null)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, weight);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, weight);
                animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandObj.position);
                animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandObj.rotation);
            }

            // Set the right hand target position and rotation, if one has been assigned
            if (leftHandObj != null)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, weight);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, weight);
                animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandObj.position);
                animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandObj.rotation);
            }
        }


    }
}
