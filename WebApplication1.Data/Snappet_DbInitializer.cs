using System.Data.Entity;
using System.IO;
using WebApplication1.Data.Contexts;
using Newtonsoft.Json;

namespace WebApplication1.Data
{
    class Snappet_DbInitializer : DropCreateDatabaseIfModelChanges<SnappetContext>
    {
        protected override void Seed(SnappetContext context)
        {
            var newContext = ConvertJsonToDbContext();

            base.Seed(newContext);
        }

        private SnappetContext ConvertJsonToDbContext()
        {
            //todo: get resource from another location. I want to use an Embedded Resource now because it gets the job done

            var resourceAsByteArray = Properties.Resources.work;

            //geen moeilijke usings, we weten waar de file staat en wat de content is dus no nonsense here.
            //var jsonString = System.Text.Encoding.UTF8.GetString(resourceAsByteArray);


            using (MemoryStream stream = new MemoryStream(resourceAsByteArray))
            using (StreamReader reader = new StreamReader(stream))
            {
                JsonTextReader jsonTextReader = new JsonTextReader(reader);
                jsonTextReader.Read();
                //JsonConverter convert = new JsonConverter(jsonTextReader);
            }

            return null;
        }
    }
}
