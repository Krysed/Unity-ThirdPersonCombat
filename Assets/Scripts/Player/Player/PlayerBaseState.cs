using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState : State
{
    protected PlayerStateMachine stateMachine;

    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }
    protected void Move(float deltaTime)
    {
        Move(Vector3.zero, deltaTime);
    }
    /*
        Movement method that applies gravity to a player movement 
     */
    protected void Move(Vector3 motion, float deltaTime)
    {
        stateMachine.Controller.Move((motion + stateMachine.ForceReceiver.Movement) * deltaTime);
    }
    /*
        Rotate Player Character to face the target
     */
    protected virtual void FaceTarget() 
    { 
        //make sure we have a target
        if(stateMachine.Targeter.CurrentTarget == null) { return; }
        //take the target and subtract our position
        Vector3 lookPos = stateMachine.Targeter.CurrentTarget.transform.position - stateMachine.transform.position;
        lookPos.y = 0f;

        //convert vector to a quartenion
        stateMachine.transform.rotation = Quaternion.LookRotation(lookPos);
    }
    protected void ReturnToLocomotion()
    {
        if(stateMachine.Targeter.CurrentTarget != null)
        {
            stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
        }
        else
        {
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
        }
    }
}
