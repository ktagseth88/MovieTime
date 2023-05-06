using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace lol.dataloader.LolResources
{
    public class ChampLoader
    {
        private static readonly string _champFileDir = @"G:\Personal_Projects\lol_data\11.15.1\data\en_US\champion.json";

        public static async Task LoadChamps()
        {
            var champJson = await File.ReadAllTextAsync(_champFileDir);

            var champs = JsonConvert.DeserializeObject<RiotChampionDto>(champJson);
            var champEntities = new List<lol.dataloader.Champion>();
            using var context = new MovietimeContext();

            var existingChamps = await context.Champion.Select(c => c.RiotChampionId).ToListAsync();

            foreach (var champ in champs.Data.Values)
            {
                if (!existingChamps.Contains((int)champ.Key))
                {
                    var champEntity = new lol.dataloader.Champion
                    {
                        Name = champ.Name,
                        RiotChampionId = (int)champ.Key
                    };
                    champEntities.Add(champEntity);
                }
            }

            if (champEntities.Any())
            {

                context.Champion.AddRange(champEntities);
                await context.SaveChangesAsync();
            }
        }
    }
}
