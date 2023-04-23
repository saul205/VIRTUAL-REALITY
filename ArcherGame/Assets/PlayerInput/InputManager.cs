using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button3))
        {
            if (!hold2)
                press2 = true;
        }
        else if (Input.GetKey(KeyCode.Joystick1Button3))
        {
            hold2 = true;
        }
        else if(hold2)
        {
            release2 = true;
            hold2 = false;
        }
    }

    #region Jump
    bool jump = false;
    public void OnJump()
    {
        jump = !jump;
    }

    public bool GetJumpInput()
    {
        return Input.GetKey(KeyCode.Joystick1Button0) || jump;
    }
    #endregion

    #region Shoot 
    bool hold = false;
    bool press = false;
    bool release = false;
    bool cancel = false;

    bool hold2 = false;
    bool press2 = false;
    bool release2 = false;
    bool cancel2 = false;
    private void OnShoot()
    {
        if (!hold)
        {
            press = true;
        }
        else
        {
            release = true;
        }
        hold = !hold;
    }

    public void GetShootInput(ref bool _press, ref bool _hold, ref bool _release, ref bool _cancel)
    {
        _hold = hold || hold2;
        _press = press || press2;
        _release = release || release2;
        _cancel = cancel || Input.GetKeyDown(KeyCode.Joystick1Button1);
    }
    #endregion

    #region Weapon
    bool switchWeapon = false;
    bool next = true;
    bool reload = false;
    private void OnCancelCharge()
    {
        cancel = true;
    }

    private void OnSwitchWeapon(InputValue value)
    {

        if (value.Get() != null)
        {
            next = value.Get<float>() > 0;
            switchWeapon = true;
        }
        else
        {
            next = true;
            switchWeapon = false;
        }
    }

    private void OnReload()
    {
        reload = true; //weapons[ActiveWeaponIndex].SetReload();
    }

    public bool GetReloadInput()
    {
        return reload || Input.GetKeyDown(KeyCode.Joystick1Button2);
    }

    public bool GetSwitchInput(ref bool _next)
    {
        _next = next;
        return switchWeapon || Input.GetKeyDown(KeyCode.Joystick1Button1);
    }

    
    #endregion
    
    private void LateUpdate()
    {
        cancel = false;
        release = false;
        press = false;
        switchWeapon = false;
        reload = false;
    }
}
