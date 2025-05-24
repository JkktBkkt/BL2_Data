namespace BL2
{
    public class Seed_Options
    {
        public Dictionary<string, bool> areas = new Dictionary<string, bool>();
        public Dictionary<string, bool> mission_enemy = new Dictionary<string, bool>();
        public Dictionary<string, bool> misc = new Dictionary<string, bool>();

        public Dictionary<string, string> name_mapping = new Dictionary<string, string>();

        // -------------------------------------------------------------------------------------------------------------------------------------
        public Seed_Options()
        {
            areas.Add("Base Game", false);
            areas.Add("Pirate's Booty", false);
            areas.Add("Campaign Of Carnage", false);
            areas.Add("Hammerlock's Hunt", false);
            areas.Add("Dragon Keep", false);
            areas.Add("Fight For Sanctuary", false);
            areas.Add("Bloody Harvest", false);
            areas.Add("Wattle Gobbler", false);
            areas.Add("Mercenary Day", false);
            areas.Add("Wedding Day Massacre", false);
            areas.Add("Son Of Crawmerax", false);
            areas.Add("UVHM Pack", false);
            areas.Add("Digistruct Peak", false);

            mission_enemy.Add("Short Missions", false);
            mission_enemy.Add("Long Missions", false);
            mission_enemy.Add("Very Long Missions", false);
            mission_enemy.Add("Vehicle Missions", false);
            mission_enemy.Add("Slaughter Missions", false);
            mission_enemy.Add("Unique Enemies", false);
            mission_enemy.Add("Slow Enemies", false);
            mission_enemy.Add("Rare Enemies", false);
            mission_enemy.Add("Very Rare Enemies", false);
            mission_enemy.Add("Evolved Enemies", false);
            mission_enemy.Add("Digistruct Enemies", false);

            misc.Add("Mob Farms", false);
            misc.Add("Raids", false);
            misc.Add("Mission Locations", false);
            misc.Add("Special Vendors", false);
            misc.Add("Freebies", false);
            misc.Add("Miscellaneous", false);
            misc.Add("Duplicate Items", false);
            misc.Add("Allow Hints", false);

            name_mapping.Add("BaseGame", "Base Game");
            name_mapping.Add("PiratesBooty", "Pirate's Booty");
            name_mapping.Add("CampaignOfCarnage", "Campaign Of Carnage");
            name_mapping.Add("HammerlocksHunt", "Hammerlock's Hunt");
            name_mapping.Add("DragonKeep", "Dragon Keep");
            name_mapping.Add("FightForSanctuary", "Fight For Sanctuary");
            name_mapping.Add("BloodyHarvest", "Bloody Harvest");
            name_mapping.Add("WattleGobbler", "Wattle Gobbler");
            name_mapping.Add("MercenaryDay", "Mercenary Day");
            name_mapping.Add("WeddingDayMassacre", "Wedding Day Massacre");
            name_mapping.Add("SonOfCrawmerax", "Son Of Crawmerax");
            name_mapping.Add("UVHMPack", "UVHM Pack");
            name_mapping.Add("DigistructPeak", "Digistruct Peak");
            name_mapping.Add("ShortMission", "Short Missions");
            name_mapping.Add("LongMission", "Long Missions");
            name_mapping.Add("VeryLongMission", "Very Long Missions");
            name_mapping.Add("VehicleMission", "Vehicle Missions");
            name_mapping.Add("Slaughter", "Slaughter Missions");
            name_mapping.Add("UniqueEnemy", "Unique Enemies");
            name_mapping.Add("SlowEnemy", "Slow Enemies");
            name_mapping.Add("RareEnemy", "Rare Enemies");
            name_mapping.Add("VeryRareEnemy", "Very Rare Enemies");
            name_mapping.Add("EvolvedEnemy", "Evolved Enemies");
            name_mapping.Add("DigistructEnemy", "Digistruct Enemies");
            name_mapping.Add("MobFarm", "Mob Farms");
            name_mapping.Add("Raid", "Raids");
            name_mapping.Add("MissionLocation", "Mission Locations");
            name_mapping.Add("Vendor", "Special Vendors");
            name_mapping.Add("namesellaneous", "namesellaneous");
            name_mapping.Add("Miscellaneous", "Miscellaneous");
            name_mapping.Add("Freebie", "Freebies");
        }

        // -------------------------------------------------------------------------------------------------------------------------------------
        public void Set_Option(string line)
        {
            string[] parts = line.Split(':', StringSplitOptions.RemoveEmptyEntries);

            if (areas.ContainsKey(parts[0]))
                areas[parts[0]] = parts[1].Trim().ToLower() == "on";

            else if (mission_enemy.ContainsKey(parts[0]))
                mission_enemy[parts[0]] = parts[1].Trim().ToLower() == "on";

            else if (misc.ContainsKey(parts[0]))
                misc[parts[0]] = parts[1].Trim().ToLower() == "on";

            else
                throw new Exception("Unexpected error reading seed options " + line);
        }

        // -------------------------------------------------------------------------------------------------------------------------------------
        public bool Check_Options_String(string group_text)
        {
            if (String.IsNullOrEmpty(group_text))
                return false;

            string[] groups = group_text.Split('|', StringSplitOptions.RemoveEmptyEntries);

            foreach (string group in groups)
            {
                string name = name_mapping[group];

                if (areas.ContainsKey(name) && areas[name] == false)
                    return false;

                if (mission_enemy.ContainsKey(name) && mission_enemy[name] == false)
                    return false;

                if (misc.ContainsKey(name) && misc[name] == false)
                    return false;
            }

            return true;
        }
    }
}
