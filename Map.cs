namespace BL2
{
    public class Map
    {
        public static BL2_Data bl2;

        public string name { get; set; } = string.Empty;
        public List<Meo> meos { get; set; } = [];


        public Area? area = null;
        
        public Dictionary<(MEO_Type, Found_Type), int> meo_totals = new()
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
        public List<Meo> Get_MEO_List(Found_Type found_type)
        {
            List<Meo> matching = [];

            foreach (var meo in meos)
                if (bl2.Options_Are_OK(meo.options))
                    switch (found_type)
                    {
                        case Found_Type.FOUND:
                            if (meo.found)
                                matching.Add(meo);
                            break;

                        case Found_Type.NOT_FOUND:
                            if (!meo.found)
                                matching.Add(meo);
                            break;

                        default:
                            matching.Add(meo);
                            break;
                    }

            return matching;
        }

        // --------------------------------------------------------------------------------------------------------------------------------------------
        public string[] Map_MEO_Detail_Row(Found_Type found_type = Found_Type.FOUND)
        {
            return
            [
                total_missions == 0 ? String.Empty : String.Format("{0,3} | {1}", meo_totals[(MEO_Type.Mission, found_type)], total_missions),
                total_enemies  == 0 ? String.Empty : String.Format("{0,3} | {1}", meo_totals[(MEO_Type.Enemy, found_type)], total_enemies),
                total_other    == 0 ? String.Empty : String.Format("{0,3} | {1}", meo_totals[(MEO_Type.Other, found_type)], total_other),
            ];
        }

        // --------------------------------------------------------------------------------------------------------------------------------------------
        public void Update_MEO_Totals(bool reset = false)
        {
            total_missions = reset ? 0 : Total_By_Type(MEO_Type.Mission);
            total_enemies  = reset ? 0 : Total_By_Type(MEO_Type.Enemy);
            total_other    = reset ? 0 : Total_By_Type(MEO_Type.Other);

            for (int x = 0; x < 3; x++)
                for (int y = 0; y < 3; y++)
                {
                    MEO_Type mt = (MEO_Type)x;
                    Found_Type ft = (Found_Type)y;

                    if (reset)
                        meo_totals[(mt, ft)] = 0;
                    else
                        switch (ft)
                        {
                            case Found_Type.FOUND:
                                foreach (var meo in meos)
                                    if (meo.meo_type == mt && meo.found == true)
                                        meo_totals[(mt, ft)]++;
                                break;

                            case Found_Type.NOT_FOUND:
                                foreach (var meo in meos)
                                    if (meo.meo_type == mt && meo.found == false)
                                        meo_totals[(mt, ft)]++;
                                break;

                            case Found_Type.EITHER:
                                foreach (var meo in meos)
                                    if (meo.meo_type == mt)
                                        meo_totals[(mt, ft)]++;
                                break;
                        }
                }
        }

        // --------------------------------------------------------------------------------------------------------------------------------------------
        public int Total_By_Type(MEO_Type type)
        {
            int total = 0;

            foreach (var meo in meos)
                if (meo.meo_type == type && bl2.Options_Are_OK(meo.options))
                    total++;

            return total;
        }

    }
}
