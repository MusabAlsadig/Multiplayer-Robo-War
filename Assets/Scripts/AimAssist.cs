using UnityEngine;
using UnityStandardAssets.Utility;

public class AimAssist : MonoBehaviour
{
    [SerializeField, Range(0.75f, 1f), Tooltip("higher number means lower angle, 0.75 = 45 degree, 1 = no assist")]
    private float minProduct;

    [SerializeField, Range(0.8f, 2f)]
    private float assistPower;

    [SerializeField]
    private Transform camera;
    [SerializeField]
    private PlayerMotor playerMotor;

    private void FixedUpdate()
    {
        if (TryFindTarget(out HealthSystem target))
            AssistAim(target);
    }

    private bool TryFindTarget(out HealthSystem bestTarget)
    {
        float highestProductTillNow = 0;
        bestTarget = null;

        foreach (HealthSystem healthSystem in HealthSystem.allHealthSystems)
        {
            Vector3 direction = (healthSystem.transform.position - transform.position).normalized;


            

            //if (distance.magnitude > range) // out of reach
            //    continue;

            

            float dotProduct = Vector3.Dot(transform.forward, direction);

            if (dotProduct < minProduct) // too much off the target to assist
                continue;

            if (PlayersHolder.IsMyAlly(healthSystem.OwnerClientId)) // ignore allies, you don't want to shoot them right?
                continue;

            if (Physics.Raycast(transform.position, direction) != healthSystem) // behind a wall or something
                continue;



            if (highestProductTillNow > dotProduct) // there is already a better target
                continue;


            // new best target
            highestProductTillNow = dotProduct;
            bestTarget = healthSystem;
        }


        return bestTarget != null;

    }

    private void AssistAim(HealthSystem target)
    {
        float factor = Mathf.Clamp01(assistPower * Time.fixedDeltaTime);
        Vector3 desiredDirection = (target.transform.position - transform.position).normalized;
        Vector3 desiredAngles = Quaternion.LookRotation(desiredDirection).eulerAngles;

        // rotate horizontally for this object
        Vector3 playerEulerAngles = transform.eulerAngles;
        playerEulerAngles.x =  Mathf.LerpAngle(desiredAngles.y, playerEulerAngles.y, factor);
        transform.rotation = Quaternion.Euler(playerEulerAngles);

        // rotate vertically for the child camera
        Vector3 cameraEulerAngles = camera.transform.eulerAngles;
        cameraEulerAngles.x = Mathf.LerpAngle(desiredAngles.x, cameraEulerAngles.x, factor);
        camera.transform.rotation = Quaternion.Euler(cameraEulerAngles);

        playerMotor.Refresh();
    }

}
