using ServerSide;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class GunBase : NetworkBehaviour
{

    public string model;

    [Header("Proprties as gun")]
    [SerializeField] protected Bullet bullet;
    [SerializeField] private int inaccuracyDegrees = 3;
    [SerializeField]
    private ParticleSystem[] bulletParticleSystems;
    [SerializeField]
    private SimpleSoundPlayer sound;
    public int Range => bullet.maxDistance;

    [Header("Ammo")]
    [SerializeField] private int maxAmmo;
    [SerializeField] private int clipSize;
    public float reloadTime;
    /// <summary>
    /// current ammo on clip
    /// </summary>
    public int Ammo { get; private set; }
    /// <summary>
    /// all additional ammo on storage
    /// </summary>
    public int StoredAmmo { get; private set; }

    public bool CanReload => Ammo != clipSize && StoredAmmo > 0;

    [SerializeField] private Transform firePlace;
    [SerializeField] private GameObject hitParticle;

    public GameObject HitParticle => hitParticle;


    private Shoot shoot;
    private Transform cameraTransform;


    public void Setup(Shoot shoot, Transform cameraTransfor)
    {
        this.shoot = shoot;
        this.cameraTransform = cameraTransfor;
    }

    public void Refill()
    {
        StoredAmmo = maxAmmo;
        Ammo = clipSize;
    }

    public void Reload()
    {

        int dif = clipSize - Ammo;

        dif = Mathf.Clamp(dif, 0, StoredAmmo); // (on last ammo) make sure that you dont reload more than what you have

        StoredAmmo -= dif;
        Ammo += dif;

    }

    protected bool TryDecreaseAmmo()
    {
        if (Ammo <= 0)
            return false;

        Ammo--;

        return true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns> if it can fire or no</returns>
    public abstract bool TryStartShooting();
    
    public abstract void StopShooting();

    public void EmitParticleAndSound()
    {
        foreach (var particleSystem in bulletParticleSystems)
        {
            particleSystem.Emit(1);
            sound.Play();
        }
    }

    protected void Fire()
    {
        Fire(needAmmo: true, damageMultiplier: 1);
    }

    protected void Fire(bool needAmmo, float damageMultiplier)
    {
        if (needAmmo && !TryDecreaseAmmo())
        return;

        shoot.Fire((int)(damageMultiplier * bullet.maxDamage));

        //System.Random random = new System.Random();
        //Vector3 offset = new Vector3(random.Next(-inaccuracyDegrees, inaccuracyDegrees), random.Next(-inaccuracyDegrees, inaccuracyDegrees));
        //Vector3 bulletDirection;
        //Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        //if (Physics.Raycast(ray, out RaycastHit hitInfo, Range))
        //{
        //bulletDirection = hitInfo.point - firePlace.position;
        //}
        //else
        //{
        //Vector3 farestPoint = ray.GetPoint(Range);
        //bulletDirection = farestPoint - firePlace.position;
        //}

        //Vector3 currentEuler = Quaternion.LookRotation(bulletDirection).eulerAngles;
        //Quaternion rotation = Quaternion.Euler(currentEuler + offset);

        //Fire_ServerRpc(shoot.OwnerClientId, firePlace.position, bulletDirection);
    }

    [ServerRpc]
    private void Fire_ServerRpc(ulong owner, Vector3 position,Vector3 direction)
    {
        NetworkObject bulletCopy = Instantiate(bullet, position, Quaternion.identity).NetworkObject;
        bulletCopy.transform.forward = firePlace.forward;
        bulletCopy.Spawn();

        bulletCopy.GetComponent<Bullet>().Fire(direction, owner);
    }


}
