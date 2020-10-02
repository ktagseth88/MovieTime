﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lol.dataloader
{
    [Table("match_participant", Schema = "lol")]
    public partial class MatchParticipant
    {
        [Key]
        [Column("match_participant_id")]
        public int MatchParticipantId { get; set; }
        [Column("match_id")]
        public int MatchId { get; set; }
        [Column("champion_id")]
        public int ChampionId { get; set; }
        [Required]
        [Column("role")]
        [StringLength(255)]
        public string Role { get; set; }
        [Required]
        [Column("lane")]
        [StringLength(255)]
        public string Lane { get; set; }
        [Column("summoner_spell_1")]
        public int SummonerSpell1 { get; set; }
        [Column("summoner_spell_2")]
        public int SummonerSpell2 { get; set; }
        [Column("team_id")]
        public int TeamId { get; set; }
        [Column("gold_earned")]
        public int GoldEarned { get; set; }
        [Column("total_damage_taken")]
        public int TotalDamageTaken { get; set; }
        [Column("champ_level")]
        public int ChampLevel { get; set; }
        [Column("creep_score")]
        public int CreepScore { get; set; }
        [Column("kills")]
        public int Kills { get; set; }
        [Column("deaths")]
        public int Deaths { get; set; }
        [Column("assists")]
        public int Assists { get; set; }
        [Column("crowd_control_dealt_duration")]
        public int CrowdControlDealtDuration { get; set; }
        [Column("total_damage_dealt")]
        public int TotalDamageDealt { get; set; }
        [Column("vision_wards")]
        public int VisionWards { get; set; }
        [Column("turret_kills")]
        public int TurretKills { get; set; }
        [Column("vision_score")]
        public long VisionScore { get; set; }
        [Column("first_blood_kill")]
        public bool FirstBloodKill { get; set; }
        [Column("first_blood_assist")]
        public bool FirstBloodAssist { get; set; }
        [Column("first_tower_kill")]
        public bool FirstTowerKill { get; set; }
        [Column("first_tower_assist")]
        public bool FirstTowerAssist { get; set; }
        [Column("enemy_jungle_minions_killed")]
        public int EnemyJungleMinionsKilled { get; set; }
        [Column("item_0_id")]
        public int Item0Id { get; set; }
        [Column("item_1_id")]
        public int Item1Id { get; set; }
        [Column("item_3_id")]
        public int Item3Id { get; set; }
        [Column("item_4_id")]
        public int Item4Id { get; set; }
        [Column("item_5_id")]
        public int Item5Id { get; set; }
    }
}