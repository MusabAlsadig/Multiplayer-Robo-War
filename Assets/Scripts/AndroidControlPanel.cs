using UnityEngine;

public class AndroidControlPanel : MonoBehaviour
{
    private void Awake()
    {
        if (!PlayerInput.IsOnSmartPhone)
            gameObject.SetActive(false);
    }
}

