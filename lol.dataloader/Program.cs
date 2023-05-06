using lol.dataloader.LolResources;
using Microsoft.EntityFrameworkCore;
using MingweiSamuel.Camille;
using MingweiSamuel.Camille.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace lol.dataloader
{
    class Program
    {
        private static List<string> _usernames = new List<string> { "ktagseth", "PootLoops" };
        private static RiotApi _riotApi;
        public static async Task Main(string[] args)
        {
            //await ChampLoader.LoadChamps();
            _riotApi = RiotApi.NewInstance("");
            //await LoadAccounts();
            //await UpdateMatches();
            foreach (var username in _usernames)
            {
                await LoadMatchesForUser(username);
                Console.WriteLine($"Finished loading matches for: {username}");
            }

            Console.WriteLine("done. Press any key to continue");
            Console.Read();
        }

        private static async Task UpdateMatches()
        {
            using var context = new MovietimeContext();

            var riotMatchIds = await context.Match.Select(x => x.RiotMatchId).ToListAsync();
            int count = 0;
            foreach(var matchId in riotMatchIds)
            {
                var riotMatch = _riotApi.MatchV4.GetMatch(Region.NA, matchId);

                var matchEntity = context.Match.FirstOrDefault(m => m.RiotMatchId == matchId);

                matchEntity.GameVersion = riotMatch.GameVersion;
                matchEntity.Season = riotMatch.SeasonId;
                matchEntity.QueueType = riotMatch.QueueId;

                context.SaveChanges();
                count += 1;
                Console.WriteLine($"{count}");
            }
        }

        private static async Task LoadMatchesForUser(string accountName)
        {
            using var context = new MovietimeContext();
            var champs = context.Champion.ToList();
            var accountInfo = (await context.Summoner.FirstOrDefaultAsync(x => x.Name == accountName));
            var riotMatches = await _riotApi.MatchV4.GetMatchlistAsync(Region.NA, accountInfo.AccountId, queue: new[] { 440, 420 });

            int apiRequestCount = 0;
            for(int i = 0; i < riotMatches.TotalGames; i++)
            {
                if(i != 0 && (i % 100 == 0))
                {
                    riotMatches = await _riotApi.MatchV4.GetMatchlistAsync(Region.NA, accountInfo.AccountId, beginIndex: i, queue: new[] { 440, 420 });
                }
                if(apiRequestCount == 175)
                {
                    Console.WriteLine("sleeping for api request limit");
                    //Thread.Sleep(1000 * 60 * 2);//wait 2 minutes for rate limit
                    apiRequestCount = 0;
                }

                Console.WriteLine(i);

                if(riotMatches.Matches.ElementAtOrDefault(i % 100) is null)
                {
                    break;
                }

                var riotMatchId = riotMatches.Matches[i % 100].GameId;
                
                if(!context.Match.Any(x => x.RiotMatchId == riotMatchId))
                {
                    var matchDetails = await _riotApi.MatchV4.GetMatchAsync(Region.NA, riotMatchId);
                    var matchEntity = new Match
                    {
                        RiotMatchId = riotMatchId,
                        GameDurationInSeconds = (int)matchDetails.GameDuration,
                        Season = matchDetails.SeasonId,
                        QueueType = matchDetails.QueueId,
                        GameVersion = matchDetails.GameVersion
                    };

                    context.Match.Add(matchEntity);
                    context.SaveChanges();

                    foreach(var teamDetails in matchDetails.Teams)
                    {
                        var teamStats = new TeamStats
                        {
                            MatchId = matchEntity.MatchId,
                            DragonKills = teamDetails.DragonKills,
                            FirstDragon = teamDetails.FirstDragon,
                            FirstBlood = teamDetails.FirstBlood,
                            TeamId = teamDetails.TeamId,
                            FirstInhibitor = teamDetails.FirstInhibitor,
                            FirstTower = teamDetails.FirstTower,
                            FirstRiftHerald = teamDetails.FirstRiftHerald,
                            InhibitorKills = teamDetails.InhibitorKills,
                            TowerKills = teamDetails.TowerKills,
                            Win = teamDetails.Win == "Win"
                        };
                        context.TeamStats.Add(teamStats);
                        await context.SaveChangesAsync();

                        var banEntities = teamDetails.Bans.Select(x => new TeamBans
                        {
                            ChampionId = champs.FirstOrDefault(y => y.RiotChampionId == x.ChampionId)?.ChampionId ?? -1,
                            PickTurn = x.PickTurn,
                            TeamStatsId = teamStats.TeamStatsId
                        });

                        context.TeamBans.AddRange(banEntities);
                        await context.SaveChangesAsync();
                    }

                    foreach(var participant in matchDetails.Participants)
                    {
                        context.MatchParticipant.Add(new MatchParticipant
                        {
                            MatchId = matchEntity.MatchId,
                            SummonerId = matchDetails.ParticipantIdentities[participant.ParticipantId - 1].Player.SummonerId,
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
                            TeamId = participant.TeamId,
                            Item0Id = participant.Stats.Item0,
                            Item1Id = participant.Stats.Item1,
                            Item3Id = participant.Stats.Item3,
                            Item4Id = participant.Stats.Item4,
                            Item5Id = participant.Stats.Item5
                        });

                    }
                    await context.SaveChangesAsync();
                }

                apiRequestCount++;
            }
        }

        private static async Task LoadAccounts()
        {
            using var context = new MovietimeContext();
            
            foreach(var username in _usernames)
            {
                var user = await _riotApi.SummonerV4.GetBySummonerNameAsync(Region.NA, username);

                if(!context.Summoner.Any(x => x.AccountId == user.AccountId))
                {
                    var summonerEntity = new Summoner
                    {
                        Name = username,
                        AccountId = user.AccountId,
                        Puuid = user.Puuid,
                        RiotSummonerId = user.Id
                    };
                    context.Summoner.Add(summonerEntity);
                }
            }

            await context.SaveChangesAsync();

            //var keaton = await _riotApi.SummonerV4.GetBySummonerNameAsync(Region.NA, "ktagseth");
            //var griffin = await _riotApi.SummonerV4.GetBySummonerNameAsync(Region.NA, "pootloops");
            //var zidane = await _riotApi.SummonerV4.GetBySummonerNameAsync(Region.NA, "zidaneslasthope");

            //var keatonEntity = new Summoner
            //{
            //    Name = "ktagseth",
            //    AccountId = keaton.AccountId,
            //    Puuid = keaton.Puuid,
            //    RiotSummonerId = keaton.Id
            //};

            //var griffinEntity = new Summoner
            //{
            //    Name = "pootloops",
            //    AccountId = griffin.AccountId,
            //    Puuid = griffin.Puuid,
            //    RiotSummonerId = griffin.Id
            //};

            //using var context = new MovietimeContext();
            //context.Summoner.Add(keatonEntity);
            //context.Summoner.Add(griffinEntity);

            //await context.SaveChangesAsync();
        }
    }
}
