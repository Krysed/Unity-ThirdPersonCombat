using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Cinemachine;
using UnityEngine;

public class Targeter : MonoBehaviour
{
    [SerializeField] private CinemachineTargetGroup cineTargerGroup;
    private List<Target> targets = new List<Target>();
    public Target CurrentTarget { get; private set; }
    public Camera mainCanera;
    private void Start()
    {
        mainCanera = Camera.main;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<Target>(out Target target)) { return; }

        targets.Add(target);
        target.OnDestroyed += RemoveTarget;
    }

    /*
        Remove target from the list when it not in range
     */
    private void OnTriggerExit(Collider other)
    {
        if(!other.TryGetComponent<Target>(out Target target)) { return; }

        RemoveTarget(target);
    }

    /*
        Lock on target
     */
    public bool SelectTarget()
    {
        if (targets.Count == 0) { return false; }

        Target closestTarget = null;
        float closestTargetDistance = Mathf.Infinity;

        //selecting closest target
        foreach (Target target in targets) 
        {
            //passing possition to a target
            Vector2 viewPos = mainCanera.WorldToViewportPoint(target.transform.position);

            //ignore targets that we can't see
            if(!target.GetComponentInChildren<Renderer>().isVisible) 
            {  
                continue; 
            }

            //how far away it is from the center
            Vector2 toCenter = viewPos - new Vector2(0.5f, 0.5f);
            //if that is closest to out closest target
            if(toCenter.sqrMagnitude < closestTargetDistance)
            {
                closestTarget = target;
                closestTargetDistance = toCenter.sqrMagnitude;
            }
        }

        if(closestTarget == null) { return false; }

        CurrentTarget = closestTarget;
        //Adding members to the targeting group with right weight and radius
        cineTargerGroup.AddMember(CurrentTarget.transform, 1f, 2f);

        return true;
    }    
    /*
        Cenceling Action
     */
    public void Cancel()
    {
        if(CurrentTarget == null) { return; }

        cineTargerGroup.RemoveMember(CurrentTarget.transform);
        CurrentTarget = null;
    }
    /*
        when target is Destroyed Remove it from the list
     */
    private void RemoveTarget(Target target)
    {
        if(CurrentTarget == target)
        {
            cineTargerGroup.RemoveMember(CurrentTarget.transform);
            CurrentTarget = null;
        }
        target.OnDestroyed -= RemoveTarget;
        targets.Remove(target);
    }
}
