using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    int currentWeaponIdx = 0;
    int previousWeaponIdx;


    void Update()
    {
        previousWeaponIdx = currentWeaponIdx;

        //Make Changes to current weapon index
        HandleKeyPress();
        HandleScrollWheel();

        //if any changes are made, change weapon
        if(previousWeaponIdx != currentWeaponIdx)
        {
            SwitchWeapon();
        }
    }

    void HandleKeyPress()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            currentWeaponIdx = 0;
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            currentWeaponIdx = 1;
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            currentWeaponIdx = 2;
    }

    void HandleScrollWheel()
    {
        if(Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            currentWeaponIdx = (currentWeaponIdx >= transform.childCount - 1) ? 0 : currentWeaponIdx + 1;
        }
        else if(Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            currentWeaponIdx = (currentWeaponIdx <= 0) ? transform.childCount - 1 : currentWeaponIdx - 1;
        }
    }

    void SwitchWeapon()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(i==currentWeaponIdx);
        }
    }
}
