﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lol.dataloader
{
    [Table("team_stats", Schema = "lol")]
    public partial class TeamStats
    {
        [Key]
        [Column("team_stats_id")]
        public int TeamStatsId { get; set; }
        [Column("match_id")]
        public int MatchId { get; set; }
        [Column("tower_kills")]
        public int TowerKills { get; set; }
        [Column("first_blood")]
        public bool FirstBlood { get; set; }
        [Column("inhibitor_kills")]
        public int InhibitorKills { get; set; }
        [Column("first_dragon")]
        public bool FirstDragon { get; set; }
        [Column("dragon_kills")]
        public int DragonKills { get; set; }
        [Column("first_inhibitor")]
        public bool FirstInhibitor { get; set; }
        [Column("first_tower")]
        public bool FirstTower { get; set; }
        [Column("first_rift_herald")]
        public bool FirstRiftHerald { get; set; }
        [Column("team_id")]
        public int TeamId { get; set; }
        [Column("win")]
        public bool Win { get; set; }
    }
}