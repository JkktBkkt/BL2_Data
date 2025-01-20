namespace BL2
{ 
    public class Area
    {
        public string name { get; set; } = string.Empty;
        public List<Map> maps { get; set; } = [];


        public Dictionary<(MEO_Type, Found_Type), int> map_totals = new()
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
        public string[] MEO_Detail_Row()
        {
            return
            [
                total_missions == 0 ? String.Empty : String.Format("{0,3} | {1}", map_totals[(MEO_Type.Mission, Found_Type.FOUND)], total_missions),
                total_enemies == 0 ? String.Empty : String.Format("{0,3} | {1}", map_totals[(MEO_Type.Enemy, Found_Type.FOUND)], total_enemies),
                total_other == 0 ? String.Empty : String.Format("{0,3} | {1}", map_totals[(MEO_Type.Other, Found_Type.FOUND)], total_other),
            ];
        }

        // --------------------------------------------------------------------------------------------------------------------------------------------
        public void Update_Map_Totals(bool reset = false)
        {
            total_missions = reset ? 0 : Total_By_Type(MEO_Type.Mission);
            total_enemies  = reset ? 0 : Total_By_Type(MEO_Type.Enemy);
            total_other    = reset ? 0 : Total_By_Type(MEO_Type.Other);

            foreach (var map in maps)
            {
                map.Update_MEO_Totals(reset);

                for (int x = 0; x < 3; x++)
                    for (int y = 0; y < 3; y++)
                    {
                        MEO_Type mt = (MEO_Type)x;
                        Found_Type ft = (Found_Type)y;

                        if (reset)
                            map_totals[(mt, ft)] = 0;
                        else
                            map_totals[(mt, ft)] += map.meo_totals[(mt, ft)];
                    }
            }
        }

        // --------------------------------------------------------------------------------------------------------------------------------------------
        public int Total_By_Type(MEO_Type type)
        {
            int total = 0;

            foreach (var map in maps)
                total += map.Total_By_Type(type);

            return total;
        }
    }
}
