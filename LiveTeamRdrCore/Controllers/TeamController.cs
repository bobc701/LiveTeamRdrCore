using DataAccess;
using DataAccess.Entities;
using LiveTeamRdrApi.BusinessLogic;
using LiveTeamRdrCore.BusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace LiveTeamRdrCore.Controllers
{
   public class TeamController : Controller
   {

      IDbxMlbHistory _dbx;
      CTeamBldr _bldr;
      ILogger<TeamController> _logger;

      public TeamController(IDbxMlbHistory dbx, CTeamBldr bldr, ILogger<TeamController> logger) 
      {
         _dbx = dbx;
         _bldr = bldr;
         _logger = logger;
      }

      // GET: api/Team/{teamTag}/{year:int}
      [Route("api/team/{teamTag}/{year:int}")]
      [HttpGet]
      public DTO_TeamRoster GetTeam(string teamTag, int year) {

         try {
            LogRequest("GetTeanm");
            //var bldr = new CTeamBldr();
            DTO_TeamRoster team1 = _bldr.ConstructTeamMlb(teamTag, year);
            return team1;
         }
         catch (Exception ex) {
            string msg = $"Unable to load team data for {teamTag} for year {year}\r\n{ex.Message}";
            var response = new HttpResponseMessage(HttpStatusCode.NotFound) {
               Content = new StringContent(msg, System.Text.Encoding.UTF8, "text/plain"),
               StatusCode = HttpStatusCode.NotFound
            };
            //Debug.WriteLine(response.StatusCode);
            throw new HttpResponseException(response);
         }

      }


      // GET:
      [Route("api/team-cust/{teamID:int}")]
      [HttpGet]
      public DTO_TeamRoster GetTeamCust(int teamID) {

         try {
            LogRequest("GetTeamCust");
            //var bldr = new CTeamBldr();
            DTO_TeamRoster team1 = _bldr.ConstructTeamCust(teamID);
            return team1;
         }
         catch (Exception ex) {
            string msg = $"Unable to load team data for {teamID}\r\n{ex.Message}";
            var response = new HttpResponseMessage(HttpStatusCode.NotFound) {
               Content = new StringContent(msg, System.Text.Encoding.UTF8, "text/plain"),
               StatusCode = HttpStatusCode.NotFound
            };
            throw new HttpResponseException(response);
         }

      }


      // GET: api/Team/{teamTag}/{year:int}
      [Route("api/team-list/{year1:int}/{year2:int}")]
      [HttpGet]
      public List<CTeamRecord> GetTeamList(int year1, int year2) {

         try {
            LogRequest("GetTeamList");
            //var bldr = new CTeamBldr();
            List<CTeamRecord> result = _bldr.ConstructTeamList(year1, year2)
               .Select(t => new CTeamRecord {
                  City = t.City,
                  LineName = t.LineName,
                  LgID = t.lgID,
                  NickName = t.NickName,
                  TeamTag = t.ZTeam1, //For real teams
                  Year = t.yearID,    //For real teams
                  UserTeamID = 0,     //For custom teams
                  UsesDh = t.UsesDH
               })
            .OrderByDescending(t => t.LgID).ThenBy(t => t.City).ToList();
            return result;
         }
         catch (Exception ex) {
            string msg = $"Unable to retrieve list of teams for years {year1} to {year2}\r\n{ex.Message}";
            var response = new HttpResponseMessage(HttpStatusCode.NotFound) {
               Content = new StringContent(msg, System.Text.Encoding.UTF8, "text/plain"),
               StatusCode = HttpStatusCode.NotFound
            };
            throw new HttpResponseException(response);
         }

      }


      [Route("api/team-list-cust/{userName}")]
      [HttpGet]
      public List<CTeamRecord> GetTeamListCust(string userName) {

         try {
            LogRequest("GetTeamListCust");
            //var bldr = new CTeamBldr();
            List<CTeamRecord> result = _bldr.ConstructTeamListCust(userName)
               .Select(t => new CTeamRecord {
                  City = t.TeamName,
                  LineName = t.TeamName.Substring(0, 3),
                  LgID = "NA",
                  NickName = t.TeamName,
                  TeamTag = "CUS",  //For real teams
                  Year = 0,         //For real teams
                  UserTeamID = t.UserTeamID, //For custom teams
                  UsesDh = t.UsesDh
               })
               .OrderBy(t => t.City).ToList();

            return result;
         }

         catch (Exception ex) {
            string msg = $"Unable to retrieve list of teams for user {userName}";
            var response = new HttpResponseMessage(HttpStatusCode.NotFound) {
               Content = new StringContent(msg, System.Text.Encoding.UTF8, "text/plain"),
               StatusCode = HttpStatusCode.NotFound
            };
            throw new HttpResponseException(response);
         }

      }


      [Route("api/fullteam/{teamTag}/{year:int}")]
      [HttpGet]
      public List<Batting1_app_Result> GetFullTeam(string teamTag, int year) {

         LogRequest("GetFullTeam");
         //var ctx = new DataAccess.DB_133455_mlbhistoryEntities2();
         List<Batting1_app_Result> list1;
         list1 = _dbx.Batting1_app(teamTag, year);

         return list1;
      }

      // ---------------------------------------------------
      // The following action 'GetTest', returns a complex data type,
      // CTeamInfo (actual class to be used in bcxb3, with
      // nested subtypes and a list of nested subtypes, and it can be tested
      // with 'HttpClientDemo' project, using HttpClient's GetAsync
      // method.
      // The object is hard coded, but the downstream process doesn't know that!
      // ---------------------------------------------------

      [Route("api/test/{teamTag}/{year:int}")]
      [HttpGet]
      public DTO_TeamRoster GetTest(string teamTag, int year) {


         var team = new DTO_TeamRoster {
            Team = teamTag + year.ToString(),
            City = "New York",
            NickName = "Mets",
            LineName = "NYM",
            leagueStats = new DTO_BattingStats(),
            PlayerInfo = new List<DTO_PlayerInfo> {
               new DTO_PlayerInfo() {
                  UseName = "Seaver",
                  UseName2 = "T.Seaver",
                  SkillStr = "4--------",
                  slot = 9,
                  posn = 1,
                  slotdh = 0,
                  posnDh = 9,
                  battingStats = new DTO_BattingStats(),
                  Playercategory = 'P',
                  pitchingStats = new DTO_PitchingStats()
               },
               new DTO_PlayerInfo {
                  UseName = "Jones",
                  UseName2 = "C.Jones",
                  SkillStr = "-------4-",
                  slot = 0,
                  posn = 0,
                  slotdh = 0,
                  posnDh = 0,
                  battingStats = new DTO_BattingStats(),
                  Playercategory = 'B',
                  pitchingStats = null
               }
            }
         };

         return team;
      }


      // POST: api/Team
      public void Post([FromBody] string value) {
      }


      private void LogRequest(string actionName) {

         _logger.LogInformation($"Action method: {actionName}");
         _logger.LogInformation($"Request.Query: {Request.Query}");
         //_logger.LogInformation($"Request.Form:  {Request.Form}");
      }


   }



   public struct CTeamRecord
{

   public string TeamTag { get; set; } //For real teams
   public int Year { get; set; }       //For real teams
   public int UserTeamID { get; set; } // For custom teams
   public string LineName { get; set; }
   public string City { get; set; }
   public string NickName { get; set; }
   public bool UsesDh { get; set; }
   public string LgID { get; set; }

}

}


