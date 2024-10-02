namespace Domain.Rents;

public class Plan
{
    public int Id { get; set; }
    public string? Description { get; set; }
    public int Days { get; set; }
    public decimal Fine { get; set; }
    public decimal PricePerDay { get; set; }
    public decimal PricePerAditionalDay { get; set; }

    public Plan()
    {
    }

    private Plan(int id, string? description, int days, decimal fine, decimal pricePerDay, decimal pricePerAditionalDay)
    {
        Id = id;
        Description = description;
        Days = days;
        Fine = fine;
        PricePerDay = pricePerDay;
        PricePerAditionalDay = pricePerAditionalDay;
    }

    public Plan? GetPlanById(int? id)
    {
        var plans = new List<Plan>
        {
            new(id: 7, description: "Plano 7 dias", days: 7, fine: 20.00m, pricePerDay: 30.00m, pricePerAditionalDay: 50.0m),
            new(id: 15, description: "Plano 15 dias", days: 15, fine: 40.00m, pricePerDay: 28.00m, pricePerAditionalDay: 50.0m),
            new(id: 30, description: "Plano 30 dias", days: 30, fine: 0.00m, pricePerDay: 22.00m, pricePerAditionalDay: 50.0m),
            new(id: 45, description: "Plano 45 dias", days: 45, fine: 0.00m, pricePerDay: 20.00m, pricePerAditionalDay: 50.0m),
            new(id: 60, description: "Plano 60 dias", days: 60, fine: 0.00m, pricePerDay: 18.00m, pricePerAditionalDay: 50.0m)
        };

        return plans.SingleOrDefault(x => x.Id == id);
    }
}