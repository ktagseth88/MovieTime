using Microsoft.EntityFrameworkCore;
using MovieTime.Entities.Lol;
using Microsoft.Data.SqlClient;
using Dapper;
using MovieTime.Models.Lol;

namespace MovieTime.Services.Lol
{
    public class PlayerStatService
    {
        private readonly LolContext _lolContext;
        public PlayerStatService(LolContext lolContext)
        {
            _lolContext = lolContext;
        }

        public async Task<IEnumerable<PlayerChampionWinRate>> GetPlayerStats(string summonerName)
        {
            using var conn = new SqlConnection(_lolContext.Database.GetConnectionString());

            var results = await conn.QueryAsync<PlayerChampionWinRate>("""
                select 
                	c.name ChampionName,
                	count(case when ts.win = 0 then 1 end) wins,
                	count(case when ts.win = 1 then 1 end) losses,
                	cast(count(case when ts.win = 0 then 1 end) as float) / cast(count(c.name) as float) win_rate
                from lol.match m
                inner join lol.match_participant mp on m.match_id = mp.match_id
                inner join lol.champion c on mp.champion_id = c.champion_id 
                inner join 
                (
                	select 
                		m.match_id,
                		mp.team_id,
                		c.name playChampName
                	from lol.match m 
                	inner join lol.match_participant mp on m.match_id = mp.match_id
                	inner join lol.champion c on mp.champion_id = c.champion_id
                	inner join lol.summoner s on mp.summoner_id = s.riot_summoner_id
                	where s.name = @summonerName 
                ) as myMatchIds 
                	on myMatchIds.match_id = m.match_id 
                		and mp.team_id != myMatchIds.team_id
                inner join lol.team_stats ts 
                	on m.match_id = ts.match_id 
                		and ts.team_id != myMatchIds.team_id
                group by c.name
                order by ChampionName
                """, new { summonerName});

            return results;
        }
    }
}
