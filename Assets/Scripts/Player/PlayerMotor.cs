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


    [SerializeField] private Animator animator;
    [SerializeField] private Transform cam;

    private Vector3 movement;
    public static float rotationSpeedMultiplier;


    private Vector3 camEuler;
    private Vector3 playerEuler;


    private float h, v;
    private float mouse_X, mouse_Y;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

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
        animator.SetFloat("movement", v);

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
    }

}
