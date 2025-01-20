namespace BL2
{ 
    public class Loot
    {
        public static BL2_Data? bl2;

        public string type { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public string rarity { get; set; } = string.Empty;
        public string options { get; set; } = string.Empty;
        public string hint { get; set; } = string.Empty;


        private string? type_lowered;
        private string? name_lowered;
        private string? rarity_lowered;
        private string? area_lowered;
        private string? hint_lowered;

        public string weap_or_item_txt = String.Empty;
        public string short_type_txt = String.Empty;

        public Loot_Type loot_type = Loot_Type.NONE;

        public bool error = false;

        public List<Seed_Line> sources = [];

        public bool is_weapon { get { return loot_type >= Loot_Type.AR && loot_type <= Loot_Type.SNIPER; } }
        public bool is_item   { get { return loot_type >= Loot_Type.COM && loot_type <= Loot_Type.SHIELD; } }
        public bool has_dupes { get { return sources.Any(o => o.status == Seed_Line_Status.Duplicate); } }
        public bool found     { get { return sources.Any(o => o.status >= Seed_Line_Status.Nothing); } }



        // --------------------------------------------------------------------------------------------------------------------------------------------
        public bool Contains_Text(string query)
        {
            if (bl2.Options_Are_OK(options))
            {
                if (type_lowered!.Contains(query)) return true;
                if (name_lowered!.Contains(query)) return true;
                if (rarity_lowered!.Contains(query)) return true;
                if (area_lowered!.Contains(query)) return true;
                if (hint_lowered!.Contains(query)) return true;
            }

            return false;
        }

        // --------------------------------------------------------------------------------------------------------------------------------------------
        public bool Check_Type(Loot_Type filter_type)
        {
            switch (filter_type)
            {
                case Loot_Type.ALL_WEAP:
                    return is_weapon;

                case Loot_Type.ALL_ITEM:
                    return is_item;

                case Loot_Type.ANY:
                    return true;

                default:
                    return loot_type == filter_type;
            }
        }

        // --------------------------------------------------------------------------------------------------------------------------------------------
        public bool Check_Found(Found_Type found)
        {
            switch (found)
            {
                case Found_Type.FOUND:
                    return this.found;

                case Found_Type.NOT_FOUND:
                    return !this.found;

                default:
                    return true;
            }
        }

        // --------------------------------------------------------------------------------------------------------------------------------------------
        public List<object[]> Get_Hint_Rows(Found_Type found_type, Loot_Type filter_type, bool name_with_type = false)
        {
            int i = 0;

            List<object[]> rows = [];

            if (Check_Found(found_type) && Check_Type(filter_type) && bl2.Options_Are_OK(options))
            {
                do
                {
                    rows.Add(new object[]
                    {
                        found,
                        hint,
                        name,
                        sources.Count > i ? sources[i].source      : String.Empty,
                        sources.Count > i ? sources[i].source_area : String.Empty,
                        options
                    });

                } while (++i < sources.Count);
            }

            return rows;
        }

        // --------------------------------------------------------------------------------------------------------------------------------------------
        public List<object[]> Get_Source_Rows(Found_Type found_type, Loot_Type filter_type, bool name_with_type = false)
        {
            int i = 0;
            List<object[]> rows = [];

            if (Check_Found(found_type) && Check_Type(filter_type) && bl2.Options_Are_OK(options))
            {
                do
                {
                    rows.Add(new object[]
                    {
                        found,
                        Name(name_with_type),
                        short_type_txt,
                        rarity,
                        sources.Count > i ? sources[i].source      : String.Empty,
                        sources.Count > i ? sources[i].source_area : String.Empty,
                        sources.Count > i ? sources[i].meo.rate    : String.Empty,
                        hint,
                        options
                    });

                } while (++i < sources.Count);
            }

            return rows;
        }

        // --------------------------------------------------------------------------------------------------------------------------------------------
        public string Name(bool with_type = true)
        {
            // CHECK FOR COM WITH LONG NAME TO BE SHORTENED
            if (loot_type == Loot_Type.COM && !name.StartsWith("Slayer") && BL2_Data.char_class != Char_Class.NOT_SET)
            {
                int first_space = name.IndexOf(' ');
                string rarity = name.Substring(0, first_space);
                string coms = name.Substring(first_space + 1);
                string[] parts = coms.Split(" / ", StringSplitOptions.RemoveEmptyEntries);

                return String.Format("{0} {1}{2}", rarity, parts[(int)BL2_Data.char_class], with_type ? String.Format(" {0}", short_type_txt) : String.Empty);
            }
            else
            {
                if (with_type)
                    return name.EndsWith(short_type_txt) ? name : String.Format("{0} {1}", name, short_type_txt);
                else
                    return name;
            }
        }

        // --------------------------------------------------------------------------------------------------------------------------------------------
        public void Update_Values()
        {
            type_lowered = type.ToLower();
            name_lowered = name.ToLower();
            rarity_lowered = rarity.ToLower();
            hint_lowered = hint.ToLower();
            area_lowered = options.ToLower();

            switch (type)
            {
                case "Assault Rifle":
                    weap_or_item_txt = "Weapon";
                    short_type_txt = "AR";
                    loot_type = Loot_Type.AR;
                    break;

                case "Launcher":
                    weap_or_item_txt = "Weapon";
                    short_type_txt = "Launcher";
                    loot_type = Loot_Type.LAUNCHER;
                    break;

                case "Pistol":
                    weap_or_item_txt = "Weapon";
                    short_type_txt = "Pistol";
                    loot_type = Loot_Type.PISTOL;
                    break;

                case "Shotgun":
                    weap_or_item_txt = "Weapon";
                    short_type_txt = "Shotgun";
                    loot_type = Loot_Type.SHOTGUN;
                    break;

                case "SMG":
                    weap_or_item_txt = "Weapon";
                    short_type_txt = "SMG";
                    loot_type = Loot_Type.SMG;
                    break;

                case "Sniper Rifle":
                    weap_or_item_txt = "Weapon";
                    short_type_txt = "Sniper";
                    loot_type = Loot_Type.SNIPER;
                    break;

                case "COM":
                    weap_or_item_txt = "Item";
                    short_type_txt = "COM";
                    loot_type = Loot_Type.COM;
                    break;

                case "Relic":
                    weap_or_item_txt = "Item";
                    short_type_txt = "Relic";
                    loot_type = Loot_Type.RELIC;
                    break;

                case "Grenade":
                    weap_or_item_txt = "Item";
                    short_type_txt = "Grenade";
                    loot_type = Loot_Type.GRENADE;
                    break;

                case "Shield":
                    weap_or_item_txt = "Item";
                    short_type_txt = "Shield";
                    loot_type = Loot_Type.SHIELD;
                    break;

                default:
                    error = true;
                    break;
            }
        }
    }
}