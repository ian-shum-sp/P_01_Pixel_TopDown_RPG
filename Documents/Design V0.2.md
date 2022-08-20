## Game

### Equipment System
1. Modifiers:
	- Debuffs: 
		1. Bleeding: Deals damage per second for a fixed duration of time
		2. Knockback: Push unit further away
		3. Elements (Fire, Wind, Ice, Thunder): Deals damage per second for a fixed duration of time

	***Debuffs only apply to level higher or equal to applied Debuff***  
	For example: if player is already affected with Bleeding II, Bleeding I will not be apply and time will not be reset for the player, only applying Bleeding II or higher level will refresh the time  

	- Buffs:
		1. Bleeding Resistance: Reduce damage per second caused by Bleeding
		2. Knockback Resistance: Reduce push distance caused by Knockback
		3. Element (Fire, Wind, Ice, Thunder) Resistance: Reduce damage per second caused by Elements
		4. Strength: Increase base damage of weapon for a fixed duration of time (Potion only)
		5. Speed: Increase movement speed of player for a fixed duration of time (Potion only)
	
	***All Buffs and Debuffs have 3 maximum levels***  
	For Bleeding, Knockback and Elements, Buffs will reduce the level of Debuffs (Example: Hitting with Bleeding III on Bleeding Resistance I will result in applying Bleeding II)  
	All Buffs and Debuffs ignore Damage Points and Armor Points
2. Armor
 	- Adds Armor and Buffs Modifiers
	- Has Level Requirement
	- Type:
		1. Head: Moderate Armor
		2. Chest: High Armor
		3. Boots: Low Armor  
	- Lowest Armor Points: 1 (1% Damage Reduction)
	- Highest Armor Points: 50 (50% Damage Reduction)
2. Weapon
	- With Damage and Debuffs Modifiers
	- Has Level Requirement
	- Type: 
		1. Melee: 
			- High Damage
			- Higher Chance with High Level Debuffs: Bleeding, Knockback
		2. Ranged: 
			- Low Damage
			- Higher Chance with High Level Debuffs: Elements
	- Lowest Damage Points: 1
	- Highest Damage Points: 50
3. Potion
	- Provides Buffs for a fixed duration of time
	- Type:
		1. Bleeding Resistance (Pink)
		2. Knockback Resistance (Yellow)
		3. Element (Fire, Wind, Ice, Thunder) Resistance (Purple)
		4. Strength (Red)
		5. Speed (Blue)
		6. Healing (Light Green): Heals a fixed amount of health when consumed, maximum 3 levels
		7. Stamina (Dark Green): Heals a fixed amount of stamina when consumed, maximum 3 levels
4. Pouch
	- Only potions in the Pouch slot is considered as active and can be used
	- Upgradable with Gold and Level Requirement
	- Maximum 4 Level
5. Inventory
	- 3 different inventory section: Equipment, Weapon and Potions
	- One row can have 16 slots
		- Equipment Inventory: Maximum 4 rows   
		- Equipment Inventory: Maximum 3 rows   
		- Equipment Inventory: Maximum 1 row   
	- Upgradable with Gold and Level Requirement
	- Maximum 4 Level

### Gold System
1. Use for upgrading Pouch/Inventory Slots and Buying Equipments

### Level System
1. XP Table
2. Mob grants different amount of XP
3. Level allows player to meet different Level Requirement of Equipments
4. Level adds health and stamina to the player
5. 
### Combat System
1. Hitboxes
2. Modifiers
3. Health 
4. Stamina

### General Map Design
1. Player can enter map through different portal once unlocked
2. Player can return to Central Hub through the portals found inside the map
3. Upon entering/leaving the map, the map will be reset
4. Same map in regions has different difficulty level which has Level Requirement
5. Different map in regions has different difficulty level which has Level Requirement

### Regions
1. Dungeon
	1. Central Hub Shop: 
		- Blacksmith: specializes in
			- weapon of bleeding and fire element
			- armor of bleeding resistance and fire element resistance
		- Special: Potions of bleeding resistance
	2. Map Designs: 
		- Small Mob Quantity: High
		- Medium Mob Quantity: Medium
		- Big Mob Quantity: Medium
	3. Map Loots:
		- More Gold
		- Normal Equipment (Armor, Weapon, Potion)
		- Normal XP
2. Enchanted Forest
	1. Central Hub Shop: 
		- Blacksmith: specializes in
			- weapon of knockback and wind element
			- armor of knockback resistance and wind element resistance
		- Special: Potions of knockback resistance
	2. Map Designs: 
		- Small Mob Quantity: High
		- Medium Mob Quantity: High
		- Big Mob Quantity: Low
	3. Map Loots:
		- Normal Gold
		- More Equipment (Armor, Weapon, Potion)
		- Normal XP
3. Fantasy
	1. Central Hub Shop: 
		- Blacksmith: specializes in
			- weapon of fire, wind, ice and thunder element
			- armor of fire, wind, ice and thunder element resistance
		- Special: Potions of element resistance
	2. Map Designs: 
		- Small Mob Quantity: Medium
		- Medium Mob Quantity: High
		- Big Mob Quantity: Medium
	3. Map Loots:
		- Normal Gold
		- Normal Equipment (Armor, Weapon, Potion)
		- More XP

***For Gold, Experience Table, Equipments Stats, Modifiers' Effects, Mobs, please refer to Stats.md under Documents directory***

### Updates:
1. Element modifiers now is a general modifiers, no longer has different elements
2. Added extra effects to Modifiers: 
	- Debuffs:
		1. Bleeding: Now also reduce the speed of affected units
		2. Element: Now also reduce the damage output of affected units
3. Removed stamina stat from game
4. Updated Shops in Central Hubs
	- Dungeon:
		1. Now sell weapon and armor related to bleeding
	- Enchanted Forest:
		1. Now sell weapon and armor related to knockback
	- Fantasy:
		1. Now sell weapon and armor related to element
