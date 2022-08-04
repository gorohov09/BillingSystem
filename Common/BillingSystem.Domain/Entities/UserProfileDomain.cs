using BillingSystem.Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BillingSystem.Domain.Entities;

[Table("UserProfile")]
[Index("Name", IsUnique = true, Name = "Phone_Index")]
public class UserProfileDomain : Entity
{
    [Required]
    public string Name { get; set; }

    public long? Amount { get; set; }

    public int Rating { get; set; }

    public ICollection<CoinDomain> Coins { get; set; } = new List<CoinDomain>();
}
