using UnityEngine;
using System.Collections.Generic;
using Weapons;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> weaponPrefabs = new List<GameObject>();
    [SerializeField] private Transform weaponHolder;
    
    private List<Weapon> activeWeapons = new List<Weapon>();
    
    public static WeaponManager Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        if (weaponHolder == null)
        {
            weaponHolder = transform;
        }
    }
    
    public void AddWeapon(int weaponIndex)
    {
        if (weaponIndex < 0 || weaponIndex >= weaponPrefabs.Count)
        {
            Debug.LogError("Invalid weapon index!");
            return;
        }
        
        GameObject weaponObj = Instantiate(weaponPrefabs[weaponIndex], weaponHolder);
        Weapon weapon = weaponObj.GetComponent<Weapon>();
        
        if (weapon != null)
        {
            activeWeapons.Add(weapon);
        }
        else
        {
            Debug.LogError("Weapon prefab doesn't have a Weapon component!");
            Destroy(weaponObj);
        }
    }
    
    public void UpgradeFireWeapon(string upgrade)
    {
        foreach (Weapon weapon in activeWeapons)
        {
            if (weapon is FireWeapon fireWeapon)
            {
                switch (upgrade)
                {
                    case "FlameBurst":
                        fireWeapon.UnlockFlameBurst();
                        break;
                    case "MoltenCore":
                        fireWeapon.UnlockMoltenCore();
                        break;
                    case "MeteorDrop":
                        fireWeapon.UnlockMeteorDrop();
                        break;
                }
            }
        }
    }
    
    public List<Weapon> GetActiveWeapons()
    {
        return activeWeapons;
    }
    
    // Call this when starting a new game
    public void ClearWeapons()
    {
        foreach (Weapon weapon in activeWeapons)
        {
            Destroy(weapon.gameObject);
        }
        
        activeWeapons.Clear();
    }
} 