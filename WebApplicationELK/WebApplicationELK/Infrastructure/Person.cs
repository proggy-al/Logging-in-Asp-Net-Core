using Destructurama.Attributed;

namespace WebApplicationELK.Infrastructure
{
    public class Person
    {
        [LogWithName("[Personal ID]")]
        public int Id { get; set; }

        [LogMasked(Text = "_REMOVED_", ShowFirst = 3)]
        public string Name { get; set; }

        [LogMasked]
        public string Document { get; set; }

        [NotLogged]
        public string TopSecretInformation { get; set; }
    }
}
