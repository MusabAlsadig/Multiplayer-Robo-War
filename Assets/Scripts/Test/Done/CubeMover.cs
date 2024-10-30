using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CubeMover : MonoBehaviour
{

    [SerializeField] int speed = 5;
    [SerializeField] int distance = default;

    [SerializeField] Vector3Int minPosition = default,maxPosition = default;

    bool move;
    bool back;

    float position;


    private void Start()
    {
        Invoke(nameof(Move), Random.Range(5, 6));
    }

    private void Update()
    {

        if (move)
            PreformMovment(false);

        else if (back)
            PreformMovment(true);
        
    }

    void Move()
    {
        ChangePosition();
        
        Invoke(nameof(GetBack), Random.Range(3, 4));

        move = true;
        position = transform.position.y;

        Debug.Log("start moving");
    }

    void PreformMovment(bool invert)
    {
        int a = invert ? -1 : 1;

        transform.position += transform.up * distance * 2 * Time.deltaTime * a;

        if (!invert)
        {

            if ((position + distance) <= transform.position.y)
            {
                transform.position = new Vector3(transform.position.x, position + distance, transform.position.z);
                Stop();
            }

        }

        else
        {
            if ((position - distance) >= transform.position.y)
            {
                transform.position = new Vector3(transform.position.x, position - distance, transform.position.z);
                Stop();
            }


        }
    }

    void Stop()
    {
        move = false;
        back = false;
        Debug.Log("stoped moving");
    }

    void ChangePosition()
    {
        int x = Random.Range(minPosition.x, maxPosition.x);
        int y = Random.Range(minPosition.y, maxPosition.y);
        int z = Random.Range(minPosition.z, maxPosition.z);

        transform.position = new Vector3(x, y, z);

    }

    void GetBack()
    {
        back = true;

        position = transform.position.y;

        Invoke(nameof(Move), 1);
    }

}
