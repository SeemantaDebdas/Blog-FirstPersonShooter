using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;

public class WeaponZoom : MonoBehaviour
{
    [Header("Scope")]
    [SerializeField] bool hasScope;
    [SerializeField] Image scopeImage;
    [SerializeField] Camera weaponCamera;

    [Header("Post Processing")]
    [SerializeField] Volume postProcessVolume;
    [SerializeField] [Range(0, 1)] float normalVignetteIntensity;
    [SerializeField] [Range(0, 1)] float zoomedVignetteIntensity;
    
    [Header("Zoom Parameters")]
    [SerializeField] float zoomedFOV;
    [SerializeField] float zoomTime;
    float normalFOV;

    Camera mainCam;
    Animator anim;
    Vignette vignette;

    private void Awake()
    {
        postProcessVolume.profile.TryGet<Vignette>(out vignette);
        anim = GetComponentInParent<Animator>();
    }

    private void Start()
    {
        mainCam = Camera.main;
        normalFOV = mainCam.fieldOfView;
    }

    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            if (hasScope)
            {
                anim.SetBool("IsScoped", true);
                StartCoroutine(ScopedZoom());
            }
            else
            {
                NormalZoom();
            }
        }
        else 
        {
            anim.SetBool("IsScoped", false);
            weaponCamera.gameObject.SetActive(true);
            scopeImage.gameObject.SetActive(false);
            mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, normalFOV, zoomTime);
            vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, normalVignetteIntensity, zoomTime);
        }
    }

    void NormalZoom()
    {
        mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, zoomedFOV, zoomTime);
        vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, zoomedVignetteIntensity, zoomTime);
    }

    IEnumerator ScopedZoom()
    {
        yield return new WaitForSeconds(0.15f);
        weaponCamera.gameObject.SetActive(false);
        scopeImage.gameObject.SetActive(true);
        mainCam.fieldOfView = zoomedFOV;
    }
}
