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
        public int XCoordinate { get; }
        public int YCoordinate { get; }
        public string Name { get; }
        public string Description { get; }
        public string ImageName { get; }
        public List<Quest> QuestsAvailableHere { get; } = new List<Quest>();
        public List<MonsterEncounter> MonstersHere { get; } = new List<MonsterEncounter>();
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
                            .ChanceOfEncountering = chanceOfEncountering;
            }
            else
            {
                // 否则，在此位置添加该Monster的生成概率
                MonstersHere.Add(new MonsterEncounter(monsterID, chanceOfEncountering));
            }
        }

        /// <summary>
        /// 由本Location的所有Monster生成概率，随机生成一个Monster
        /// </summary>
        public Monster GetMonster()
        {
            if (!MonstersHere.Any())
            {
                return null;
            }

            int totalChances = MonstersHere.Sum(m => m.ChanceOfEncountering);
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
