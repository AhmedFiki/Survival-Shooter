using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunContainer : MonoBehaviour
{

    public GunSO currentGun;

    public int damage = 10;
    public float fireRate = 0.2f;
    public int magazineSize = 10;
    public float reloadTime = 1.0f;

    [SerializeField]
    private int currentAmmo;
    [SerializeField]
    private int ammoPool = 999;
    [SerializeField]
    private bool isReloading = false;
    private float nextShotTime = 0f;

    public float accuracy;
    public float recoil;

    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 10f;

    private Vector3 defaultAimDirection;

    public float maxAccuracy = 1.0f;
    public float accuracyLossPerShot = 0.1f;
    public float accuracyRecoveryRate = 0.1f;
    public float maxRecoilTime = 0.5f;
    private float currentRecoilTime;

    private float currentAccuracy;

    private void Start()
    {
        currentAmmo = magazineSize;
    }
    private void Update()
    {
        if (isReloading)
        {
            return;
        }

        if (Input.GetButton("Fire1") && Time.time >= nextShotTime)
        {
            nextShotTime = Time.time + 1f / fireRate;
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
    }
    
    private void Shoot()
    {
        if (currentAmmo <= 0)
        {
            Reload();
            return;
        }

        PlayShootingEffects();

        Vector3 adjustedAimDirection = CalculateAdjustedAimDirection();

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        //bullet.transform.right = adjustedAimDirection;


        ApplyRecoil();

        currentAmmo--;


    }

    void PlayShootingEffects()
    {


    }

    private void ApplyRecoil()
    {
        if (currentRecoilTime <= 0f)
        {
            currentRecoilTime = maxRecoilTime;
            Vector3 recoilDirection = -firePoint.up;
            transform.position += recoilDirection * recoil;
        }
    }

    private Vector3 CalculateAdjustedAimDirection()
    {
        Vector2 randomOffset = Random.insideUnitCircle * (1.0f - currentAccuracy);
        Vector3 adjustedAimDirection = defaultAimDirection + new Vector3(randomOffset.x, randomOffset.y, 0f);
        return adjustedAimDirection.normalized;
    }

    private void Reload()
    {
        if (currentAmmo < magazineSize)
        {
            StartCoroutine(ReloadCoroutine());
        }
    }

    private IEnumerator ReloadCoroutine()
    {
        isReloading = true;

        yield return new WaitForSeconds(reloadTime);


        if (ammoPool <= magazineSize)
        {
            currentAmmo = ammoPool;
            ammoPool = 0;
        }
        else
        {
            ammoPool -= magazineSize;
            currentAmmo = magazineSize;

        }



        isReloading = false;
    }
}

