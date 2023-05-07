namespace MovieTime.Models.Lol
{
    public class PlayerChampionWinRate
    {
        public string ChampionName { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public float WinRate { get; set; }
    }
}