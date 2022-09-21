using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DataAccess.Entities;
using Microsoft.Extensions.Configuration;

namespace DataAccess
{
   public class DbxMlbHistory : IDbxMlbHistory
   {

      private string _connStr;
      private IConfiguration _config;
      private SqlConnection con;

      public DbxMlbHistory(IConfiguration config) {

         _config = config;
         string _conStr = _config.GetConnectionString("Default");
         con = new(_conStr);
         con.Open();

      }


      public DbxMlbHistory(string conStr) {

         _connStr = conStr;
         con = new(_connStr);
         con.Open();

      }


      public LeagueStat GetLeagueStatSingle(string lg, int yr) {
         // This version of LeagueStats filters on lg & yr, returns just 1...
         string sql = "SELECT * FROM LeagueStats WHERE lgID = @lg AND yearID = @yr";
         LeagueStat item =
            con.QueryFirstOrDefault<LeagueStat>(
               sql,
               new { lg = lg, yr = yr }
            );
         return item;
      }

      public List<LeagueStat> GetLeagueStatsAll() {
         // This version of LeagueStats returns the whole table (about 220 rows)...
         string sql = "SELECT * FROM LeagueStats";
         List<LeagueStat> list =
            con.Query<LeagueStat>(sql).ToList();
         return list;
      }



      public ZFielding GetZFieldingSingle(string playerID) {
         string sql = "SELECT * FROM ZFieldings WHERE playerID = @id";
         ZFielding item =
            con.QueryFirst<ZFielding>(sql, new { id = playerID });
         return item;
      }


      public List<ZFielding> GetZFieldingAll() {
         string sql = "SELECT * FROM ZFielding";
         var list = con.Query<ZFielding>(sql);
         return list.ToList();
      }


      public ZTeam GetZTeamSingle(string tm, int yr) {
         string sql = "SELECT * FROM ZTeams WHERE teamID = @tm AND yearID = @yr";
         ZTeam item = con.QuerySingle<ZTeam>(sql, new { tm = tm, yr = yr });
         return item;
      }

      public List<ZTeam> GetZTeamsByRange(int yr1, int yr2) {
         string sql = "SELECT * FROM ZTeams WHERE yearID >= @yr1 AND yearID <= @yr2";
         var list =
            con.Query<ZTeam>(sql, new { yr1 = yr1, yr2 = yr2 });
         return list.ToList();
      }


      public UserTeam GetUserTeamByTeamID(int teamID) {
         string sql = "SELECT * FROM UserTeams WHERE UserTeamID = @teamID";
         UserTeam item = con.QuerySingle<UserTeam>(sql, new { teamID = teamID });
         return item;
      }


      public List<UserTeam> GetUserTeamsByUserName(string user) {
         string sql = "SELECT * FROM UserTeams WHERE UserName = @user AND IsComplete = 1";
         var list = con.Query<UserTeam>(sql, new { user = user });
         return list.ToList();
      }


      public List<Batting1_app_Result> Batting1_app(string team, Nullable<int> year) {
         string sp = "Batting1_app_A";
         var list =
            con.Query<Batting1_app_Result>(
               sp,
               new { team = team, year = year },
               commandType: System.Data.CommandType.StoredProcedure
            );
         return list.ToList();
      }


      public List<FieldingYear1_app_Result> FieldingYear1_app(string team, Nullable<int> year) {
         string sp = "FieldingYear1_app";
         var list =
            con.Query<FieldingYear1_app_Result>(
               sp,
               new { team = team, year = year },
               commandType: System.Data.CommandType.StoredProcedure
            );
         return list.ToList();
      }


      public List<GamesByPosn1_app_Result> GamesByPosn1_app(string team, Nullable<int> bldYear) {
         string sp = "GamesByPosn1_app";
         var list =
            con.Query<GamesByPosn1_app_Result>(
               sp,
               new { team = team, BldYear = bldYear },
               commandType: System.Data.CommandType.StoredProcedure
            );
         return list.ToList();
      }


      public List<Pitching1_app_Result> Pitching1_app(string team, Nullable<int> year) {
         string sp = "Pitching1_app";
         var list =
            con.Query<Pitching1_app_Result>(
               sp,
               new { team = team, year = year },
               commandType: System.Data.CommandType.StoredProcedure
            );
         return list.ToList();
      }


      public List<Batting1_cust_Result> Batting1_cust(Nullable<int> userTeamID) {
         string sp = "Batting1_cust_A"; // "_A" is for 
         var list =
            con.Query<Batting1_cust_Result>(
               sp,
               new { userTeamID = userTeamID },
               commandType: System.Data.CommandType.StoredProcedure
            );
         return list.ToList();
      }


      public List<FieldingYear1_cust_Result> FieldingYear1_cust(Nullable<int> userTeamID) {
         string sp = "FieldingYear1_cust";
         var list =
            con.Query<FieldingYear1_cust_Result>(
               sp,
               new { userTeamID = userTeamID },
               commandType: System.Data.CommandType.StoredProcedure
            );
         return list.ToList();
      }


      public List<GamesByPosn1_cust_Result> GamesByPosn1_cust(Nullable<int> userTeamID) {
         string sp = "GamesByPosn1_cust";
         var list =
            con.Query<GamesByPosn1_cust_Result>(
               sp,
               new { userTeamID = userTeamID },
               commandType: System.Data.CommandType.StoredProcedure
            );
         return list.ToList();
      }

      public List<Pitching1_cust_Result> Pitching1_cust(Nullable<int> userTeamID) {
         string sp = "Pitching1_cust";
         var list =
            con.Query<Pitching1_cust_Result>(
               sp,
               new { userTeamID = userTeamID },
               commandType: System.Data.CommandType.StoredProcedure
            );
         return list.ToList();
      }

   }

}
