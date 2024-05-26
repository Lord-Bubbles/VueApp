using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace VueApp1.Server.Models;

public class Performance
{
  public int ID { get; set; }

  public string[] Goals { get; set; } = [];

  public string[] Improve { get; set; } = [];

  public string[] Well { get; set; } = [];

  public int Rating { get; set; }

  public string Type { get; set; } = "";

  public int UserID { get; set; }

  public DateTime CreatedAt { get; set; } = new();
}

public class PerformanceParameters : QueryParameters
{
  [FromQuery(Name = "user")]
  public int UserID { get; set; }

  [FromQuery(Name = "type")]
  public string? Type { get; set; }
}