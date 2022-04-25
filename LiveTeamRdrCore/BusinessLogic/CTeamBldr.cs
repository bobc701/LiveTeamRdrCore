﻿using DataAccess;
using DataAccess.Entities;
using LiveTeamRdrApi.BusinessLogic;
//using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LiveTeamRdrCore.BusinessLogic {

   public class CTeamBldr {

      private IDbxMlbHistory _dbx;

      public CTeamBldr(IDbxMlbHistory dbx) 
      {
         _dbx = dbx;
      }

      public ZTeam zteam1 { get; set; }
      public List<ZBatting> zbatting1 { get; set; }
      public List<ZGamesByPosn> zgamesByPosn1 { get; set; }
      public List<ZPitching> zpitching1 { get; set; }
      public ZLeagueStats zleagueStats1 { get; set; }
      public List<ZFielding> zfielding1 { get; set; }
      public List<ZFieldingYear> zfieldingYear1 { get; set; }


      public List<ZTeam> ConstructTeamList(int year1, int year2) {
         
         //_dbx = new DbxMlbHistory();

         List<ZTeam> res = _dbx.GetZTeamsByRange(year1, year2);
         return res;

      }


      public List<UserTeam> ConstructTeamListCust(string userName) {
         
         //_dbx = new DB_133455_mlbhistoryEntities2();

         List<UserTeam> res = _dbx.GetUserTeamsByUserName(userName);
         return res;
      
      }



      public DTO_TeamRoster ConstructTeamMlb(string teamTag, int year) {

         //_dbx = new DB_133455_mlbhistoryEntities2();

         zteam1 = _dbx.GetZTeamSingle(teamTag, year);
         if (zteam1 == null) throw new Exception($"Error: Could not find team {teamTag} for year {year}");

         zbatting1 = _dbx.Batting1_app(teamTag, year).ToZBatting();
         zpitching1 = _dbx.Pitching1_app(teamTag, year).ToZPitching();
         zgamesByPosn1 = _dbx.GamesByPosn1_app(teamTag, year).ToZGamesByPosn();
         zfielding1 = _dbx.GetZFieldingAll(); // Is this a good approach? (~900 rows!)
         zfieldingYear1 = _dbx.FieldingYear1_app(teamTag, year).ToZFieldingYear();

         DTO_TeamRoster team = ConstructTeam();
         return team;

      }


      public DTO_TeamRoster ConstructTeamCust(int userTeamID) {

         //_dbx = new DB_133455_mlbhistoryEntities2();

         zteam1 = _dbx.GetUserTeamByTeamID(userTeamID).ToZTeam();

         zbatting1 = _dbx.Batting1_cust(userTeamID).ToZBatting();
         zpitching1 = _dbx.Pitching1_cust(userTeamID).ToZPitching();
         zgamesByPosn1 = _dbx.GamesByPosn1_cust(userTeamID).ToZGamesByPosn();
         zfielding1 = _dbx.GetZFieldingAll(); // Is this good?
         zfieldingYear1 = _dbx.FieldingYear1_cust(userTeamID).ToZFieldingYear();

         DTO_TeamRoster team = ConstructTeam();
         return team;

      }


      private DTO_TeamRoster ConstructTeam() {
      
      // In Access this was called 'BuildAllTeams'


         DupeUseNames();
         DupeUseNames2();

         FillSkillStr();     // This initializes with 3's based on GamesByPosn.
         ComputeSkillStr2(); // This This overlays with 'actual' skills using CF_FieldingYear.
         ComputeSkillStr();  // This will override with subjective skill's in CF_Fielding.

         ComputeDefense(dh: false);
         ComputeLineup(dh: false);
         ComputeDefense(dh: true);
         ComputeLineup(dh: true);

         var team = new DTO_TeamRoster();
         WriteCDB(team);

         return team;

      }

      private const int MAX_BATTERS = 14;
      private const int MAX_STARTERS = 6;
      private const int MAX_CLOSERS = 2;
      private const int MAX_OTHERS = 3;
      private const int MAX_ROSTER = 25;


      private void WriteCDB(DTO_TeamRoster team) {
         
         var listB = new List<string>();
         var listP = new List<string>();
         IEnumerable<ZBatting> listb;
         IEnumerable<ZPitching> listp;

         void AddToList(char cat, string playerID) {
            
            if (!listB.Contains(playerID) && !listP.Contains(playerID)) {
               switch (cat) {
                  case 'B': listB.Add(playerID); break;
                  case 'P': listP.Add(playerID); break;
               }
            }
         }

         // --------------------------------------------
         // Build ListB and ListP with playerID's
         // --------------------------------------------

         // -- Starters (1 pitcher + batters w/ slot or slotdh (to ListP & ListB)

         listb = zbatting1.Where(bat => bat.slot != 0 || bat.slotDh != 0);
         foreach (ZBatting bat in listb) {
            if (bat.posn == 1 || bat.posnDh == 1) AddToList('P', bat.playerID);
            else AddToList('B', bat.playerID);
         }

         // -- Pitchers (to ListP)

         listp = zpitching1.OrderByDescending(pit => pit.gs);
         foreach (ZPitching pit in listp) {
            if (listP.Count >= MAX_STARTERS) break;
            AddToList('P', pit.playerID);
         }

         // -- Closers (to ListP)

         listp = zpitching1.OrderByDescending(pit => pit.sv);
         foreach (ZPitching pit in listp) {
            if (listP.Count >= MAX_STARTERS + MAX_CLOSERS) break;
            AddToList('P', pit.playerID);
         }

         // -- Other pitchers (to ListP)

         listp = zpitching1.OrderByDescending(pit => pit.ipOuts);
         foreach (ZPitching pit in listp) {
            if (listP.Count >= MAX_STARTERS + MAX_CLOSERS + MAX_OTHERS) break;
            AddToList('P', pit.playerID);
         }

         // -- More batters... (to listB)

         listb = zbatting1.OrderByDescending(bat => bat.ab);
         foreach (ZBatting bat in listb) {
            if (listB.Count + listP.Count >= MAX_ROSTER) break;
            AddToList('B', bat.playerID);
         }


         // --------------------------------------
         // Construct the CTeamInfo object
         // --------------------------------------


         team.Team = zteam1.ZTeam1;
         team.YearID = zteam1.yearID;
         team.City = zteam1.City;
         team.NickName = zteam1.NickName;
         team.LineName = zteam1.LineName;
         team.UsesDhDefault = zteam1.UsesDH;
         team.ComplPct = team.YearID switch {2020 => 37, _ => 100 }; //#2010.01 
         team.LgID = zteam1.lgID;

         //team.leagueStats = new DTO_BattingStats {
         //   pa = zleagueStats1.pa,
         //   ab = zleagueStats1.ab,
         //   h = zleagueStats1.h,
         //   b2 = zleagueStats1.b2,
         //   b3 = zleagueStats1.b3,
         //   hr = zleagueStats1.hr,
         //   so = zleagueStats1.so,
         //   sh = zleagueStats1.sh,
         //   sf = zleagueStats1.sf,
         //   bb = zleagueStats1.bb,
         //   ibb = zleagueStats1.ibb,
         //   hbp = zleagueStats1.hbp,
         //   sb = zleagueStats1.sb,
         //   cs = zleagueStats1.cs,
         //   ipOuts = zleagueStats1.ipOuts,
         //   rbi = -1
         //};

         team.PlayerInfo = new List<DTO_PlayerInfo>();
         foreach (string id in listB) {
            ZBatting bat1 = zbatting1.First(b => b.playerID == id);
            LeagueStat lg1 = _dbx.GetLeagueStatSingle(bat1.lgID, bat1.yearID);
            var player = new DTO_PlayerInfo(bat1, pit1: null, lg1);
            team.PlayerInfo.Add(player);
         }
         foreach (string id in listP) {
            ZBatting bat1 = zbatting1.First(b => b.playerID == id);
            ZPitching pit1 = zpitching1.First(p => p.playerID == id);
            LeagueStat lg1 = _dbx.GetLeagueStatSingle(bat1.lgID, bat1.yearID);
            var player = new DTO_PlayerInfo(bat1, pit1, lg1);
            team.PlayerInfo.Add(player);
         }
      }


      private void ComputeLineup(bool dh) {
      // ------------------------------------------------------
      // We have already filled posn and posnDh, and so now we
      // will assign those players with slot and slotDh, respectively.
      // ------------------------------------------------------
         var sIDList = new List<string>();

         IEnumerable<ZBatting> list;
         if (dh)
            list = zbatting1.Where(bat => bat.posnDh >= 1 && bat.posnDh <= 10);
         else
            list = zbatting1.Where(bat => bat.posn >= 1 && bat.posn <= 9);


         string GetMaxStat(string stat) {
         // ---------------------------------------------------
         // For custom teams, I remmed out the max == 0 check, so we will
         // just take 1st avail player if none avail at the posn.
         // Best we can do. User needs to add players to the team.
         // We could perhaps change TeamBldr to enforce each posn.
         // ----------------------------------------------------------
            var list1 = list.Where(bat => !sIDList.Contains(bat.playerID));
            double max = list1.Max(bat => bat[stat]);
            //if (max == 0.0) return "";
            string id1 = list1.First(bat => bat[stat] == max).playerID;
            return id1;
         }


         void PostSlot(string id, int slot) {
         
            if (dh) zbatting1.First(bat => bat.playerID == id).slotDh = slot;
            else zbatting1.First(bat => bat.playerID == id).slot = slot;
            sIDList.Add(id);
         }


      // Let's do the pitcher first...
         string id1;
         if (dh) {
            id1 = list.First(bat => bat.posnDh == 1).playerID;
            PostSlot(id1, 10); //Picher does not bat if DH rule.
         }
         else {
            id1 = list.First(bat => bat.posn == 1).playerID;
            PostSlot(id1, 9); // Pitcher bats 9th if no DH rule.
         }

         // Find player with highest slugging pct and bat him 4th...
         id1 = GetMaxStat("SLUG");
         PostSlot(id1, 4);

         // Find player with highest hitting ave and bat him 3th...
         id1 = GetMaxStat("HAve");
         PostSlot(id1, 3);

         // Find player with highest net run ave and bat him 1st...
         id1 = GetMaxStat("NRAve");
         PostSlot(id1, 1);

         // Find player with next highest net run ave and bat him 2th...
         id1 = GetMaxStat("NRAve");
         PostSlot(id1, 2);

         // Find player with next highest slugging pct and bat him 5th...
         id1 = GetMaxStat("SLUG");
         PostSlot(id1, 5);

         // Find player with next highest hitting ave and bat him 6th...
         id1 = GetMaxStat("HAve");
         PostSlot(id1, 6);

         // Find player with next highest hitting ave and bat him 7th...
         id1 = GetMaxStat("HAve");
         PostSlot(id1, 7);

         // Find player with next highest hitting ave and bat him 8th...
         id1 = GetMaxStat("HAve");
         PostSlot(id1, 8);

         // if DH rules, then find player with next highest hitting ave
         // and bat him 9th...
         if (dh) {
            id1 = GetMaxStat("HAve");
            PostSlot(id1, 9);
         }

      }


      private void ComputeDefense(bool dh) {
      // -------------------------------------------------
         var sIdList = new List<string>();
         string id = "";
         bool missingDH = false;

         string GetMaxGbp(string posn) {
            // ---------------------------------------------------
            var list = zgamesByPosn1.Where(g => !sIdList.Contains(g.playerID));
            int max = list.Max(g1 => g1[posn]);
            //if (max == 0) return "";
            string id1 = list.First(g => g[posn] == max).playerID;
            return id1;

         }

         string GetMaxGS() {
            // ---------------------------------------------------
            var list = zpitching1.Where(pit => !sIdList.Contains(pit.playerID));
            int max = list.Max(pit1 => pit1.gs);
            //if (max == 0) return "";
            string id1 = list.First(pit => pit.gs == max).playerID;
            return id1;
         }


         void PostPosn(string id1, int pos) {
            // -------------------------------------------------
            ZBatting bat = zbatting1.First(b => b.playerID == id1);
            if (dh)
               bat.posnDh = pos;
            else
               bat.posn = pos;
            sIdList.Add(id1);

         }

         // Before we start, reset posn fields to 0...
         foreach (ZBatting bat in zbatting1) {
            if (dh) bat.posnDh = 0;
            else bat.posn = 0;
         }

         // First assign the DH...
         if (dh) {
            id = GetMaxGbp("dh");
            missingDH = id == ""; // See below, we'll do this last.
            if (id != "") PostPosn(id, 10);  //posn = 10 mean non-fielder (DH)
         }

         // Get the starting pitcher, as one with most starts...
         //id = GetMax("gs");
         //if (id == "") id = GetMax("p");
         //PostPosn(id, 1);

         // GS is not in Lahman's Appearances before 1973, so use Pitching...
         id = GetMaxGS();
         PostPosn(id, 1);

         id = GetMaxGbp("ss");
         PostPosn(id, 6);

         id = GetMaxGbp("b2");
         PostPosn(id, 4);

         id = GetMaxGbp("b3");
         PostPosn (id, 5);

         id = GetMaxGbp("b1");
         PostPosn (id, 3);

         id = GetMaxGbp("c");
         PostPosn(id, 2);

         // Outfield...
         // Unfortunately, Lahman's Fielding table does not
         // split out RF, CF,  LF except in last couple of years.
         // Before that it is all "OF"

         id = GetMaxGbp("cf");
         if (id == "") id = GetMaxGbp("OF");
         PostPosn(id, 8);

         id = GetMaxGbp("rf");
         if (id == "") id = GetMaxGbp("OF");
         PostPosn(id, 9);

         id = GetMaxGbp("lf");
         if (id == "") id = GetMaxGbp("OF");
         PostPosn(id, 7);

         if (dh && missingDH) {
         // This will happen in years before dh was used.
         // We need to identify one anyway!
            id = GetMaxGbp("of"); // Use 'of' for this purpose.
            PostPosn(id, 10);
         }

      }


      private int GetSkill(int posn, double? inn, int? RtotYr) {
         // -------------------------------------------------------------
         // This is algorism to convert Rtot/Yr into fielding skill 0..6, centered
         var bracket = new int[6];
         double r;
         int res;

         // Credibility: Consider 400 inn as fully credible.
         // If < 400 inn, weight with 0.
         // So if very few inn, most likely r will be 0, which will translate to '3'.
         if (RtotYr == null || inn == null) return 3;
         if (RtotYr == 0) return 3;
         r = (double)(inn >= 400 ? RtotYr : inn / 400.0 * RtotYr);
         switch (posn) {
            case 1: bracket = new int[] { -5, -4, -3, 3, 4, 5 }; break;
            case 2: bracket = new int[] { -25, -20, -10, 10, 15, 20 }; break;
            case 3: bracket = new int[] { -10, -8, -5, 7, 10, 15 }; break;
            case 4: bracket = new int[] { -15, -12, -6, 6, 12, 20 }; break;
            case 5: bracket = new int[] { -15, -12, -6, 6, 12, 20 }; break;
            case 6: bracket = new int[] { -15, -12, -6, 6, 12, 20 }; break;
            case 7: bracket = new int[] { -25, -18, -10, 10, 15, 20 }; break;
            case 8: bracket = new int[] { -25, -18, -10, 10, 15, 20 }; break;
            case 9: bracket = new int[] { 25, -18, -10, 10, 15, 20 }; break;
         }

         res = 6;
         for (int i = 0; i <= 5; i++) {
            if (r < bracket[i]) {
               res = i;
               break;
            }
         }

      // Due to questionable reliability, lets just constrain this to
      // 2, 3, 4... (-bc 1907)
         switch (res) {
            case 0: return 2;
            case 1: return 2; 
            case 2: return 3; 
            case 3: return 3; 
            case 4: return 3; 
            case 5: return 4; 
            case 6: return 4;
            default: return 3;
         }

      }


      private void ComputeSkillStr() {
         // --------------------------------------------------------
         // TASK: Update SkillStr in zbatting1 from data in ZFieldings,
         // which is 1 rec for each player, with a field for each posn, p1, ... ,p9.
         // Data is manually (and subjectively) maintained in PackageWriter.mdb
         // and must be imported into SQL.
         // --------------------------------------------------------
         StringBuilder s = null;

         foreach (ZBatting bat in zbatting1) {
            var fld = zfielding1.FirstOrDefault(f => f.playerID == bat.playerID);
            if (fld == null) continue;
            s = new StringBuilder(bat.SkillStr);
            if (fld.p1 != null) s[0] = fld.p1.ToString()[0];
            if (fld.p2 != null) s[1] = fld.p2.ToString()[0];
            if (fld.p3 != null) s[2] = fld.p3.ToString()[0];
            if (fld.p4 != null) s[3] = fld.p4.ToString()[0];
            if (fld.p5 != null) s[4] = fld.p5.ToString()[0];
            if (fld.p6 != null) s[5] = fld.p6.ToString()[0];
            if (fld.p7 != null) s[6] = fld.p7.ToString()[0];
            if (fld.p8 != null) s[7] = fld.p8.ToString()[0];
            if (fld.p9 != null) s[8] = fld.p9.ToString()[0];

            bat.SkillStr = s.ToString();
         }


      }


      private void ComputeSkillStr2() {
      // ------------------------------------------------------
      // TASK: Update SkillStr in zbatting1 from data in ZFieldingYears,
      // which is 1 rec for each player, team, year, and pos.
      // This table is maintained outside of Lahman, currently have
      // used BBRef fielding text files from (I think) 2015 to 2019.
      // ------------------------------------------------------
         StringBuilder s;
         foreach (ZBatting bat in zbatting1) {
            s = new StringBuilder(bat.SkillStr);
            var fld = zfieldingYear1.Where(f =>
               f.playerID == bat.playerID && f.ZTeam == bat.ZTeam && f.year == bat.yearID);
            foreach (ZFieldingYear f in fld) {
               f.Skill = GetSkill(f.Posn, f.inn, f.RtotYr);
               s[f.Posn - 1] = f.Skill.ToString()[0];
            }
            bat.SkillStr = s.ToString();

         }
      }



      private void FillSkillStr() {
         // ------------------------------------------------------
         // TASK:
         // Build 's' using GBP, then post it to ZBatter, matching 
         // on playerID.
         // ------------------------------------------------------

         StringBuilder s;
         foreach (var bat in zbatting1) {

            var gbp = zgamesByPosn1.FirstOrDefault(g => g.playerID == bat.playerID);
            if (gbp == null)
               bat.SkillStr = "---------";
            else {
               s = new StringBuilder("---------");
               if (gbp.p > 0 || bat.PlayerCategory == "P") s[0] = '3';
               if (gbp.c > 0) s[1] = '3';
               if (gbp.b1 > 0) s[2] = '3';
               if (gbp.b2 > 0) s[3] = '3';
               if (gbp.b3 > 0) s[4] = '3';
               if (gbp.ss > 0) s[5] = '3';
               if (gbp.lf > 0) s[6] = '3';
               if (gbp.cf > 0) s[7] = '3';
               if (gbp.rf > 0) s[8] = '3';

               // In absence of specific OF position, we'll use the
               // OF position and apply it to all 3...
               if (gbp.lf == 0 && gbp.cf == 0 && gbp.rf == 0 && gbp.of != 0) {
                  s[6] = '3';
                  s[7] = '3';
                  s[8] = '3';
               }
               bat.SkillStr = s.ToString();
            }
         }

      }


            private void DupeUseNames() {
      // ------------------------------------------
      // TASK: Eliminate dupes in UseName.

      // Go through this 3 times...
         for (int i = 1; i <= 3; i++) {

            var x =
               (from bat in zbatting1
                group bat by bat.UseName into grouping
                where grouping.Count() >= 2
                select grouping);

            if (x.Count() == 0) break;
            foreach (var g in x) {
               zbatting1
                  .Where(bat => bat.UseName == g.Key).ToList()
                  .ForEach(bat => bat.UseName = bat.nameFirst.Substring(0, i) + "." + bat.nameLast);
            }

         }

      }


      private void DupeUseNames2() {
         // ------------------------------------------
         // TASK: Eliminate dupes in UseName2, which is already initialized as A.Judge...

         // Go through this 2 times...
         for (int i = 2; i <= 3; i++) {

            var x =
               (from bat in zbatting1
                group bat by bat.UseName2 into grouping
                where grouping.Count() >= 2
                select grouping);

            if (x.Count() == 0) break;
            foreach (var g in x) {
               zbatting1
                  .Where(bat => bat.UseName2 == g.Key).ToList()
                  .ForEach(bat => bat.UseName2 = bat.nameFirst.Substring(0, i) + "." + bat.nameLast);
            }

         }

      }

   }

}