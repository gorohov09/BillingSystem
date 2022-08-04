using BillingSystem.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace BillingSystem.Domain.Entities;

[Table("Coin")]
public class CoinDomain : Entity
{
    public string History { get; set; }

    public UserProfileDomain? UserProfile { get; set; }
}
