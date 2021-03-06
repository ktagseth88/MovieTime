﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lol.dataloader
{
    [Table("summoner", Schema = "lol")]
    public partial class Summoner
    {
        [Key]
        public int SummonerId { get; set; }
        [Required]
        [StringLength(56)]
        public string AccountId { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        [Required]
        [StringLength(63)]
        public string RiotSummonerId { get; set; }
        [Required]
        [StringLength(78)]
        public string Puuid { get; set; }
    }
}