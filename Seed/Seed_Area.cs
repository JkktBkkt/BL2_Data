namespace BL2
{ 
    public class Seed_Area
    {
        public string name = String.Empty;

        public List<Seed_Line> everything = [];

        public List<Seed_Line> missions = [];
        public List<Seed_Line> enemies = [];
        public List<Seed_Line> other = [];

        public List<Seed_Line> hints = [];
        public List<Seed_Line> nothings = [];

        public int num_found = 0;


        // -------------------------------------------------------------------------------------------------------------------------------------
        public void Add_Line(Seed_Line line)
        {
            if (line.data_ok)
            {
                everything.Add(line);

                switch (line.status)
                {
                    case Seed_Line_Status.Nothing:
                        nothings.Add(line);
                        break;

                    case Seed_Line_Status.Hint:
                        hints.Add(line);
                        break;

                    case Seed_Line_Status.Identified:
                    case Seed_Line_Status.Duplicate:
                        num_found++;
                        break;
                }

                switch (line.meo_type)
                {
                    case MEO_Type.Mission:
                        missions.Add(line);
                        break;
                    case MEO_Type.Enemy:
                        enemies.Add(line);
                        break;
                    case MEO_Type.Other:
                        other.Add(line);
                        break;
                }
            }
        }
    }
}
