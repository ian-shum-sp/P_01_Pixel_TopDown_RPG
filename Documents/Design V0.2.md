Game:

Equipment System
1. Modifiers:
	Debuffs: 
		- Bleeding: Deals damage per second for a fixed duration of time
		- Knockback: Push enemy further away
		- Elements (Fire, Wind, Ice, Thunder): Deals damage per second for a fixed duration of time
		- Debuffs only apply to level higher or equal to applied Debuff 
		- For example: if player is already affected with Bleeding II, Bleeding I will not be apply and time will not be reset for the player, only applying Bleeding II or higher level will refresh the time
	Buffs:
		- Bleeding Resistance: Reduce damage per second caused by Bleeding
		- Knockback Resistance: Reduce push distance caused by Knockback
		- Element (Fire, Wind, Ice, Thunder) Resistance: Reduce damage per second caused by Elements
		- Strength: Increase base damage of weapon for a fixed duration of time (Potion only)
		- Speed: Increase movement speed of player for a fixed duration of time	(Potion only)
	All Buffs and Debuffs have 3 maximum levels:
		- For Bleeding, Knockback and Elements, Buffs will reduce the level of Debuffs (Example: Hitting with Bleeding III on Bleeding Resistance I will result in applying Bleeding II)
	All Buffs and Debuffs ignore Damage Points and Armor Points
2. Armor
	Adds Armor and Buffs Modifiers
	Has Level Requirement
	Type:
		- Head: Moderate Armor
		- Chest: High Armor
		- Boots: Low Armor
	Lowest Armor Points: 1 (1% Damage Reduction)
	Highest Armor Points: 50 (50% Damage Reduction)
2. Weapon
	With Damage and Debuffs Modifiers
	Has Level Requirement
	Type: 
		- Melee: 
			- High Damage
			- Higher Chance with High Level Debuffs: Bleeding, Knockback
		- Ranged: 
			- Low Damage
			- Higher Chance with High Level Debuffs: Elements
	Lowest Damage Points: 1
	Highest Damage Points: 50
3. Potion
	Provides Buffs for a fixed duration of time
	Type:
		- Bleeding Resistance (Pink)
		- Knockback Resistance (Yellow)
		- Element (Fire, Wind, Ice, Thunder) Resistance (Purple)
		- Strength (Red)
		- Speed (Blue)
		- Healing (Light Green): Heals a fixed amount of health when consumed, maximum 3 levels
		- Stamina (Dark Green): Heals a fixed amount of stamina when consumed, maximum 3 levels
4. Pouch
	Only potions in the Pouch slot is considered as active and can be used
	Upgradable with Gold and Level Requirement
	Maximum 4 Level
5. Inventory
	3 different inventory section: Equipment, Weapon and Potions
	One row can have 16 slots
		- Equipment Inventory: Maximum 4 rows   
		- Equipment Inventory: Maximum 3 rows   
		- Equipment Inventory: Maximum 1 row   
	Upgradable with Gold and Level Requirement
	Maximum 4 Level

Gold System
1. Use for upgrading Pouch/Inventory Slots and Buying Equipments

Level System
1. XP Table
2. Mob grants different amount of XP
3. Level allows player to meet different Level Requirement of Equipments
4. Level adds health and stamina to the player

Combat System
1. Hitboxes
2. Modifiers
3. Health 
4. Stamina

General Map Design
1. Player can enter map through different portal once unlocked
2. Player can return to Central Hub through the portals found inside the map
3. Upon entering/leaving the map, the map will be reset
4. Same map in regions has different difficulty level which has Level Requirement
5. Different map in regions has different difficulty level which has Level Requirement

Regions
1. Dungeon
	Central Hub Shop: 
		- Blacksmith: specializes in
			- weapon of bleeding and fire element
			- armor of bleeding resistance and fire element resistance
		- Special: Potions of bleeding resistance
	Map Designs: 
		- Small Mob Quantity: High
		- Medium Mob Quantity: Medium
		- Big Mob Quantity: Medium
	Map Loots:
		- More Gold
		- Normal Equipment (Armor, Weapon, Potion)
		- Normal XP
2. Enchanted Forest
	Central Hub Shop: 
		- Blacksmith: specializes in
			- weapon of knockback and wind element
			- armor of knockback resistance and wind element resistance
		- Special: Potions of knockback resistance
	Map Designs: 
		- Small Mob Quantity: High
		- Medium Mob Quantity: High
		- Big Mob Quantity: Low
	Map Loots:
		- Normal Gold
		- More Equipment (Armor, Weapon, Potion)
		- Normal XP
3. Fantasy
	Central Hub Shop: 
		- Blacksmith: specializes in
			- weapon of fire, wind, ice and thunder element
			- armor of fire, wind, ice and thunder element resistance
		- Special: Potions of element resistance
	Map Designs: 
		- Small Mob Quantity: Medium
		- Medium Mob Quantity: High
		- Big Mob Quantity: Medium
	Map Loots:
		- Normal Gold
		- Normal Equipment (Armor, Weapon, Potion)
		- More XP


-0.212
1 tile = 30 atk range
now the ranged weapon too op? the animation time too short to travel very far (maybe increase the cooldown ??)
the chase length need update
drink potion should update bleeding status/element status (reduce level of debuff-> set the _currentBleedingResistanceLevel) -done

need update xp table
need update mobs drop and attack speed (all x1.8)
player's weapon all x1.5
backswing reduce to 0.1