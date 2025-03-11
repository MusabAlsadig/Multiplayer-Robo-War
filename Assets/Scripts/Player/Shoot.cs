using UnityEngine;
using Unity.Netcode;
using UI;

public class Shoot : NetworkBehaviour
{
    [SerializeField] private GunBase gun;

    [SerializeField] private Transform cameraTransform;


    [SerializeField] private KeyCode fire = KeyCode.M, reload = KeyCode.R;

    private HealthSystem healthSystem;

    private bool isReloading;
    private float reloadingTime;

    private AmmoPanel AmmoPanel => AmmoPanel.Instance;

    private void Awake()
    {
        gun.Setup(this, cameraTransform);

        reloadingTime = gun.reloadTime;

        gun.Refill();

        healthSystem = GetComponent<HealthSystem>();

        healthSystem.OnRevive += OnRevive;
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
            return;

        EventManager.AssignOnClick(EventManager.Controls.Fire, OnFireButtonClick);
        EventManager.AssignOnCancle(EventManager.Controls.Fire, OnFireButtonCancle);

        EventManager.Assign(EventManager.Controls.Reload, TryReload);
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner)
            return;


        EventManager.RemoveOnClick(EventManager.Controls.Fire, OnFireButtonClick);
        EventManager.RemoveOnCancle(EventManager.Controls.Fire, OnFireButtonCancle);
        EventManager.Remove(EventManager.Controls.Reload, TryReload);
    }



    private void TryReload()
    {
        if (!isReloading && gun.CanReload)
            StartReload();
    }

    private void OnFireButtonClick()
    {
        if (gun.TryStartShooting())
            StopReload();
    }

    private void OnFireButtonCancle()
    {
        gun.StopShooting();
    }

    private void OnRevive()
    {
        gun.Refill();
    }

    private void Update()
    {
        Local_CheckSight();
        AmmoPanel.UpdateAmmoText(gun.Ammo, gun.StoredAmmo);


        bool noAmmoOnClip = gun.Ammo <= 0;
        if (noAmmoOnClip && !isReloading)
        {
            OnFireButtonCancle();
        }

        if ((Input.GetKeyDown(reload) || noAmmoOnClip))
            TryReload();


        if (isReloading)
            PreformReloading();
    }

    public void Fire(int damage)
    {
        if (IsOwner)
        {
            Local_Fire();
            Fire_ServerRpc(damage);
        }
        else
            Debug.LogError("How did a non owner call this");
    }

    [ServerRpc]
    private void Fire_ServerRpc(int damage)
    {
        ShowBulletEffect_ClientRpc();

        if (!RayCast(out RaycastHit hit))
            return;

        
        if (!hit.transform.TryGetComponent(out HealthSystem healthSystem))
            return;

        if (hit.transform.TryGetComponent(out Player player)
            && PlayersHolder.AreAllies(player.OwnerClientId, OwnerClientId))
            return; // don't hit ally

        healthSystem.Server_TakeDamage(damage, OwnerClientId);

        Vector3 position = hit.transform.position;
        if (healthSystem.textSpawningPlace != null)
            position = healthSystem.textSpawningPlace.position;

        ShowDamage_ClientRpc(position, damage);
    }

    private void Local_Fire()
    {
        gun.EmitParticle();

        if (!RayCast(out RaycastHit hit))
            return;


        Instantiate(gun.HitParticle, hit.point, Quaternion.Euler(hit.normal), hit.transform);
    }

    [ClientRpc]
    private void ShowDamage_ClientRpc(Vector3 position, int damage)
    {
        if (IsOwner)
            Popups.Instance.ShowDamageDelt(damage, position);
    }

    [ClientRpc]
    private void ShowBulletEffect_ClientRpc()
    {
        if (IsOwner)
            return;

        gun.EmitParticle();
    }

    private void Local_CheckSight()
    {
        
        if (!RayCast(out RaycastHit hit))
        {
            PlayerUIUpdater.Instance.UpdateCrosshair(AllyOrEnemy.None);
            return;
        }


        if (hit.transform.TryGetComponent(out HealthSystem _))
        {
            if (hit.transform.TryGetComponent(out Player player)
                && PlayersHolder.IsMyAlly(player.OwnerClientId))
                PlayerUIUpdater.Instance.UpdateCrosshair(AllyOrEnemy.Ally);
            else
                PlayerUIUpdater.Instance.UpdateCrosshair(AllyOrEnemy.Enemy);
        }

        else
            PlayerUIUpdater.Instance.UpdateCrosshair(AllyOrEnemy.None);




    }

    #region Utilities
    private bool RayCast(out RaycastHit hit)
    {
        Vector3 orgin = cameraTransform.position + cameraTransform.forward;
        Vector3 direction = cameraTransform.forward;

        return Physics.Raycast(orgin, direction, out hit, gun.Range);
    }
    #endregion

    #region Reloading Methods

    private void StartReload()
    {
        isReloading = true;

        reloadingTime = gun.reloadTime;

        AmmoPanel.ShowReloading();
        Debug.Log("start loading");
    }

    private void StopReload()
    {
        isReloading = false;
        reloadingTime = gun.reloadTime;

        AmmoPanel.HideReloading();
    }

    private void PreformReloading()
    {
        AmmoPanel.UpdateReloadingStatus(reloadingTime);

        reloadingTime -= Time.deltaTime;


        if (reloadingTime <= 0)
        {
            gun.Reload();
            StopReload();
        }
    }

    #endregion

}
