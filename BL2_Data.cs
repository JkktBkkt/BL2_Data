using System.Collections.Generic;

namespace BL2
{ 
    public enum Loot_Type { NONE = 0, ANY = 1, ALL_ITEM = 2, COM = 3, GRENADE = 4, RELIC = 5, SHIELD = 6, ALL_WEAP = 7, AR = 8, LAUNCHER = 9, PISTOL = 10, SHOTGUN = 11, SMG = 12, SNIPER = 13 }
    public enum Found_Type { FOUND = 0, NOT_FOUND = 1, EITHER = 2 }
    public enum Char_Class { NOT_SET = -1, ZERO = 0, SAL = 1, MAYA = 2, AXTON = 3, KRIEG = 4, GAIGE = 5 }
    public enum MEO_Type { Not_Set = -1, Mission = 0, Enemy = 1, Other = 2 }

    // --------------------------------------------------------------------------------------------------------------------------------------------
    public class BL2_Data
    {
        public string game { get; set; } = string.Empty;
        public List<Area> areas { get; set; } = [];
        public List<Loot> loots { get; set; } = [];

        public Seed? seed = null;

        public static Char_Class char_class = Char_Class.NOT_SET;

        public readonly static Dictionary<Loot_Type, string> type_string = new()
        {
            { Loot_Type.ALL_ITEM,  "All Items" },
            { Loot_Type.COM ,      "COM" },
            { Loot_Type.RELIC ,    "Relic" },
            { Loot_Type.GRENADE ,  "Grenade" },
            { Loot_Type.SHIELD ,   "Shield" },
            { Loot_Type.ALL_WEAP , "All Weapons" },
            { Loot_Type.AR ,       "Assault Rifle" },
            { Loot_Type.LAUNCHER , "Launcher" },
            { Loot_Type.PISTOL ,   "Pistol" },
            { Loot_Type.SHOTGUN ,  "Shotgun" },
            { Loot_Type.SMG ,      "SMG" },
            { Loot_Type.SNIPER ,   "Sniper Rifle" }
        };

        public readonly static Dictionary<string, string> seed_areas = new()
        {
            { "Borderlands 2",                               "BaseGame" },
            { "Captain Scarlett and her Pirate's Booty",     "PiratesBooty"},
            { "Mr. Torgue's Campaign of Carnage",            "CampaignOfCarnage"},
            { "Sir Hammerlock's Big Game Hunt",              "HammerlocksHunt"},
            { "Tiny Tina's Assault on Dragon Keep",          "DragonKeep"},
            { "Commander Lilith & the Fight for Sanctuary",  "FightForSanctuary"},
            { "Headhunter 1: Bloody Harvest",                "BloodyHarvest"},
            { "Headhunter 2: Wattle Gobbler",                "WattleGobbler"},
            { "Headhunter 3: Mercenary Day",                 "MercenaryDay"},
            { "Headhunter 4: Wedding Day Massacre",          "WeddingDayMassacre"},
            { "Headhunter 5: Son of Crawmerax",              "SonOfCrawmerax"},
            { "Ultimate Vault Hunter Upgrade Pack 2",        "DigistructPeak" }
        };

        public Dictionary<Loot_Type, List<Loot>> loot_lists = new()
        {
            { Loot_Type.NONE, [] },
            { Loot_Type.ANY, [] },
            { Loot_Type.ALL_WEAP, [] },
            { Loot_Type.ALL_ITEM, [] },
            { Loot_Type.COM, [] },
            { Loot_Type.GRENADE, [] },
            { Loot_Type.RELIC, [] },
            { Loot_Type.SHIELD, [] },
            { Loot_Type.AR, [] },
            { Loot_Type.LAUNCHER, [] },
            { Loot_Type.PISTOL, [] },
            { Loot_Type.SHOTGUN, [] },
            { Loot_Type.SMG, [] },
            { Loot_Type.SNIPER, [] },
        };


        public Dictionary<(MEO_Type, Found_Type), int> area_totals = new()
        {
            { (MEO_Type.Mission, Found_Type.NOT_FOUND), 0 },
            { (MEO_Type.Mission, Found_Type.EITHER),    0 },
            { (MEO_Type.Mission, Found_Type.FOUND),     0 },
            { (MEO_Type.Enemy,   Found_Type.NOT_FOUND), 0 },
            { (MEO_Type.Enemy,   Found_Type.EITHER),    0 },
            { (MEO_Type.Enemy,   Found_Type.FOUND),     0 },
            { (MEO_Type.Other,   Found_Type.NOT_FOUND), 0 },
            { (MEO_Type.Other,   Found_Type.EITHER),    0 },
            { (MEO_Type.Other,   Found_Type.FOUND),     0 }
        };

        public int total_missions = 0;
        public int total_enemies = 0;
        public int total_other = 0;


        // --------------------------------------------------------------------------------------------------------------------------------------------
        public bool Options_Are_OK(string option_str)
        {
            if (seed == null)
                return true;

            return seed.options.Check_Options_String(option_str);
        }

        // --------------------------------------------------------------------------------------------------------------------------------------------
        public string[] Everything_MEO_Detail_Row()
        {
            return
            [
                total_missions == 0 ? String.Empty : String.Format("{0,3} | {1}", area_totals[(MEO_Type.Mission, Found_Type.FOUND)], total_missions),
                total_enemies == 0 ? String.Empty : String.Format("{0,3} | {1}", area_totals[(MEO_Type.Enemy, Found_Type.FOUND)], total_enemies),
                total_other == 0 ? String.Empty : String.Format("{0,3} | {1}", area_totals[(MEO_Type.Other, Found_Type.FOUND)], total_other),
            ];
        }

        // --------------------------------------------------------------------------------------------------------------------------------------------
        public void Update_Area_Totals(bool reset = false)
        {
            total_missions = reset ? 0 : Total_By_Type(MEO_Type.Mission);
            total_enemies  = reset ? 0 : Total_By_Type(MEO_Type.Enemy);
            total_other    = reset ? 0 : Total_By_Type(MEO_Type.Other);

            foreach (var area in areas)
            {
                area.Update_Map_Totals(reset);

                for (int x = 0; x < 3; x++)
                    for (int y = 0; y < 3; y++)
                    {
                        MEO_Type mt = (MEO_Type)x;
                        Found_Type ft = (Found_Type)y;

                        if (reset)
                            area_totals[(mt, ft)] = 0;
                        else
                            area_totals[(mt, ft)] += area.map_totals[(mt, ft)];
                    }
            }
        }

        // --------------------------------------------------------------------------------------------------------------------------------------------
        public int Total_By_Type(MEO_Type type)
        {
            int total = 0;

            foreach (var area in areas)
                total += area.Total_By_Type(type);

            return total;
        }

        // --------------------------------------------------------------------------------------------------------------------------------------------
        public string[] Get_Loot_Type_Details_Row(Loot_Type type)
        {
            int total_found = 0;
            foreach (var loot in loot_lists[type])
                if (loot.found)
                    total_found++;

            return
            [
                total_found.ToString(),
                loot_lists[type].Count.ToString(),
                ((decimal)total_found / (decimal)loot_lists[type].Count).ToString("P1")
            ];
        }

        // --------------------------------------------------------------------------------------------------------------------------------------------
        public List<object[]> Search_Loot(string query)
        {
            query = query.ToLower();

            List<object[]> rows = [];

            foreach (var loot in loots)
                if (loot.Contains_Text(query))
                    rows.AddRange(loot.Get_Source_Rows(Found_Type.EITHER, Loot_Type.ANY));

            return rows;
        }

        // --------------------------------------------------------------------------------------------------------------------------------------------
        public List<object[]> Search_MEO(string query)
        {
            query = query.ToLower();

            List<object[]> rows = [];

            foreach (var area in areas)
                foreach (var map in area.maps)
                    foreach (var meo in map.meos)
                        if (meo.Contains_Text(query))
                            rows.Add(meo.Get_Detail_Row());

            return rows;
        }

        // --------------------------------------------------------------------------------------------------------------------------------------------
        public string Load_Seed(Seed seed)
        {
            if (seed != null)
            {
                Update_Area_Totals(true);

                this.seed = seed;

                if (!String.IsNullOrEmpty(seed.error_msg))
                    return seed.error_msg;

                for (int i = 0; i < loots.Count; i++)
                    loots[i].sources.Clear();

                Loot? loot;
                foreach (var seed_line in seed.all_seed_lines)
                {
                    loot = null;
                    if (seed_line.status > Seed_Line_Status.Nothing)
                    {
                        loot = loots.Find(o => o.name == seed_line.item_or_hint);
                        if (loot != null)
                        {
                            // LINK SEED LINE TO BL2 LOOT ITEM
                            loot.sources.Add(seed_line);
                            seed_line.loot = loot;
                        }
                    }

                    foreach (var area in areas)
                    foreach (var map in area.maps)
                    foreach (var meo in map.meos)
                        if (meo.name == seed_line.name)
                        {
                            // LINK SEED LINE TO BL2 MISSION / ENEMY / OTHER
                            seed_line.meo = meo;
                            meo.seed_line = seed_line;
                            meo.loot = loot;
                        }
                }

                Update_Area_Totals();

                return String.Empty;  // NO ERRORS
            }
            else
                return "The seed is NULL";
        }

        // --------------------------------------------------------------------------------------------------------------------------------------------
        public void Update_Lists_and_Totals()
        {
            foreach (var area in areas)
            {
                foreach (var map in area.maps)
                {
                    map.area = area;
                    foreach (var meo in map.meos)
                    {
                        meo.map = map;
                        meo.Update_Values();
                    }

                    map.Update_MEO_Totals();
                }

                area.Update_Map_Totals();
            }

            Update_Area_Totals();

            foreach (var loot in loots)
            {
                loot.Update_Values();

                loot_lists[Loot_Type.ANY].Add(loot);

                if (loot.is_weapon)
                    loot_lists[Loot_Type.ALL_WEAP].Add(loot);

                else if (loot.is_item)
                    loot_lists[Loot_Type.ALL_ITEM].Add(loot);

                loot_lists[loot.loot_type].Add(loot);
            }

            for (int i = 0; i < loot_lists.Count; i++)
                loot_lists[(Loot_Type)i] = loot_lists[(Loot_Type)i].OrderBy(o => o.name).ToList();
        }
    }
}
