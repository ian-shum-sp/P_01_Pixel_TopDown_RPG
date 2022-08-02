using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{   
    public List<Equipment> equipments = new List<Equipment>();
    public List<GameObject> weaponPrefabs = new List<GameObject>();

    public List<Equipment> GetStarterEquipments()
    {
        List<Equipment> starterEquipments = new List<Equipment>();

        Equipment starterHeadArmor = equipments.First(x => x.equipmentID == "HA01");
        Equipment starterChestArmor = equipments.First(x => x.equipmentID == "CA01");
        Equipment starterBootsArmor = equipments.First(x => x.equipmentID == "BA01");
        Equipment starterMeleeWeapon = equipments.First(x => x.equipmentID == "MW01");
        Equipment starterRangedWeapon = equipments.First(x => x.equipmentID == "RW01");
        Equipment starterPotion = equipments.First(x => x.equipmentID == "P01");

        starterEquipments.Add(starterHeadArmor);
        starterEquipments.Add(starterChestArmor);
        starterEquipments.Add(starterBootsArmor);
        starterEquipments.Add(starterMeleeWeapon);
        starterEquipments.Add(starterRangedWeapon);
        starterEquipments.Add(starterPotion);

        return starterEquipments;
    }

    public Equipment GetRandomArmor()
    {
        List<Equipment> armors = equipments.Where(x => x.equipmentType == Common.EquipmentType.BOOTS_ARMOR || x.equipmentType == Common.EquipmentType.CHEST_ARMOR || x.equipmentType == Common.EquipmentType.HEAD_ARMOR).ToList();
        int randomIndex = Random.Range(0, armors.Count + 1);
        return armors[randomIndex];
    }

    public Equipment GetRandomWeapon()
    {
        List<Equipment> weapons = equipments.Where(x => x.equipmentType == Common.EquipmentType.MELEE_WEAPON || x.equipmentType == Common.EquipmentType.RANGED_WEAPON).ToList();
        int randomIndex = Random.Range(0, weapons.Count + 1);
        return weapons[randomIndex];
    }

    public Equipment GetRandomPotion()
    {
        List<Equipment> potions = equipments.Where(x => x.equipmentType == Common.EquipmentType.POTION).ToList();
        int randomIndex = Random.Range(0, potions.Count + 1);
        return potions[randomIndex];
    }
}
