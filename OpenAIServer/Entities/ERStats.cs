namespace OpenAIServer.Entities
{
    public class ERStats
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? Level { get; set; }
        public int? GreatRune { get; set; }
        public int? HP { get; set; }
        public int? MaxHP { get; set; }
        public int? Deaths { get; set; }
        public int? Runes { get; set; }
        public string? Class { get; set; }
        public string? Gender { get; set; }
        public string? Location { get; set; }
        public string? PrimaryWeapon { get; set; }
        public string? SecondaryWeapon { get; set; }
        public string? TertiaryWeapon { get; set; }
        public string? LastEnemyFought { get; set; }
        public int? ResposneCount { get; set; }
        public int? Events { get; set; }
        public List<string>? EventList { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
