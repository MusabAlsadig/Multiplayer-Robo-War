using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCharactercontroller : MonoBehaviour
{
    [SerializeField]
    private int movementSpeed = 2;
    [SerializeField]
    private int rotationSpeed = 5;

    private float h, v;
    private float mouse_X, mouse_Y;

    Vector3 movement;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerInput input = PlayerInput.instance;
        h = input.movement.x;
        v = input.movement.y;


        mouse_X = input.mouseDelta.x;
        mouse_Y = input.mouseDelta.y;

        movement = transform.forward * v + transform.right * h;

        movement *= movementSpeed * Time.deltaTime;

        transform.position += movement;

        float _finalRotationSpeed = rotationSpeed * Time.fixedDeltaTime;
        transform.Rotate(0, mouse_X * _finalRotationSpeed, 0, Space.Self);

    }
}
