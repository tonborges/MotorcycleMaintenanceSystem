using Domain.DeliveryPeople;
using Domain.Motorcycles;
using Shared.Domain;

namespace Domain.Rents;

public class Rent : Entity
{
    public Guid Id { get; private set; }
    public string? DeliveryPersonId { get; private set; }
    public virtual DeliveryPerson? DeliveryPerson { get; private set; }
    public string? MotorcycleId { get; private set; }
    public virtual Motorcycle? Motorcycle { get; private set; }
    public DateTime? StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public DateTime? ForecastDateEnd { get; private set; }
    public DateTime? ReturnDate { get; private set; }
    public int? Plan { get; private set; }
    public decimal? DailyPrice { get; private set; }
    public decimal? FineValue { get; private set; }
    public decimal? TotalValue { get; private set; }

    public Rent(Guid id)
    {
        Id = id;
    }

    public Rent()
        : this(Guid.NewGuid())
    {
    }

    public Rent SetDeliveryPersonId(string? deliveryPersonId)
    {
        DeliveryPersonId = deliveryPersonId;
        return this;
    }

    public Rent SetMotorcycleId(string? motorcycleId)
    {
        MotorcycleId = motorcycleId;
        return this;
    }

    public Rent SetStartDate(DateTime? startDate)
    {
        StartDate = startDate;
        return this;
    }

    public Rent SetEndDate(DateTime? endDate)
    {
        EndDate = endDate;
        return this;
    }

    public Rent SetForecastDateEnd(DateTime? forecastDateEnd)
    {
        ForecastDateEnd = forecastDateEnd;
        return this;
    }

    public Rent SetPlan(int? plan)
    {
        Plan = plan;
        return this;
    }

    public Rent SetDailyPrice(decimal? dailyPrice)
    {
        DailyPrice = dailyPrice;
        return this;
    }

    public void FinalizeRental(DateTime? returnDate)
    {
        ReturnDate = returnDate;
        var differenceInDays = (ReturnDate - EndDate)?.Days ?? 0;
        var plan = new Plan().GetPlanById(Plan);

        switch (differenceInDays)
        {
            case < 0:
            {
                var realDaysRent = plan?.Days + differenceInDays;
                var totalPrice = plan?.PricePerDay * realDaysRent;
                var fine = (plan?.PricePerDay * (differenceInDays * -1) * plan?.Fine) / 100m;
                DailyPrice = plan?.PricePerDay;
                FineValue = fine;
                TotalValue = totalPrice + FineValue;
                break;
            }
            case > 0:
                DailyPrice = plan?.PricePerDay;
                FineValue = (plan?.PricePerAditionalDay * differenceInDays);
                TotalValue = (DailyPrice * plan?.Days + FineValue);
                break;
            default:
                DailyPrice = plan?.PricePerDay;
                FineValue = 0;
                TotalValue = (DailyPrice * plan?.Days);
                break;
        }
    }
}