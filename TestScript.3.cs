using System;
using System.Windows.Forms;
using GTA;
using GTA.Native;
//	Since this is a ``.cs`` file and not a project within a ``.dll``, we can't call classes that are from other files. For this very reason, I cannot make a separate file to contain all of the custom hooks. Those have to be within this file.
//using GlobalPlayer;

/*
//	Custom hooks to use.
*/

public class GlobalEntity : Script
{
	public float damageReduction = 0f;
    public GlobalEntity()
    {
        Tick += Update;
        Interval = 0;
    }

    /// <summary>
    /// Runs every frame.
    /// </summary>
	public virtual void Update(object sender, EventArgs e)
	{

	}

    /// <summary>
    /// Runs before receiving damage.
    /// </summary>
	/*public virtual bool PreHurt(ref int damage, ref int hitDirection, ref bool headShot, ref bool customDamage, ref bool playSound, ref bool genGore, ref DamageSource damageSource)
	{
		damage -= (int)(player.statDefense * (1f - damageReduction));
		//	15 * (1f - 0.75f) = 3.75 = 3
	}*/

    /// <summary>
    /// Runs after receiving damage.
    /// </summary>
	public virtual void PostHurt()	{}
}
public class GlobalPed : GlobalEntity
{
}
public class GlobalPlayer : GlobalPed
{
	public GlobalPlayer()
	{
		KeyUp += OnKeyReleased;
		Interval = 0;
	}

    /// <summary>
    /// Runs when a key is released.
    /// </summary>
	public virtual void OnKeyReleased(object sender, KeyEventArgs e)	{}
}
public class GlobalObject : GlobalEntity
{
	//	PoolObject, not Object.
}
public class GlobalProp : GlobalObject
{
}
public class GlobalPickup : GlobalProp
{
	public GlobalPickup()
	{
		KeyUp += OnPickup;
		Interval = 0;
	}

    /// <summary>
    /// Runs when a pickup is collected.
    /// </summary>
	public virtual void OnPickup(object sender, KeyEventArgs e)	{}
}
public class GlobalProjectile : GlobalProp
{
}
public class GlobalVehicle : GlobalProp
{
}
public class GlobalWeapon : Script
{
}

/*
//	The actual scripts.
*/

public class GiveItems : GlobalPlayer
{
	public override void OnKeyReleased(object sender, KeyEventArgs e)
	{
		if (e.KeyCode == Keys.Oemcomma)
		{
			Ped player = Game.Player.Character;
			WeaponCollection weapon = Game.Player.Character.Weapons;
			Weapon acidPackage = weapon.HasWeapon(WeaponHash.AcidPackage) ? weapon[WeaponHash.AcidPackage] : weapon.Give(WeaponHash.AcidPackage, 0, false, true);
			acidPackage.Ammo += 10;
			Weapon snowball = weapon.HasWeapon(WeaponHash.Snowball) ? weapon[WeaponHash.Snowball] : weapon.Give(WeaponHash.Snowball, 0, false, true);
			snowball.Ammo += 10;
			Weapon jerryCan = weapon.HasWeapon(WeaponHash.PetrolCan) ? weapon[WeaponHash.PetrolCan] : weapon.Give(WeaponHash.PetrolCan, 0, false, true);
			jerryCan.Ammo += 500;
			Weapon fertilizerCan = weapon.HasWeapon(WeaponHash.FertilizerCan) ? weapon[WeaponHash.FertilizerCan] : weapon.Give(WeaponHash.FertilizerCan, 0, false, true);
			fertilizerCan.Ammo += 500;
			Weapon stunGun = weapon.HasWeapon(WeaponHash.StunGun) ? weapon[WeaponHash.StunGun] : weapon.Give(WeaponHash.StunGun, 0, false, true);
			Weapon stunGunMP = weapon.HasWeapon(WeaponHash.StunGunMultiplayer) ? weapon[WeaponHash.StunGunMultiplayer] : weapon.Give(WeaponHash.StunGunMultiplayer, 0, false, true);
			Weapon musket = weapon.HasWeapon(WeaponHash.Musket) ? weapon[WeaponHash.Musket] : weapon.Give(WeaponHash.Musket, 0, false, true);
			musket.Ammo += 1;
			//	Old code. Might keep for memorial purposes.
			/*weapon.Give("WEAPON_ACIDPACKAGE", 10, false, true);
			weapon[WeaponHash.AcidPackage].Ammo += 10;
			weapon.Give("WEAPON_SNOWBALL", 10, false, true);
			weapon[WeaponHash.Snowball].Ammo += 10;*/
		}
	}
}
public class Juggernaut : GlobalPlayer
{
	public bool isWearingJuggernautSuit;
	public override void OnKeyReleased(object sender, KeyEventArgs e)
	{
		if (e.KeyCode == Keys.OemPeriod)
		{
			//isWearingJuggernautSuit = ();
			//isWearingJuggernautSuit = !isWearingJuggernautSuit;
			if (!isWearingJuggernautSuit)	{	EquipJuggernautSuit();	}
			else	{	UnequipJuggernautSuit();	}
		}
	}
	public override void Update(object sender, EventArgs e)
	{
		if (isWearingJuggernautSuit)
		{
			Player player = Game.Player;
			Ped playerPed = Game.Player.Character;
			//	This doesn't actually prevent the Weapon Wheel from opening. It just stops it from rendering.
			//Function.Call(Hash.HIDE_HUD_COMPONENT_THIS_FRAME, 19);
			//	This doesn't prevent Action Mode when Wanted, so we're not using it.
			//Function.Call(Hash.SET_PED_USING_ACTION_MODE, playerPed, false, 0, "DEFAULT_ACTION");
			//	This, however, works. 200 is Action Mode.
			Function.Call(Hash.SET_PED_RESET_FLAG, playerPed, 200, true);
			Game.DisableControlThisFrame(GTA.Control.Jump);
			Game.DisableControlThisFrame(GTA.Control.Enter);
			Game.DisableControlThisFrame(GTA.Control.Cover);
			Game.DisableControlThisFrame(GTA.Control.Duck);
			//Game.DisableControlThisFrame(GTA.Control.SelectWeapon);
			if (playerPed.Health <= 200)
			{
				UnequipJuggernautSuit();
			}
		}
	}
	private void EquipJuggernautSuit()
	{
		Player player = Game.Player;
		Ped playerPed = Game.Player.Character;
		//var isMichael =	PED.IS_PED_MODEL(playerPed, GAMEPLAY.GET_HASH_KEY("player_zero"));
		bool isMichael =	Function.Call(Hash.IS_PED_MODEL, playerPed, (GAMEPLAY.GET_HASH_KEY("player_zero")));
		this.isWearingJuggernautSuit = true;
		//	Going to prevent the Cover keybind instead.
		//player.CanUseCover = false;
		playerPed.MaxHealth = 2000;
		playerPed.Health = 2000;
		player.MaxArmor = 2000;
		playerPed.Armor = 2000;
		Function.Call(Hash.RESET_PED_MOVEMENT_CLIPSET, playerPed, 1.0f);
		Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, playerPed, "ANIM_GROUP_MOVE_BALLISTIC", 1.0f);
		Function.Call(Hash.SET_PED_STRAFE_CLIPSET, playerPed, "MOVE_STRAFE_BALLISTIC");
		Function.Call(Hash.SET_WEAPON_ANIMATION_OVERRIDE, playerPed, 0x5534A626);
		if (isMichael)
		{
			PED.SET_PED_COMPONENT_VARIATION(playerPed, 3, 5, 0, 0); -- Upper
			PED.SET_PED_COMPONENT_VARIATION(playerPed, 4, 5, 0, 0); -- Lower
			PED.SET_PED_COMPONENT_VARIATION(playerPed, 5, 1, 0, 0); -- Hands
			PED.SET_PED_COMPONENT_VARIATION(playerPed, 6, 5, 0, 0); -- Shoes
			PED.SET_PED_COMPONENT_VARIATION(playerPed, 8, 5, 0, 0); -- Accessory 0
			PED.SET_PED_COMPONENT_VARIATION(playerPed, 9, 0, 0, 0); -- Accessory 1
			PED.SET_PED_COMPONENT_VARIATION(playerPed, 10, 0, 0, 0); -- Badges / Juggernaut Mask
			PED.SET_PED_PROP_INDEX(playerPed, 0, 26, 0, false); -- Hats
		}
	}
	private void UnequipJuggernautSuit()
	{
		Player player = Game.Player;
		Ped playerPed = Game.Player.Character;
		this.isWearingJuggernautSuit = false;
		//player.CanUseCover = true;
		playerPed.MaxHealth = 200;
		playerPed.Health = 200;
		player.MaxArmor = 100;
		playerPed.Armor = 100;
		Function.Call(Hash.RESET_PED_MOVEMENT_CLIPSET, playerPed, 1.0f);
		Function.Call(Hash.RESET_PED_STRAFE_CLIPSET, playerPed);
		Function.Call(Hash.SET_WEAPON_ANIMATION_OVERRIDE, playerPed, 0x0);
	}
}