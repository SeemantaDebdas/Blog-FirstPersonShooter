using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Weapon : MonoBehaviour
{
    [Header("Game Feel")]
    [SerializeField] GameObject bulletHole;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] float bulletHolePositionOffset;


    [Header("Ammunation")]
    [SerializeField] AmmoType ammoType;
    [SerializeField] Ammo ammo;
    [SerializeField] TextMeshProUGUI ammoText;

    [Header("Raycast")]
    [SerializeField] LayerMask hittableLayer;
    [SerializeField] float weaponRange;


    [SerializeField] float fireRate;

    Camera mainCam;
    Animator anim;

    bool canShoot = true;
    float thresholdTime;

    private void Awake()
    {
        mainCam = Camera.main;
        anim = GetComponentInParent<Animator>();
    }

    private void Update()
    {
        HandleFireRate();

        if (Input.GetMouseButton(0) && ammo.GetAmmoCount(ammoType) > 0 && canShoot)
        {
            Shoot();
            anim.SetBool("IsShooting", true);
        }
        else
        {
            anim.SetBool("IsShooting", false);
        }

        if (Input.GetKeyDown(KeyCode.R))
            ammo.ReloadAmmoCount(ammoType);

        UpdateAmmoCountText();
    }

    private void HandleFireRate()
    {
        if (thresholdTime < Time.time)
        {
            canShoot = true;
            thresholdTime = Time.time + fireRate;
        }
        else
        {
            canShoot = false;
        }
    }

    private void Shoot()
    { 
        if(ammo!=null)
            ammo.ReduceAmmoCount(ammoType);
        HandleMuzzleFlash();
        HandleRaycast();
    }

    private void HandleRaycast()
    {
        if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out RaycastHit hit, weaponRange, hittableLayer))
        {
            Instantiate(bulletHole, hit.point + (hit.normal * bulletHolePositionOffset), Quaternion.LookRotation(hit.normal));
        }
        else
        {
            Debug.Log("Not hit Hall");
        }
    }

    void UpdateAmmoCountText()
    {
        if(ammoText != null)
            ammoText.text = ammo.GetAmmoCount(ammoType).ToString() + "/" + ammo.GetInitialAmmoCount(ammoType).ToString();
    }

    private void HandleMuzzleFlash()
    {
        if (muzzleFlash.isPlaying)
            muzzleFlash.Stop();
        muzzleFlash.Play();
    }
}
