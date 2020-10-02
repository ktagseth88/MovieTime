using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace lol.dataloader.LolResources
{
    public class ChampLoader
    {
        private static readonly string _champFileDir = @"G:\Personal_Projects\lol_data\10.19.1\data\en_US\champion.json";

        public static async Task LoadChamps()
        {
            var champJson = await File.ReadAllTextAsync(_champFileDir);

            var champs = JsonConvert.DeserializeObject<RiotChampionDto>(champJson);
            var champEntities = new List<lol.dataloader.Champion>();

            foreach(var champ in champs.Data.Values)
            {
                var champEntity = new lol.dataloader.Champion
                {
                    Name = champ.Name,
                    RiotChampionId = (int)champ.Key
                };
                champEntities.Add(champEntity);
            }

            using var context = new MovietimeContext();
            context.Champion.AddRange(champEntities);
            await context.SaveChangesAsync();
        }
    }
}
