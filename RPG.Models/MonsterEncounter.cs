namespace RPG.Models
{
    // 存储生成概率
    public class MonsterEncounter
    {
        public int MonsterID { get; }
        public int ChanceOfEncounter { get; set; }

        public MonsterEncounter(int monsterID, int chanceOfEncounter)
        {
            MonsterID = monsterID;
            ChanceOfEncounter = chanceOfEncounter;
        }
    }
}
