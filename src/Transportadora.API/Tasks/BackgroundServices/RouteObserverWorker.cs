
//using Microsoft.EntityFrameworkCore;
//using Transportadora.API.Data;
//using Transportadora.API.DTOs.Models;
//using Transportadora.API.Shared.Enums;

//namespace Transportadora.API.Tasks.BackgroundServices;

//public sealed class RouteObserverWorker(IConfiguration configuration, ApplicationDbContext _db) : PeriodicBackgroundService(TimeSpan.FromMinutes(1))
//{
//    private readonly IConfiguration _configuration = configuration;

//    private readonly ApplicationDbContext db = _db;

//    protected override async Task IterateAsync(CancellationToken cancelationToken = default)
//    {
//        try
//        {
//            ushort minutesAgo = 3;

//            var routes = await db.Routes
//                .Where(r => r.Status == RouteStatus.InProgress
//                         && r.StartedAt > DateTimeOffset.UtcNow.AddMinutes(-minutesAgo)
//                         && r.Stops.OrderByDescending(s => s.CreatedAt)
//                                   .Select(s => s.DepartedAt)
//                                   .FirstOrDefault() != null)
//                .OrderByDescending(p => p.CreatedAt)
//                .Select(p => new {
//                    p.Id,
//                })
//                .ToListAsync(cancelationToken);

//            foreach (var route in routes)
//            {
//                var lastPosition = await db.VehiclePositions
//                .AsNoTracking()
//                .Where(p => p.RouteId == route.Id)
//                .OrderByDescending(p => p.CapturedAt)
//                .Select(p => new RouteModel()
//                {
//                    Latitude = p.Latitude,
//                    Longitude = p.Longitude,
//                    Speed = p.Speed,
//                    Heading = p.Heading,
//                    CapturedAt = p.CapturedAt
//                })
//                .FirstOrDefaultAsync(cancelationToken);

//                if (lastPosition is null) {
//                    continue;
//                }

//                var targetTime = lastPosition.CapturedAt.AddMinutes(-minutesAgo);

//                var positionBefore = await db.VehiclePositions
//                    .AsNoTracking()
//                    .Where(p => p.RouteId == route.Id && p.CapturedAt <= targetTime)
//                    .OrderByDescending(p => p.CapturedAt)
//                    .Select(p => new RouteModel()
//                    {
//                        Latitude = p.Latitude,
//                        Longitude = p.Longitude,
//                        Speed = p.Speed,
//                        Heading = p.Heading,
//                        CapturedAt = p.CapturedAt
//                    })
//                    .FirstOrDefaultAsync(cancelationToken);

//                if (positionBefore is null) {
//                    continue;
//                }
//            }

//            //return Ok(new GetRouteFromTimeAgoModel()
//            //{
//            //    RouteBefore = positionBefore,
//            //    RouteNow = lastPosition
//            //});
//        }
//        catch (Exception ex)
//        {
//            return;
//        }
//    }
//}
