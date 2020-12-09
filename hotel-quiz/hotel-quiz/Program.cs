using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

var factory = new HotelContextFactory();
var context = factory.CreateDbContext();

enum Special
{
    Spa,
    Sauna,
    DogFriendly,
    IndoorPool,
    OutdoorPool,
    BikeRental,
    ECarChargingStation,
    VegetarianCuisine,
    OrganicFood
}

class Hotel
{
    public int Id { get; set; }

    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(200)]
    public string Address { get; set; } = string.Empty;

    public List<HotelSpecial> Specials { get; set; } = new();

    public List<RoomType> RoomTypes { get; set; } = new();
}

class HotelSpecial
{
    public int Id { get; set; }

    public Special Special { get; set; }

    public Hotel? Hotels { get; set; }
}

class RoomType
{
    public int Id { get; set; }

    public Hotel? Hotel { get; set; }

    public Price? Price { get; set; }

    public int HotelId { get; set; }

    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Size { get; set; } = string.Empty;

    public bool DisabilityAccessible { get; set; }

    public uint RoomsAvailable { get; set; }
}

class Price
{
    public int Id { get; set; }

    public int RoomTypeId { get; set; }

    public RoomType? RoomType { get; set; }

    public DateTime? ValidFrom { get; set; }

    public DateTime? ValidTo { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal PriceEUR { get; set; }
}

class HotelContext : DbContext
{
    public DbSet<Hotel> Hotels { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public HotelContext(DbContextOptions<HotelContext> options) : base(options) { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}

class HotelContextFactory : IDesignTimeDbContextFactory<HotelContext>
{
    public HotelContext CreateDbContext(string[]? args = null)
    {
        var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        var optionsBuilder = new DbContextOptionsBuilder<HotelContext>();
        optionsBuilder
            .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
            .UseSqlServer(configuration["ConnectionStrings:DefaultConnection"]);

        return new HotelContext(optionsBuilder.Options);
    }
}