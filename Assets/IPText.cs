using TMPro;
using UnityEngine;

public class IPText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txt;

    private static string currentAddress;
    private static bool isLocal;

    private void Awake()
    {
        if (isLocal)
        {
            txt.SetText($"No IP Address for solo testing");
        }
        else
            txt.SetText($"IP Address is <u>{currentAddress}</u>");
    }


    public static void SetIPAddress(string ipAddress)
    {
        isLocal = false;
        currentAddress = ipAddress;
    }

    public static void LocalMatch()
    {
        isLocal = true;
    }
}
