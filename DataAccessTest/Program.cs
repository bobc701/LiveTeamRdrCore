using DataAccess.Entities;
using DataAccess;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessTest
{
   public class Program
   {
      static void Main(string[] args) {
         Console.WriteLine("Hello World!");

         string connStr = 
            @"Data Source=s20.winhost.com;
            Initial Catalog=DB_133455_mlbhistory;
            User ID=DB_133455_mlbhistory_user;
            Password=klubX511;";
         DbxMlbHistory dbx = new(connStr);
         
         //LeagueStat stat = dbx.GetLeagueStatSingle("NL", 2021);
         //Console.WriteLine($"GetLeagueStatSingle: {stat.AB}");

         List<Batting1_app_Result> list1 = dbx.Batting1_app("BOS", 2004);
         Console.WriteLine($"Batting1_app: {list1.First().playerID}");

      }
   }
}
