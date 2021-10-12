using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchWeapon : MonoBehaviour
{
    public int currentWpnIndex = 0;

    public GameObject[] weapons;

    // Start is called before the first frame update
    private void SetWeaponActive(bool active)
    {
        weapons[currentWpnIndex].SetActive(active);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            SetWeaponActive(false);

            currentWpnIndex = (currentWpnIndex + 1) % weapons.Length;

            Gun gun = weapons[currentWpnIndex].GetComponent<Gun>();
            if (gun != null)
            {
                gun.OnWeaponUse();
            }

            SetWeaponActive(true);
        }
    }
}
