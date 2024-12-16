using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Utility;

public class AimAssist : MonoBehaviour
{
    [Tooltip("Assist strength decay as angle to closest target increases")]
    [SerializeField] private AnimationCurve _aimAssistStrength;
    [Tooltip("Max angle for which aim is assisted. Beyond this value there is no assist")]
    [SerializeField][Range(0f, 90f)] private float _maxAngle = 30f;

    public Vector3 GetAssistedAim(Vector3 aim)
    {
        var targets = GetValidTargets();
        return GetAssistedAim(aim, targets);
    }


    private List<Transform> GetValidTargets()
    {
        List<Transform> targets = new List<Transform>();
        foreach (HealthSystem healthSystem in HealthSystem.allHealthSystems)
        {
            Vector3 direction = (healthSystem.transform.position - transform.position).normalized;

            if (PlayersHolder.IsMyAlly(healthSystem.OwnerClientId)) // ignore allies, you don't want to shoot them right?
                continue;

            if (Physics.Raycast(transform.position, direction) != healthSystem) // behind a wall or something
                continue;

            targets.Add(healthSystem.transform);
        }

        return targets;
    }

    private Vector3 GetAssistedAim(Vector3 aim, List<Transform> targets)
    {
        // if no targets then return aim un-changed
        if (targets.Count == 0) return aim;
        // identify the closest target in angle
        float minAngle = 180f;
        Vector3 desiredDirection = aim;
        foreach (Transform target in targets)
        {
            Vector3 directionToTarget = target.position - transform.position;
            directionToTarget.y = 0f;
            directionToTarget.Normalize();
            float angle = Vector3.Angle(aim, directionToTarget);
            if (angle < minAngle)
            {
                minAngle = angle;
                desiredDirection = directionToTarget;
            }
        }


        if (minAngle > _maxAngle) return aim;
        // return the assisted aim-direction based on the proximity to the target
        float assistStrength = _aimAssistStrength.Evaluate(minAngle / _maxAngle);
        return Vector3.Slerp(aim, desiredDirection, assistStrength);
    }
}
