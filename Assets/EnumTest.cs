using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnumTest : MonoBehaviour
{

    enum MyEnum
    {
        zero,
        one,
        two
    }

    [SerializeField] MyEnum @enum;
    
    private void Awake()
    {
        InvokeRepeating(nameof(Test), 0, 1);
    }

    [ContextMenu("Test")]
    void Test()
    {
        int m = @enum.GetHashCode();
        Debug.Log(m);
    }

}
