﻿using Engine.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class Location
    {
        public int XCoordinate { get; set; }
        public int YCoordinate { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageName { get; set; }
        public List<Quest> QuestsAvailableHere { get; set; } = new List<Quest>();

        public List<MonsterEncounter> MonstersHere { get; set; } = new List<MonsterEncounter>();

        // 为当前Location更新MonsterEncounter列表
        public void UpdateMonsterEncounter(int monsterID, int chanceOfEncountering)
        {
            if (MonstersHere.Exists(m => m.MonsterID == monsterID))
            {
                // 如果Monster已经在这个位置，更新它的生成概率
                MonstersHere.First(m => m.MonsterID == monsterID)
                            .ChanceOfEncountering = chanceOfEncountering;
            }
            else
            {
                // 否则，在此位置添加该Monster的生成概率
                MonstersHere.Add(new MonsterEncounter(monsterID, chanceOfEncountering));
            }
        }

        // 
        public Monster GetMonster()
        {
            if (!MonstersHere.Any())
            {
                return null;
            }

            // 计算这个位置的所有怪物的生成概率总和
            int totalChances = MonstersHere.Sum(m => m.ChanceOfEncountering);
            // 选择1到总和之间的一个随机数
            int randomNumber = RandomNumberGenerator.NumberBetween(1, totalChances);

            // 累加生成概率，直到它大于随机数的值，生成当前对应怪物
            int nowTotal = 0;
            foreach (MonsterEncounter monsterEncounter in MonstersHere)
            {
                nowTotal += monsterEncounter.ChanceOfEncountering;
                if (randomNumber <= nowTotal)
                {
                    return MonsterFactory.GetMonster(monsterEncounter.MonsterID);
                }
            }
            // 保证生成
            return MonsterFactory.GetMonster(MonstersHere.Last().MonsterID);
        }
    }
}
