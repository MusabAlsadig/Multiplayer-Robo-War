using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlaceChanger : NetworkBehaviour
{
    public static List<CubeMover1> cubes = new List<CubeMover1>();

    [SerializeField] Vector3Int min = default, max = default;

    List<int> X = new List<int>(), Y = new List<int>(), Z = new List<int>();
    List<int> x = new List<int>(), y = new List<int>(), z = new List<int>();

    private void Awake()
    {


        for (int i = min.x; i < max.x; i++)
        {
            X.Add(i);
        }

        for (int i = min.y; i < max.y; i++)
        {
            Y.Add(i);
        }

        for (int i = min.z; i < max.z; i++)
        {
            Z.Add(i);
        }
        x = new List<int>(X);
        y = new List<int>(Y);
        z = new List<int>(Z);
    }

    private void Start()
    {

        if (!IsServer)
        {
            enabled = false;
        }
    }

    private void Update()
    {
        ChangePose();
    }


    void ChangePose()
    {
        for (int i = 0; i < cubes.Count;i++)
        {
            
            if (cubes[i].change)
            {
                cubes[i].change = false;


                int _x = x[Random.Range(0, x.Count)];
                int _y = -9;
                int _z = z[Random.Range(0, z.Count)];

                cubes[i].transform.position = new Vector3(_x, _y, _z);

                x.Remove(_x);
                y.Remove(_y);
                z.Remove(_z);

                if (x.Count == 0)
                {
                    x = new List<int>(X);
                    Debug.Log("x");
                }

                if (y.Count == 0)
                {
                    y = new List<int>(Y);
                    Debug.Log("y");
                }

                if (z.Count == 0)
                {
                    z = new List<int>(Z);
                    Debug.Log("z");
                }

            }
        }
    }
}
