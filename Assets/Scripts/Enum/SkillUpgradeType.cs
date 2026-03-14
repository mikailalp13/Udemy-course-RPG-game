using UnityEngine;

public enum SkillUpgradeType 
{
    None,

    // ------- Dash Tree -------
    Dash, // Dash to avoid damage
    Dash_CloneOnStart, // Create a clone when dash starts
    Dash_CloneOnStartAndArrival, // Create a clone when dash starts and ends
    Dash_ShardOnStart, // Create a shard when dash starts
    Dash_ShardOnStartAndArrival, // Create a shard when dash starts and ends

    // ------- Shard Tree -------
    Shard, // The shard explodes when touched by an enemy or time goes up
    Shard_MoveToEnemy, // Shard will move towards nearest enemy
    Shard_Multicast, // Shard ability can have up to N charges. You can cast them all in a row
    Shard_Teleport, // You can swap places with the last shard you created
    Shard_TeleportHpRewind, // When you swap places with shard, your HP % is same as it when you created the shard

    // ------- Sword Tree -------
    SwordThrow, // You can throw your sword to damage enemies from range
    SwordThrow_Spin, // Your sword will spin at one point and damage enemies, like a chainsaw
    SwordThrow_Pierce, // Pierce sword will pierce N Targets
    SwordThrow_Bounce, // Bounce sword will bounce between enemies

    // ------- Time Echo Tree -------
    TimeEcho, // Create a clone of the player. It can take damage from enemies
    TimeEcho_SingleAttack, // Time Echo can perform a single attack
    TimeEcho_MultiAttack, // Time Echo can perform N attacks
    TimeEcho_ChanceToDuplicate, // Time Echo has a chance to create another time echo when attacks

    TimeEcho_HealWisp, // When time echo dies it creates a wisp that flies towards to player to heal
                       // Heal amount is the last damage player took before its death

    TimeEcho_CleanseWisp, // Wisp will now remove negative effects from player
    TimeEcho_CooldownWisp, // Wisp will reduce cooldown of all skills by N seconds

    // ------- Domain Expansion Tree -------
    Domain_SlowingDown, // Create an area in which you slow down enemies by %90-100. You can freely move and fight
    Domain_EchoSpam, // You can no longer move but you spam enemy with Time Echo ability
    Domain_ShardSpam // You can no longer move but you spam enemy with Time Shard ability
}
