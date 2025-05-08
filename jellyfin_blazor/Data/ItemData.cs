namespace jellyfin_blazor.Data
{
    public class ItemData
    {
        public List<dynamic> Items { get; set; }
    }

    public enum ItemType
    {
        Movie,
        Series
    }
}
