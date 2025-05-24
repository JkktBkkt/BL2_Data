namespace BL2
{
    public class Seed
    {
        public FileInfo? fi = null;
        public string file_name = String.Empty;

        public Seed_Options options = new();
        public List<Seed_Area> areas = [];
        public List<Seed_Line> all_seed_lines = [];
        public List<Seed_Line> duplicates = [];
        public List<Seed_Line> nothings = [];
        public List<Seed_Line> hints = [];

        public int total_locations = 0;

        public int found_duplicates_or_nothings = 0;
        public int total_duplicates_or_nothings = 0;
        public decimal percent_duplicates_or_nothings = 0;

        public int found_items = 0;
        public int total_items = 0;
        public decimal percent_items = 0;

        public int total_hints = 0;
        
        public string error_msg = String.Empty;


        // -------------------------------------------------------------------------------------------------------------------------------------
        public object[] Get_Details_Row()
        {
            if (String.IsNullOrEmpty(error_msg) && fi != null) 
                return
                [
                    " " + file_name,
                    " " + fi!.LastWriteTime.ToString("g"),
                    " " + total_locations,
                    String.Format(" {0,3}  |  {1,3}  |  {2,4:P1}", found_items, total_items, percent_items),
                    String.Format(" {0,3}  |  {1,3}  |  {2,4:P1}", found_duplicates_or_nothings, total_duplicates_or_nothings, percent_duplicates_or_nothings),
                    " " + total_hints
                ];

            return [];
        }

        // -------------------------------------------------------------------------------------------------------------------------------------
        public Seed(string full_path)
        {
            Seed_Area? area = null;
            string[] lines;

            try
            {
                fi = new FileInfo(full_path);
                lines = File.ReadAllLines(full_path);
            }
            catch (Exception ex)
            {
                error_msg = String.Format("Could not read seed file:\n\n{0}The error was:\n\n{1}", full_path, ex.Message);
                return;
            }

            file_name = Path.GetFileName(full_path);

            if (lines.Length > 3)
            {
                string[] parts = lines[2].Split([": "], StringSplitOptions.RemoveEmptyEntries);
                total_locations = int.Parse(parts[1]);

                parts = lines[3].Split([": "], StringSplitOptions.RemoveEmptyEntries);
                total_items = int.Parse(parts[1]);

                total_duplicates_or_nothings = total_locations - total_items;

                // READ SEED OPTIONS
                int i;
                for (i = 5; i < 42; i++)
                    if (lines[i]=="Borderlands 2")
                    {
                        break;
                    }
                    else if (lines[i]=="")
                    {
                        continue;
                    }
                    else
                    {
                        options.Set_Option(lines[i]);
                    }

                // 37 = Borderlands 2
                for (; i < lines.Length; i++)
                {
                    Seed_Line seed_line = new(lines[i]);

                    if (seed_line.is_area_heading)
                    {
                        area = new() { name = lines[i] };
                        areas.Add(area);
                    }
                    else if (seed_line.data_ok)
                    {
                        area!.Add_Line(seed_line);

                        switch (seed_line.status)
                        {
                            case Seed_Line_Status.Hint:
                                hints.Add(seed_line);
                                total_hints++;
                                break;

                            case Seed_Line_Status.Nothing:
                                nothings.Add(seed_line);
                                found_duplicates_or_nothings++;
                                break;

                            case Seed_Line_Status.Identified:
                                var existing_sl = all_seed_lines.Find(o => o.item_or_hint == seed_line.item_or_hint);
                                if (existing_sl != null)
                                {
                                    duplicates.Add(existing_sl);
                                    found_duplicates_or_nothings++;
                                }
                                else
                                    found_items++;
                                break;

                            case Seed_Line_Status.Not_Checked:
                            default:
                                break;
                        }

                        all_seed_lines.Add(seed_line);
                    }
                }

                percent_duplicates_or_nothings = (decimal)found_duplicates_or_nothings / (decimal)total_duplicates_or_nothings;

                percent_items = (decimal)found_items / (decimal)total_items;
            }

            duplicates = duplicates.OrderBy(o => o.item_or_hint).ToList();
            nothings = nothings.OrderBy(o => o.name).ToList();
        }
    }
}
