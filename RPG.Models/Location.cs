using Newtonsoft.Json;

namespace RPG.Models
{
    public class Location
    {
        public int XCoordinate { get; }
        public int YCoordinate { get; }
        [JsonIgnore]
        public string Name { get; }
        [JsonIgnore]
        public string Description { get; }
        [JsonIgnore]
        public string ImageName { get; }
        [JsonIgnore]
        public List<Quest> QuestsAvailableHere { get; } = new List<Quest>();
        [JsonIgnore]
        public List<MonsterEncounter> MonstersHere { get; } = new List<MonsterEncounter>();
        [JsonIgnore]
        public Trader? TraderHere { get; set; }
        public Location(int xCoordinate, int yCoordinate, string name, string description, string imageName)
        {
            XCoordinate = xCoordinate;
            YCoordinate = yCoordinate;
            Name = name;
            Description = description;
            ImageName = imageName;
        }
        /// <summary>
        /// 在Location添加Monster生成概率，如已有则更新概率
        /// </summary>
        public void AddMonsterEncounter(int monsterID, int chanceOfEncountering)
        {
            if (MonstersHere.Exists(m => m.MonsterID == monsterID))
            {
                // 如果Monster已经在这个位置，更新它的生成概率
                MonstersHere.First(m => m.MonsterID == monsterID)
                            .ChanceOfEncounter = chanceOfEncountering;
            }
            else
            {
                // 否则，在此位置添加该Monster的生成概率
                MonstersHere.Add(new MonsterEncounter(monsterID, chanceOfEncountering));
            }
        }
    }
}
