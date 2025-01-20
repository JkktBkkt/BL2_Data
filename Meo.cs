namespace BL2
{ 
    public class Meo    // MISSION / ENEMY / OTHER
    {
        public static BL2_Data bl2;

        public string type { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public string rate { get; set; } = string.Empty;
        public string mission { get; set; } = string.Empty;
        public string options { get; set; } = string.Empty;
        public string giver { get; set; } = string.Empty;


        private string name_lowered = String.Empty;
        private string mission_lowered = String.Empty;
        private string giver_lowered = String.Empty;

        public MEO_Type meo_type = MEO_Type.Not_Set;

        public Map? map = null;
        public Seed_Line? seed_line = null;
        public Loot? loot = null;

        
        public string source_area { get { return String.Format("{0} | {1}", map.area.name, map.name); } }

        // --------------------------------------------------------------------------------------------------------------------------------------------
        public string reward
        {
            get
            {
                if (seed_line != null)
                {
                    if (seed_line.status == Seed_Line_Status.Nothing)
                        return "Nothing";

                    if (seed_line.status > Seed_Line_Status.Nothing)
                        return String.Format("{0}: {1}", loot?.weap_or_item_txt, loot?.Name());

                    if (seed_line.status == Seed_Line_Status.Hint)
                        return String.Format("Hint: {0}", seed_line.item_or_hint);
                }

                return String.Empty;
            }
        }

        // --------------------------------------------------------------------------------------------------------------------------------------------
        public bool found
        {
            get
            {
                if (loot != null)
                    return loot.found;
                else
                {
                    if (seed_line != null)
                        return seed_line.status == Seed_Line_Status.Nothing;
                    else
                        return false;
                }
            }
        }

        // --------------------------------------------------------------------------------------------------------------------------------------------
        public string source_text
        {
            get
            {
                switch (meo_type)
                {
                    case MEO_Type.Enemy:
                        if (String.IsNullOrEmpty(mission))
                            return "Area: " + map.name + " | " + map.area.name;
                        else
                            return "Mission: " + mission;

                    case MEO_Type.Mission:
                        return giver;

                    case MEO_Type.Other:
                        return "Area: " + map.name + " | " + map.area.name;
                }

                return String.Empty;
            }
        }

        // --------------------------------------------------------------------------------------------------------------------------------------------
        public bool Contains_Text(string query)
        {
            if (bl2.Options_Are_OK(options))
            {
                if (name_lowered.Contains(query)) return true;
                if (mission_lowered.Contains(query)) return true;
                if (giver_lowered.Contains(query)) return true;
            }

            return false;
        }

        // --------------------------------------------------------------------------------------------------------------------------------------------
        public void Update_Values()
        {
            meo_type = Seed_Line.Get_Type(type);

            name_lowered = name.ToLower();
            mission_lowered = mission.ToLower();
            giver_lowered = giver.ToLower();
        }

        // --------------------------------------------------------------------------------------------------------------------------------------------
        public object[] Get_Nothing_Row()
        {
            return
            [
                found,
                name,
                type,
                String.Empty,
                source_text,
                source_area,
                rate,
                String.Empty,
                options
            ];
        }

        // --------------------------------------------------------------------------------------------------------------------------------------------
        public object[] Get_Detail_Row()
        {
            return
            [
                found,
                type,
                name,
                reward,
                source_text,
                rate,
                options
            ];
        }
    }
}
