using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace VueApp1.Server.Models.Entities;

public class QueryParameters
{

  [FromQuery(Name = "page"), Required]
  public int Page { get; set; }

  [FromQuery(Name = "limit"), Required]
  public int Limit { get; set; }
}