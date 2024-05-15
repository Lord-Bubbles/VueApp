using Microsoft.AspNetCore.Mvc;

namespace VueApp1.Server.Models;

public class QueryParameters
{

    [FromQuery(Name = "page")]
    public int Page { get; set; }

    [FromQuery(Name = "limit")]
    public int Limit { get; set; }
}