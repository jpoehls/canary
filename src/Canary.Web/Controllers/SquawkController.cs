using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Canary.Web.Models;
using Dapper;

namespace Canary.Web.Controllers
{
    public class SquawkController : CanaryController
    {
        [HttpPost]
        public ActionResult Index(SquawkEventModel model)
        {
            model.Timestamp = DateTime.UtcNow;
            model.User = string.Empty;
            model.Details = Request.Form["Details"];

            using (var conn = CreateDbConnection())
            {
                conn.Open();
                using (var tx = conn.BeginTransaction())
                {
                    conn.Execute(
                        @"
declare @AppId int

select @AppId = [Id]
from dbo.[Application]
where [Name] = @App
  and [Environment] = @Env

if @AppId is null
begin
    insert into dbo.[Application]
               ([Name], [Environment])
        values (@App, @Env)
    
    set @AppId = scope_identity()
end

declare @EventId int

select @EventId = [Id]
from dbo.[Event]
where [Hash] = @Hash

if @EventId is null
begin
    insert into dbo.[Event]
               ([AppId], [Level], [Type], [Message], [Source], [Hash], [FirstTimestamp], [LastTimestamp], [TotalCount])
        values (@AppId,  @Level,  @Type,  @Message,  @Source,  @Hash,  @Timestamp,       @Timestamp,      1)
    
    set @EventId = scope_identity()
end
else
begin
    update dbo.[Event] set
        [LastTimestamp] = @Timestamp,
        [TotalCount] = TotalCount + 1
    where [Hash] = @Hash
end

insert into dbo.[EventInstance]
           ([EventId], [User], [Details], [Timestamp])
    values (@EventId,  @User,  @Details,  @Timestamp)
",
                        new
                            {
                                model.App,
                                model.Env,
                                model.Level,
                                model.Type,
                                model.Message,
                                model.Source,
                                Hash = model.ComputeHash(),
                                model.Timestamp,
                                model.User,
                                model.Details
                            },
                        tx);
                    tx.Commit();
                }
            }

            return new EmptyResult();
        }

        [HttpGet]
        public JavaScriptResult ClientScript()
        {
            var r = new JavaScriptResult();
            r.Script = System.IO.File.ReadAllText(Server.MapPath("~/scripts/canary.js"));
            return r;
        }
    }
}