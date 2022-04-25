﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataAccess;
//using System.Data.Entity.Core.Objects;
//using System.Web.UI.WebControls.WebParts;
using DataAccess.Entities;

namespace LiveTeamRdrApi.BusinessLogic {

   public static class Mapping_cust {


      public static ZTeam ToZTeam(this UserTeam listIn) {
      // ------------------------------------------------------------
      // This mapping is only needed for custom teams, not Mlb teams.
      // ------------------------------------------------------------
         ZTeam statsOut = new ZTeam() {
            City = listIn.TeamName,
            NickName = listIn.TeamName,
            LineName = listIn.TeamName.Substring(0, 3),
            lgID = "NA",
            UsesDH = listIn.UsesDh,
            yearID = DateTime.Now.Year,
            teamID = "CUS",
            ZTeam1 = "CUS"
         };

         return statsOut;

      }


      public static List<ZBatting> ToZBatting(this List<Batting1_cust_Result> listIn) {
         // ---------------------------------------------------------------------
         var result = new List<ZBatting>();
         ZBatting statsOut;
         foreach (var statsIn in listIn) {
            statsOut = new ZBatting() {

               playerID = statsIn.playerID,
               lgID = statsIn.lgID,
               yearID = statsIn.yearID,
               ZTeam = statsIn.ZTeam,
               nameLast = statsIn.nameLast,
               nameFirst = statsIn.nameFirst,
               UseName = statsIn.UseName,  // UseName is for play-by-play, 'Judge'
               UseName2 = statsIn.nameFirst.Substring(0, 1) + "." + statsIn.nameLast, // UseName2 is for box scores, 'A.Judge'
               bats = statsIn.bats,
               throws = statsIn.throws,
               PlayerCategory = statsIn.PlayerCategory,
               g = (int)statsIn.G,
               ab = (int)statsIn.AB,
               r = (int)statsIn.R,
               h = (int)statsIn.H,
               b2 = (int)statsIn.C2B,
               b3 = (int)statsIn.C3B,
               hr = (int)statsIn.HR,
               rbi = (int)statsIn.RBI,
               sb = (int)statsIn.SB,
               cs = (int)statsIn.CS,
               bb = (int)statsIn.BB,
               so = (int)statsIn.SO,
               ibb = (int)statsIn.IBB,
               hbp = (int)statsIn.HBP,
               sh = (int)statsIn.SH,
               sf = (int)statsIn.SF,
               gidp = statsIn.GIDP,

               slot = 0,
               posn = 0,
               slotDh = 0,
               posnDh = 0,

               SkillStr = "---------",

               p_h = 0.0,
               p_2b = 0.0,
               p_3b = 0.0,
               p_hr = 0.0,
               p_bb = 0.0,
               p_so = 0.0,
               p_sb = 0.0

            };
            result.Add(statsOut);

         }
         return result;
      }


      public static List<ZGamesByPosn> ToZGamesByPosn(this List<GamesByPosn1_cust_Result> listIn) {
         // ---------------------------------------------------------------------
         var result = new List<ZGamesByPosn>();
         ZGamesByPosn statsOut;
         foreach (var statsIn in listIn) {
            statsOut = new ZGamesByPosn() {

               playerID = statsIn.playerID,
               g = statsIn.G_all,
               gs = (int)statsIn.GS,
               p = (int)statsIn.G_p,
               c = (int)statsIn.G_c,
               b1 = (int)statsIn.G_1b,
               b2 = (int)statsIn.G_2b,
               b3 = (int)statsIn.G_3b,
               ss = (int)statsIn.G_ss,
               lf = (int)statsIn.G_lf,
               cf = (int)statsIn.G_cf,
               rf = (int)statsIn.G_rf,
               of = (int)statsIn.G_of,
               dh = (int)statsIn.G_dh,

               BBRefFielding = "",

               r1 = 0.0,
               r2 = 0.0,
               r3 = 0.0,
               r4 = 0.0,
               r5 = 0.0,
               r6 = 0.0,
               r7 = 0.0,
               r8 = 0.0,
               r9 = 0.0,

               SkillStr = ""
            };
            result.Add(statsOut);

         }
         return result;
      }


      public static List<ZPitching> ToZPitching(this List<Pitching1_cust_Result> listIn) {
         // ---------------------------------------------------------------------
         var result = new List<ZPitching>();
         ZPitching statsOut;
         foreach (var statsIn in listIn) {
            statsOut = new ZPitching() {

               playerID = statsIn.playerID,
               bfp = (int)statsIn.BFP,
               g = (int)statsIn.G,
               gs = (int)statsIn.GS,
               ipOuts = (int)statsIn.IPouts, // IPx3
               h = (int)statsIn.H,
               hr = (int)statsIn.HR,
               bb = (int)statsIn.BB,
               ibb = (int)statsIn.IBB,
               so = (int)statsIn.SO,
               hbp = (int)statsIn.HBP,
               sv = (int)statsIn.SV,
               w = (int)statsIn.W,
               l = (int)statsIn.L,
               r = -1,
               er = (int)statsIn.ER,
               era = (int)statsIn.ERA,
               wp = (int)statsIn.WP,
               bk = -1,

               p_h = 0.0,
               p_hr = 0.0,
               p_bb = 0.0,
               p_so = 0.0,

               rotation = null


            };
            result.Add(statsOut);

         }
         return result;

      }


      public static List<ZFieldingYear> ToZFieldingYear(this List<FieldingYear1_cust_Result> listIn) {
         // ---------------------------------------------------------------------
         var result = new List<ZFieldingYear>();
         ZFieldingYear statsOut;
         foreach (var statsIn in listIn) {
            statsOut = new ZFieldingYear() {

               playerID = statsIn.playerID,
               year = statsIn.Year,
               ZTeam = statsIn.ZTeam,
               Posn = statsIn.Posn,
               inn = statsIn.inn,
               po = statsIn.po,
               a = statsIn.a,
               e = statsIn.e,
               dp = statsIn.dp,
               Rtot = (int)statsIn.Rtot,
               RtotYr = (int)statsIn.Rtot,
               Skill = (int)statsIn.Skill

            };
            result.Add(statsOut);

         }
         return result;

      }



   }

}
