using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CubeMover1 : MonoBehaviour
{

    [SerializeField] int speed = 5;
    [SerializeField] int distance = default;


    bool move;
    bool back;

    [HideInInspector] public bool change;

    public bool show;

    float position;

    private void Awake()
    {
        Invoke(nameof(Move), Random.Range(5, 6));
        PlaceChanger.cubes.Add(this);
    }

    private void Update()
    {

        if (move)
            PreformMovment(false);

        else if (back)
            PreformMovment(true);

        if (show)
        {
            Debug.Log(PlaceChanger.cubes.IndexOf(this));
        }
        
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
                Done();
            }


        }
    }

    void Stop()
    {
        move = false;
        Debug.Log("stoped moving");
    }

    void ChangePosition()
    {
        change = true;
    }

    void GetBack()
    {
        back = true;

        position = transform.position.y;

        Invoke(nameof(Move), 1);
    }

    void Done()
    {

        back = false;
        Debug.Log("done down");
    }

}
