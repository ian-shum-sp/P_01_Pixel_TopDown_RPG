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
        

        #region For testing only -> uncomment if unneeded
        // Equipment starterMeleeWeaponTest1 = equipments.First(x => x.equipmentID == "MW01");
        // Equipment starterMeleeWeaponTest2 = equipments.First(x => x.equipmentID == "MW02");
        // Equipment starterMeleeWeaponTest3 = equipments.First(x => x.equipmentID == "MW03");
        // Equipment starterMeleeWeaponTest4 = equipments.First(x => x.equipmentID == "MW04");
        // Equipment starterMeleeWeaponTest5 = equipments.First(x => x.equipmentID == "MW05");
        // Equipment starterMeleeWeaponTest6 = equipments.First(x => x.equipmentID == "MW06");
        // Equipment starterMeleeWeaponTest7 = equipments.First(x => x.equipmentID == "MW07");
        // Equipment starterMeleeWeaponTest8 = equipments.First(x => x.equipmentID == "MW08");

        // Equipment starterMeleeWeaponTest9 = equipments.First(x => x.equipmentID == "MW09");
        // Equipment starterMeleeWeaponTest10 = equipments.First(x => x.equipmentID == "MW10");
        // Equipment starterMeleeWeaponTest11 = equipments.First(x => x.equipmentID == "MW11");
        // Equipment starterMeleeWeaponTest12 = equipments.First(x => x.equipmentID == "MW12");
        // Equipment starterMeleeWeaponTest13 = equipments.First(x => x.equipmentID == "MW13");
        // Equipment starterMeleeWeaponTest14 = equipments.First(x => x.equipmentID == "MW14");
        // Equipment starterMeleeWeaponTest15 = equipments.First(x => x.equipmentID == "MW15");
        // Equipment starterMeleeWeaponTest16 = equipments.First(x => x.equipmentID == "MW16");

        // Equipment starterMeleeWeaponTest17 = equipments.First(x => x.equipmentID == "MW17");
        // Equipment starterMeleeWeaponTest18 = equipments.First(x => x.equipmentID == "MW18");
        // Equipment starterMeleeWeaponTest19 = equipments.First(x => x.equipmentID == "MW19");
        // Equipment starterMeleeWeaponTest20 = equipments.First(x => x.equipmentID == "MW20");
        // Equipment starterMeleeWeaponTest21 = equipments.First(x => x.equipmentID == "MW21");
        // Equipment starterMeleeWeaponTest22 = equipments.First(x => x.equipmentID == "MW22");
        // Equipment starterMeleeWeaponTest23 = equipments.First(x => x.equipmentID == "MW23");
        // Equipment starterMeleeWeaponTest24 = equipments.First(x => x.equipmentID == "MW24");

        // Equipment starterMeleeWeaponTest25 = equipments.First(x => x.equipmentID == "MW25");
        // Equipment starterMeleeWeaponTest26 = equipments.First(x => x.equipmentID == "MW26");
        // Equipment starterMeleeWeaponTest27 = equipments.First(x => x.equipmentID == "MW27");
        // Equipment starterMeleeWeaponTest28 = equipments.First(x => x.equipmentID == "MW28");
        // Equipment starterRangedWeaponTest1 = equipments.First(x => x.equipmentID == "RW01");
        // Equipment starterRangedWeaponTest2 = equipments.First(x => x.equipmentID == "RW02");
        // Equipment starterRangedWeaponTest3 = equipments.First(x => x.equipmentID == "RW03");
        // Equipment starterRangedWeaponTest4 = equipments.First(x => x.equipmentID == "RW04");

        // Equipment starterRangedWeaponTest5 = equipments.First(x => x.equipmentID == "RW05");
        // Equipment starterRangedWeaponTest6 = equipments.First(x => x.equipmentID == "RW06");
        // Equipment starterRangedWeaponTest7 = equipments.First(x => x.equipmentID == "RW07");
        // Equipment starterRangedWeaponTest8 = equipments.First(x => x.equipmentID == "RW08");
        // Equipment starterRangedWeaponTest9 = equipments.First(x => x.equipmentID == "RW09");
        // Equipment starterRangedWeaponTest10 = equipments.First(x => x.equipmentID == "RW10");
        // Equipment starterRangedWeaponTest11 = equipments.First(x => x.equipmentID == "RW11");
        // Equipment starterRangedWeaponTest12 = equipments.First(x => x.equipmentID == "RW12");

        // Equipment starterRangedWeaponTest13 = equipments.First(x => x.equipmentID == "RW13");
        // Equipment starterRangedWeaponTest14 = equipments.First(x => x.equipmentID == "RW14");
        // Equipment starterRangedWeaponTest15 = equipments.First(x => x.equipmentID == "RW15");
        // Equipment starterRangedWeaponTest16 = equipments.First(x => x.equipmentID == "RW16");
        // Equipment starterRangedWeaponTest17 = equipments.First(x => x.equipmentID == "RW17");

        // Equipment starterPotionTest1 = equipments.First(x => x.equipmentID == "P01");
        // Equipment starterPotionTest2 = equipments.First(x => x.equipmentID == "P02");
        // Equipment starterPotionTest3 = equipments.First(x => x.equipmentID == "P03");
        // Equipment starterPotionTest4 = equipments.First(x => x.equipmentID == "P04");
        // Equipment starterPotionTest5 = equipments.First(x => x.equipmentID == "P05");
        // Equipment starterPotionTest6 = equipments.First(x => x.equipmentID == "P06");
        // Equipment starterPotionTest7 = equipments.First(x => x.equipmentID == "P07");
        // Equipment starterPotionTest8 = equipments.First(x => x.equipmentID == "P08");
        // Equipment starterPotionTest9 = equipments.First(x => x.equipmentID == "P09");
        // Equipment starterPotionTest10 = equipments.First(x => x.equipmentID == "P10");
        // Equipment starterPotionTest11 = equipments.First(x => x.equipmentID == "P11");
        // Equipment starterPotionTest12 = equipments.First(x => x.equipmentID == "P12");
        // Equipment starterPotionTest13 = equipments.First(x => x.equipmentID == "P13");
        // Equipment starterPotionTest14 = equipments.First(x => x.equipmentID == "P14");
        // Equipment starterPotionTest15 = equipments.First(x => x.equipmentID == "P15");
        // Equipment starterPotionTest16 = equipments.First(x => x.equipmentID == "P16");
        // Equipment starterPotionTest17 = equipments.First(x => x.equipmentID == "P17");
        // Equipment starterPotionTest18 = equipments.First(x => x.equipmentID == "P18");
        #endregion

        starterEquipments.Add(starterHeadArmor);
        starterEquipments.Add(starterChestArmor);
        starterEquipments.Add(starterBootsArmor);
        starterEquipments.Add(starterMeleeWeapon);
        starterEquipments.Add(starterRangedWeapon);
        starterEquipments.Add(starterPotion);

        #region For testing only -> uncomment if unneeded
        // starterEquipments.Add(starterMeleeWeaponTest1);
        // starterEquipments.Add(starterMeleeWeaponTest2);
        // starterEquipments.Add(starterMeleeWeaponTest3);
        // starterEquipments.Add(starterMeleeWeaponTest4);
        // starterEquipments.Add(starterMeleeWeaponTest5);
        // starterEquipments.Add(starterMeleeWeaponTest6);
        // starterEquipments.Add(starterMeleeWeaponTest7);
        // starterEquipments.Add(starterMeleeWeaponTest8);

        // starterEquipments.Add(starterMeleeWeaponTest9);
        // starterEquipments.Add(starterMeleeWeaponTest10);
        // starterEquipments.Add(starterMeleeWeaponTest11);
        // starterEquipments.Add(starterMeleeWeaponTest12);
        // starterEquipments.Add(starterMeleeWeaponTest13);
        // starterEquipments.Add(starterMeleeWeaponTest14);
        // starterEquipments.Add(starterMeleeWeaponTest15);
        // starterEquipments.Add(starterMeleeWeaponTest16);

        // starterEquipments.Add(starterMeleeWeaponTest17);
        // starterEquipments.Add(starterMeleeWeaponTest18);
        // starterEquipments.Add(starterMeleeWeaponTest19);
        // starterEquipments.Add(starterMeleeWeaponTest20);
        // starterEquipments.Add(starterMeleeWeaponTest21);
        // starterEquipments.Add(starterMeleeWeaponTest22);
        // starterEquipments.Add(starterMeleeWeaponTest23);
        // starterEquipments.Add(starterMeleeWeaponTest24);

        // starterEquipments.Add(starterMeleeWeaponTest25);
        // starterEquipments.Add(starterMeleeWeaponTest26);
        // starterEquipments.Add(starterMeleeWeaponTest27);
        // starterEquipments.Add(starterMeleeWeaponTest28);
        // starterEquipments.Add(starterRangedWeaponTest1);
        // starterEquipments.Add(starterRangedWeaponTest2);
        // starterEquipments.Add(starterRangedWeaponTest3);
        // starterEquipments.Add(starterRangedWeaponTest4);

        // starterEquipments.Add(starterRangedWeaponTest5);
        // starterEquipments.Add(starterRangedWeaponTest6);
        // starterEquipments.Add(starterRangedWeaponTest7);
        // starterEquipments.Add(starterRangedWeaponTest8);
        // starterEquipments.Add(starterRangedWeaponTest9);
        // starterEquipments.Add(starterRangedWeaponTest10);
        // starterEquipments.Add(starterRangedWeaponTest11);
        // starterEquipments.Add(starterRangedWeaponTest12);

        // starterEquipments.Add(starterRangedWeaponTest10);
        // starterEquipments.Add(starterRangedWeaponTest11);
        // starterEquipments.Add(starterRangedWeaponTest12);
        // starterEquipments.Add(starterRangedWeaponTest13);
        // starterEquipments.Add(starterRangedWeaponTest14);
        // starterEquipments.Add(starterRangedWeaponTest15);
        // starterEquipments.Add(starterRangedWeaponTest16);
        // starterEquipments.Add(starterRangedWeaponTest17);

        #endregion


        return starterEquipments;
    }

    public Equipment GetRandomArmor()
    {
        List<Equipment> armors = equipments.Where(x => x.equipmentType == Common.EquipmentType.BOOTS_ARMOR || x.equipmentType == Common.EquipmentType.CHEST_ARMOR || x.equipmentType == Common.EquipmentType.HEAD_ARMOR).ToList();
        int randomIndex = Random.Range(0, armors.Count);
        return armors[randomIndex];
    }

    public Equipment GetRandomWeapon()
    {
        List<Equipment> weapons = equipments.Where(x => x.equipmentType == Common.EquipmentType.MELEE_WEAPON || x.equipmentType == Common.EquipmentType.RANGED_WEAPON).ToList();
        int randomIndex = Random.Range(0, weapons.Count);
        return weapons[randomIndex];
    }

    public Equipment GetRandomPotion()
    {
        List<Equipment> potions = equipments.Where(x => x.equipmentType == Common.EquipmentType.POTION).ToList();
        int randomIndex = Random.Range(0, potions.Count);
        return potions[randomIndex];
    }

    public Equipment GetEquipmentByID(string equipmentID)
    {
        return equipments.First(x => x.equipmentID == equipmentID);
    }
}
