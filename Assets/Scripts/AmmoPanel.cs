using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ammo_txt;
    [SerializeField] private TextMeshProUGUI reloadTimer_txt;

    public static AmmoPanel Instance {  get; private set; }

    private void Awake()
    {
        Instance = this;

        HideReloading();
    }

    public void UpdateAmmoText(int ammo, int currentAmmo)
    {
        ammo_txt.text = ammo.ToString() + "/" + currentAmmo;
    }

    public void ShowReloading()
    {
        reloadTimer_txt.gameObject.SetActive(true);
    }

    public void HideReloading()
    {
        reloadTimer_txt.gameObject.SetActive(false);
    }

    public void UpdateReloadingStatus(float currentTime)
    {
        reloadTimer_txt.text = "Reloading..." + currentTime.ToString("0.00");
    }

}
