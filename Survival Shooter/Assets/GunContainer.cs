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
    public float minAccurateDistance = 10.0f;
    public float maxAccurateDistance = 12.0f;

    private float currentAccuracy;
    public float recoilFactor = 1;
    Vector3 mousePosition;
    private void Start()
    {
        currentAmmo = magazineSize;
        currentAccuracy = maxAccuracy;
    }
    private void Update()
    {
        CalculateAccuracy();
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

    void CalculateAccuracy()
    {
        //Mouse distance accuracy

        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float distanceToMouse = Vector2.Distance(transform.position, mousePosition);



        if (distanceToMouse <= minAccurateDistance)
        {
            currentAccuracy = maxAccuracy;
        }
        else
        {

            float distanceFromMin = (distanceToMouse - minAccurateDistance);

            currentAccuracy = maxAccuracy - ((distanceToMouse - minAccurateDistance) / (maxAccurateDistance - minAccurateDistance)) ;

            currentAccuracy = Mathf.Clamp01(currentAccuracy); 
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
        float deviationAngle = Random.Range(0f, 50f); 
        Vector3 deviation = Quaternion.Euler(0f, 0f, deviationAngle) * adjustedAimDirection * (1.0f - currentAccuracy) * recoilFactor;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bulletController = bullet.GetComponent<Bullet>();

        if (bulletController != null)
        {

            bulletController.SetDirection(transform.right+ deviation);
        }

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

        }
    }

    private Vector3 CalculateAdjustedAimDirection()
    {
        Vector2 randomOffset = Random.insideUnitCircle * (1.0f - currentAccuracy) ;
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
    private void OnGUI()
    {
        float circleRadius = (1.0f - currentAccuracy) * 50.0f; 
        Vector2 screenMousePosition = Camera.main.WorldToScreenPoint(mousePosition);
        Vector2 guiPosition = new Vector2(screenMousePosition.x - circleRadius, Screen.height - screenMousePosition.y - circleRadius);
        GUI.color = new Color(1f, 1f, 1f, 0.5f); 
        GUI.Box(new Rect(guiPosition.x, guiPosition.y, circleRadius * 2, circleRadius * 2), "");
    }
 
}

