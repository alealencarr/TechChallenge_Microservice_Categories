namespace Domain.Entities
{
    public class Categorie
    {
        public Categorie(string name, Guid id, DateTime createdAt, bool isEditavel)
        {
            Name = name;
            Id = id;
            CreatedAt = createdAt;
            IsEditavel = isEditavel;
        }
        public Categorie(string name, bool isEditavel)
        {
            Name = name;
            Id = Guid.NewGuid();
            CreatedAt = DateTime.Now;
            IsEditavel = isEditavel;
        }
        public Categorie() { }

        public DateTime CreatedAt { get; private set; }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool IsEditavel { get; set; }

        public bool IsLanche()
        {
            return Name.Equals("Lanche", StringComparison.OrdinalIgnoreCase);
        }
    }
}
