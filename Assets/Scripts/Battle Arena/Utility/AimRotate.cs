using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimRotate : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    Quaternion startRot;
    Quaternion lookRot;

    Transform tgt;

    [SerializeField]
    float turnDuration;

    float counter;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        startRot = animator.transform.rotation;
        tgt = animator.GetComponent<WeaponItem>().toHit.transform;

        Vector3 relativePos = tgt.position - animator.transform.position;

        lookRot = Quaternion.LookRotation(relativePos);
        counter = 0;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.rotation = Quaternion.Slerp(animator.transform.rotation, lookRot, counter);
        //animator.transform.rotation = Quaternion.Slerp(startRot, rot, counter);
        counter += (1f / turnDuration) * Time.deltaTime - counter/10f;
        
    }

    float ZeroToMSine(float t, float v, float m)
    {
        return ((Mathf.Sin(t * v) + 1f)/2f) * m; //sines from (0-1) by speed v to the hieght of m
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    //perhaps should snap here just to make sure
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
