using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMotor : MonoBehaviour
{
    [SerializeField] private float force = 50f;
    [SerializeField] private float maxForce = 100;

    [FormerlySerializedAs("rotationSpeed")]
    [SerializeField] private float defaultRotationSpeed = 5f;

    [SerializeField] private float rotLimit = 45;

    [SerializeField] private Transform cam;

    private Vector3 movement;
    public static float rotationSpeedMultiplier;


    private Vector3 camEuler;
    private Vector3 playerEuler;


    private float h, v;
    private float mouse_X, mouse_Y;

    private Rigidbody rb;
    private AimAssist aimAssist;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        aimAssist = GetComponent<AimAssist>();
    }

    private void OnEnable()
    {
        Refresh();
    }

    private void Update()
    {
        PlayerInput input = PlayerInput.instance;
        h = input.movement.x;
        v = input.movement.y;

        mouse_X = input.mouseDelta.x;
        mouse_Y = input.mouseDelta.y;

        movement = transform.forward * v + transform.right * h;
    }

    private void FixedUpdate()
    {
        if (movement != Vector3.zero)
            PreformMovement();

        if (mouse_X != 0 || mouse_Y != 0)
            PreformRotation();
    }

    public void Refresh()
    {
        camEuler = cam.localEulerAngles;
        playerEuler = transform.eulerAngles;
    }

    private void PreformMovement()
    {
        movement.Normalize();
        rb.AddForce(force * Time.fixedDeltaTime * movement, ForceMode.Impulse);

        Debug.Log(rb.velocity.magnitude);

        if (rb.velocity.magnitude > maxForce)
            rb.velocity = movement * maxForce;
    }

    private void PreformRotation()
    {
        float _finalRotationSpeed = defaultRotationSpeed * rotationSpeedMultiplier * Time.fixedDeltaTime;

        playerEuler += new Vector3(0, mouse_X * _finalRotationSpeed, 0);

        Quaternion rotation = Quaternion.Euler(playerEuler);
        transform.rotation = rotation;


        //============== For cam
        
        camEuler += new Vector3(-mouse_Y * _finalRotationSpeed / 2, 0, 0); // (-) is for the invert
        camEuler.x = Mathf.Clamp(camEuler.x, -rotLimit, rotLimit);

        Quaternion cam_rotation = Quaternion.Euler(camEuler);
        cam.localRotation = cam_rotation;


        // do some aim assist

        if (aimAssist == null)
            return;

        // camera.forward is the direction the player will shoot at

        Vector3 aim = cam.forward;
        Vector3 assistedAim = aimAssist.GetAssistedAim(aim);

        transform.forward = new Vector3(aim.x, 0, aim.z);
        cam.transform.forward = assistedAim;
    }


        [Tooltip("Assist strength decay as angle to closest target increases")]
        [SerializeField] private AnimationCurve _aimAssistStrength;
        [Tooltip("Max angle for which aim is assisted. Beyond this value there is no assist")]
        [SerializeField][Range(0f, 90f)] private float _maxAngle = 30f;

        public Vector3 GetAssistedAim(Vector3 aim, ref List<Transform> targets)
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
