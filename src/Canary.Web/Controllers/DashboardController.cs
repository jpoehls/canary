using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web.Mvc;
using Canary.Web.Models;
using Dapper;

namespace Canary.Web.Controllers
{
    public class DashboardController : CanaryController
    {
        //
        // GET: /{app}/{env}

        [HttpGet]
        public ActionResult Index(string app, string env)
        {
            var model = new AppDashboardModel();

            using (var conn = CreateDbConnection())
            {
                conn.Open();

                dynamic appDetails =
                    conn.Query(
                        @"
select [Id], [Name], [Environment]
from dbo.[Application]
where [Name] = @app and [Environment] = @env;
",
                        new { app, env }).SingleOrDefault();

                if (appDetails == null)
                {
                    model.AppName = app;
                    model.EnvName = env;
                    model.Events = Enumerable.Empty<EventSummaryModel>();
                }
                else
                {
                    model.AppName = appDetails.Name;
                    model.EnvName = appDetails.Environment;

                    int id = appDetails.Id;
                    model.Events =
                        conn.Query<EventSummaryModel>(
                            @"
select [Hash],
       [Level],
       [Type],
       [Message],
       [FirstTimestamp]
       [LastTimestamp],
       [TotalCount] as [LifetimeTotal],
       (select count([Id]) from dbo.[EventInstance] where [EventId] = e.[Id]
        and datediff(hour, [Timestamp], getutcdate()) <= 168 ) as [WeekTotal]
from dbo.[Event] e
where [AppId] = @id
order by 7 desc, 6 desc;
",
                            new { id });
                }
            }


            return View(model);
        }

        //
        // GET: /dashboard/list
        //      /dashboard/

        [HttpGet]
        public ActionResult List()
        {
            return View();
        }
    }
}