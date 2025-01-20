namespace BL2
{
    public enum Seed_Line_Status { Not_Checked = 0, Hint = 1, Nothing = 2, Identified = 3, Duplicate = 4 }

    public class Seed_Line
    {
        public readonly static List<string> area_headings =
        [
            "Borderlands 2",
            "Captain Scarlett and her Pirate's Booty",
            "Mr. Torgue's Campaign of Carnage",
            "Sir Hammerlock's Big Game Hunt",
            "Tiny Tina's Assault on Dragon Keep",
            "Commander Lilith & the Fight for Sanctuary",
            "Headhunter 1: Bloody Harvest",
            "Headhunter 2: Wattle Gobbler",
            "Headhunter 3: Mercenary Day",
            "Headhunter 4: Wedding Day Massacre",
            "Headhunter 5: Son of Crawmerax",
            "Ultimate Vault Hunter Upgrade Pack 2"
        ];

        public readonly static List<string> hints =
        [
            "Blue Class Mod",
            "E-tech Relic",
            "E-tech Weapon",
            "Effervescent Item",
            "Effervescent Weapon",
            "Legendary Assault Rifle",
            "Legendary Class Mod",
            "Legendary Grenade",
            "Legendary Pistol",
            "Legendary Rocket Launcher",
            "Legendary SMG",
            "Legendary Shield",
            "Legendary Shotgun",
            "Legendary Sniper Rifle",
            "Pearlescent Weapon",
            "Purple Assault Rifle",
            "Purple Class Mod",
            "Purple Grenade",
            "Purple Pistol",
            "Purple Relic",
            "Purple Rocket Launcher",
            "Purple SMG",
            "Purple Shield",
            "Purple Shotgun",
            "Purple Sniper Rifle",
            "Seraph Item",
            "Seraph Weapon",
            "Unique Assault Rifle",
            "Unique Grenade",
            "Unique Pistol",
            "Unique Relic",
            "Unique Rocket Launcher",
            "Unique SMG",
            "Unique Shield",
            "Unique Shotgun",
            "Unique Sniper Rifle"
        ];

        public string meo_type_txt = String.Empty;
        public string name = String.Empty;
        public string item_or_hint = String.Empty;

        public bool data_ok = false;
        public bool is_area_heading = false;

        public MEO_Type meo_type = MEO_Type.Not_Set;
        public Seed_Line_Status status = Seed_Line_Status.Not_Checked;

        public Loot? loot = null;
        public Meo? meo = null;


        public string source { get { return string.Format("{0}: {1}", meo.type, name); } }
        public string source_area { get { return string.Format("{0} | {1}", meo?.map?.area?.name, meo?.map?.name); } }

        // ------------------------------------------------------------------------------
        public Seed_Line(string line)
        {
            if (String.IsNullOrEmpty(line))
                return;

            if (area_headings.Contains(line))
            {
                is_area_heading = true;
                return;
            }

            string[] parts = line.Split(':', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length > 1)
            {
                data_ok = true;

                meo_type_txt = parts[0].Trim();

                meo_type = Get_Type(meo_type_txt);

                string name_and_item = line.Substring(meo_type_txt.Length + 2);
                parts = name_and_item.Split(" - ", StringSplitOptions.RemoveEmptyEntries);
                name = parts[0].Trim();

                if (parts.Length > 1)
                {
                    item_or_hint = parts[1].Trim();

                    if (hints.Contains(item_or_hint))
                        status = Seed_Line_Status.Hint;
                    else if (item_or_hint == "Nothing")
                        status = Seed_Line_Status.Nothing;
                    else
                        status = Seed_Line_Status.Identified;
                }
                else
                    status = Seed_Line_Status.Not_Checked;
            }
        }

        // ------------------------------------------------------------------------------
        public object[] Get_Hint_Row()
        {
            return
            [
                item_or_hint,
                name,
                meo.mission,
                meo.map.name,
                meo.map.area.name
            ];
        }

        // ------------------------------------------------------------------------------
        public static MEO_Type Get_Type(string type)
        {
            if (type == "Mission")
                return MEO_Type.Mission;
            else if (type == "Enemy")
                return MEO_Type.Enemy;
            else if (type == "Other")
                return MEO_Type.Other;

            return MEO_Type.Not_Set;
        }
    }
}
