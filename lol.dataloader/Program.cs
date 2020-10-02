using lol.dataloader.LolResources;
using Microsoft.EntityFrameworkCore;
using MingweiSamuel.Camille;
using MingweiSamuel.Camille.Enums;
using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Threading;

namespace lol.dataloader
{
    class Program
    {
        private static RiotApi _riotApi;
        public static async Task Main(string[] args)
        {
            //await ChampLoader.LoadChamps();
            //await LoadAccounts();
            _riotApi = RiotApi.NewInstance("RGAPI-80b92ea3-0b69-4477-a3a2-feb258db0946");
            await LoadKeatonMatches();

        }

        private static async Task LoadKeatonMatches()
        {
            using var context = new MovietimeContext();
            var champs = context.Champion.ToList();
            var keatonAccountId = (await context.Summoner.FirstOrDefaultAsync(x => x.Name == "ktagseth")).AccountId;
            var matches = await _riotApi.MatchV4.GetMatchlistAsync(Region.NA, keatonAccountId);

            int apiRequestCount = 0;
            for(int i = 0; i < matches.Matches.Count(); i++)
            {
                if(apiRequestCount == 175)
                {
                    Thread.Sleep(1000 * 60 * 2);//wait 2 minutes for rate limit
                    apiRequestCount = 0;
                }
                var matchId = matches.Matches[i].GameId;
                var matchDetails = await _riotApi.MatchV4.GetMatchAsync(Region.NA, matchId);
                
                if(!context.Match.Any(x => x.RiotMatchId == matchId))
                {
                    var matchEntity = new Match
                    {
                        RiotMatchId = matchId,
                        GameDurationInSeconds = (int)matchDetails.GameDuration
                    };
                    context.Match.Add(matchEntity);
                    context.SaveChanges();

                    foreach(var participant in matchDetails.Participants)
                    {
                        context.MatchParticipant.Add(new MatchParticipant
                        {
                            ChampionId = champs.FirstOrDefault(x => x.RiotChampionId == participant.ChampionId).ChampionId,
                            Assists = participant.Stats.Assists,
                            FirstBloodAssist = participant.Stats.FirstBloodAssist,
                            FirstBloodKill = participant.Stats.FirstBloodKill,
                            FirstTowerAssist = participant.Stats.FirstTowerAssist,
                            FirstTowerKill = participant.Stats.FirstTowerKill,
                            ChampLevel = participant.Stats.ChampLevel,
                            Deaths = participant.Stats.Deaths,
                            TotalDamageDealt = (int)participant.Stats.TotalDamageDealt,
                            TotalDamageTaken = (int)participant.Stats.TotalDamageTaken,
                            CreepScore = participant.Stats.NeutralMinionsKilled,
                            CrowdControlDealtDuration = participant.Stats.TotalTimeCrowdControlDealt,
                            EnemyJungleMinionsKilled = participant.Stats.NeutralMinionsKilledEnemyJungle,
                            GoldEarned = participant.Stats.GoldEarned,
                            Kills = participant.Stats.Kills,
                            Lane = participant.Timeline.Lane,
                            Role = participant.Timeline.Role,
                            VisionWards = participant.Stats.VisionWardsBoughtInGame,
                            VisionScore = participant.Stats.VisionScore,
                            SummonerSpell1 = participant.Spell1Id,
                            SummonerSpell2 = participant.Spell2Id,
                            TurretKills = participant.Stats.TurretKills,
                            MatchId = matchEntity.MatchId,
                            MatchParticipantId = participant.ParticipantId,
                            TeamId = participant.TeamId,
                            Item0Id = participant.Stats.Item0,
                            Item1Id = participant.Stats.Item1,
                            Item3Id = participant.Stats.Item3,
                            Item4Id = participant.Stats.Item4,
                            Item5Id = participant.Stats.Item5
                        });

                        
                    }

                }

                apiRequestCount++;
            }

            int breakpls = 5;
        }

        private static async Task LoadAccounts()
        {
            var keaton = await _riotApi.SummonerV4.GetBySummonerNameAsync(Region.NA, "ktagseth");
            var griffin = await _riotApi.SummonerV4.GetBySummonerNameAsync(Region.NA, "pootloops");


            var keatonEntity = new Summoner
            {
                Name = "ktagseth",
                AccountId = keaton.AccountId,
                Puuid = keaton.Puuid,
                RiotSummonerId = keaton.Id
            };

            var griffinEntity = new Summoner
            {
                Name = "pootloops",
                AccountId = griffin.AccountId,
                Puuid = griffin.Puuid,
                RiotSummonerId = griffin.Id
            };

            using var context = new MovietimeContext();
            context.Summoner.Add(keatonEntity);
            context.Summoner.Add(griffinEntity);

            await context.SaveChangesAsync();
        }
    }
}
