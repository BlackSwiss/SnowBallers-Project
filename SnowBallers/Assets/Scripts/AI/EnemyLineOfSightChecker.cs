using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SphereCollider))]
public class EnemyLineOfSightChecker : MonoBehaviour
{
    public SphereCollider Collider;
    public float FieldOfView = 90f;
    public LayerMask LineOfSightLayers;

    public delegate void GainSightEvent(Transform Target);
    public GainSightEvent OnGainSight;
    public delegate void LoseSightEvent(Transform Target);
    public GainSightEvent OnLoseSight;

    private Coroutine CheckForLineOfSightCoroutine;

    private void Awake()
    {
        Collider = GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!CheckLineOfSight(other.transform))
        {
            CheckForLineOfSightCoroutine = StartCoroutine(CheckForLineOfSight(other.transform));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        OnLoseSight?.Invoke(other.transform);
        if (CheckForLineOfSightCoroutine != null)
        {
            StopCoroutine(CheckForLineOfSightCoroutine);
        }
    }

    private bool CheckLineOfSight(Transform Target)
    {
        //get direction towards target
        Vector3 direction = (Target.transform.position - transform.position).normalized;

        //returns a number between -1 and 1, describing the direction in relation to a transforms
        //  forward. 1 meaning directly in front, -1 meaning directly behind
        float dotProduct = Vector3.Dot(transform.forward, direction);

        //if true then we do have line of sight
        if (dotProduct >= Mathf.Cos(FieldOfView))
        {
            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, Collider.radius, LineOfSightLayers))
            {
                OnGainSight?.Invoke(Target);
                return true;
            }
        }

        return false;
    }

    private IEnumerator CheckForLineOfSight(Transform Target)
    {
        WaitForSeconds Wait = new WaitForSeconds(0.5f);

        while(!CheckLineOfSight(Target))
        {
            yield return Wait;
        }
    }
}
