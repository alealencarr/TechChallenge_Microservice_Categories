using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Infrastructure.DbModels
{
    public class CategorieDbModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }

        [BsonElement("name")]
        [BsonRequired]
        public string Name { get; set; }

        [BsonElement("isEditavel")]
        public bool IsEditavel { get; set; }

        [BsonElement("createdAt")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreatedAt { get; set; }

        public CategorieDbModel()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.Now;
            IsEditavel = false;
        }

        public CategorieDbModel(Guid id, string name, bool isEditavel, DateTime? createdAt = null)
        {
            Id = id;
            Name = name;
            IsEditavel = isEditavel;
            CreatedAt = createdAt ?? DateTime.Now;
        }
    }
}
