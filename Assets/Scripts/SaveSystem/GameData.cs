using System;
using UnityEngine;
using System.Collections.Generic;


[Serializable]
public class GameData
{
    public int gold;

    public List<Inventory_Item> item_list;
    public SerializableDictionary<string, int> inventory; // string -> item_id, int -> item.stack_size. item.save_id -> stack_size
    public SerializableDictionary<string, int> storage_items;
    public SerializableDictionary<string, int> storage_materials;

    public SerializableDictionary<string, ItemType> equiped_items; // item.save_id -> slot_type

    public int skill_points;
    public SerializableDictionary<string, bool> skill_tree_ui; // skill_name -> unlock_status
    public SerializableDictionary<SkillType, SkillUpgradeType> skill_upgrades; // skill_type -> upgrade_type


    public SerializableDictionary<string, bool> unlocked_checkpoints; // checkpoint_id -> unlock_status
    public SerializableDictionary<string, Vector3> in_scene_portals; // scene_name -> portal_position

    public string portal_destination_scene_name;
    public bool returning_from_town;

    public string last_scene_played;
    public Vector3 last_player_position;

    public GameData()
    {
        inventory = new SerializableDictionary<string, int>();
        storage_items = new SerializableDictionary<string, int>();
        storage_materials = new SerializableDictionary<string, int>();

        equiped_items = new SerializableDictionary<string, ItemType>();

        skill_tree_ui = new SerializableDictionary<string, bool>();
        skill_upgrades = new SerializableDictionary<SkillType, SkillUpgradeType>();

        unlocked_checkpoints = new SerializableDictionary<string, bool>();
        in_scene_portals = new SerializableDictionary<string, Vector3>();
    }
}
