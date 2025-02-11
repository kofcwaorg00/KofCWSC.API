namespace KofCWSC.API.Models
{
    public partial class RollCallSheets
    {
        public string GroupName { get; set; }
        public string Council { get; set; }
        public string Name { get; set; }
        public int District { get; set; }
        public string Day {  get; set; }
        public int SortOrder { get; set; }
        public int DirSortOrder { get; set; }
    }
}
