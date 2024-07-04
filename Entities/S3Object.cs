namespace EFTraining.Entities
{
    public class S3Object
    {
        public string Name{ get; set; } = null!;
        public MemoryStream File { get; set; } = null!;
    }
}
