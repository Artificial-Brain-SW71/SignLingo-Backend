using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace SignLingo.Infrastructure.Models;

public class City : BaseModel
{
    public string City_Name { get; set; }
    [NotMapped]
    public Country country;
    
    [JsonIgnore]
    public List<User> Users { get; set; }
    public int CountryId { get; set; }
}