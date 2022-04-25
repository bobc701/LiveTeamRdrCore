namespace DataAccess.Entities
{
    using System;
    
    public partial class Batting1_cust_Result
    {
        public string playerID { get; set; }
        public string lgID { get; set; }
        public int yearID { get; set; }
        public string ZTeam { get; set; }
        public string nameLast { get; set; }
        public string nameFirst { get; set; }
        public string UseName { get; set; }
        public string bats { get; set; }
        public string throws { get; set; }
        public string PlayerCategory { get; set; }
        public Nullable<int> G { get; set; }
        public Nullable<int> AB { get; set; }
        public Nullable<int> R { get; set; }
        public Nullable<int> H { get; set; }
        public Nullable<int> C2B { get; set; }
        public Nullable<int> C3B { get; set; }
        public Nullable<int> HR { get; set; }
        public Nullable<int> RBI { get; set; }
        public Nullable<int> SB { get; set; }
        public Nullable<int> CS { get; set; }
        public Nullable<int> BB { get; set; }
        public Nullable<int> SO { get; set; }
        public Nullable<int> IBB { get; set; }
        public Nullable<int> HBP { get; set; }
        public Nullable<int> SH { get; set; }
        public Nullable<int> SF { get; set; }
        public Nullable<int> GIDP { get; set; }
    }
}
