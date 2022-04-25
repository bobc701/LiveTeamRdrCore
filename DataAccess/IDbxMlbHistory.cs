using DataAccess.Entities;
using System.Collections.Generic;

namespace DataAccess
{
   public interface IDbxMlbHistory
   {
      List<Batting1_app_Result> Batting1_app(string team, int? year);
      List<Batting1_cust_Result> Batting1_cust(int? userTeamID);
      List<FieldingYear1_app_Result> FieldingYear1_app(string team, int? year);
      List<FieldingYear1_cust_Result> FieldingYear1_cust(int? userTeamID);
      List<GamesByPosn1_app_Result> GamesByPosn1_app(string team, int? bldYear);
      List<GamesByPosn1_cust_Result> GamesByPosn1_cust(int? userTeamID);
      List<LeagueStat> GetLeagueStatsAll();
      LeagueStat GetLeagueStatSingle(string lg, int yr);
      UserTeam GetUserTeamByTeamID(int teamID);
      List<UserTeam> GetUserTeamsByUserName(string user);
      ZFielding GetZFieldingSingle(string playerID);
      List<ZFielding> GetZFieldingAll();
      List<ZTeam> GetZTeamsByRange(int yr1, int yr2);
      ZTeam GetZTeamSingle(string tm, int yr);
      List<Pitching1_app_Result> Pitching1_app(string team, int? year);
      List<Pitching1_cust_Result> Pitching1_cust(int? userTeamID);
   }
}