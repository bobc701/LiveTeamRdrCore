namespace DataAccess.Entities
{
    using System;
    
    public partial class Pitching1_cust_Result
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
        public Nullable<int> BFP { get; set; }
        public Nullable<int> W { get; set; }
        public Nullable<int> L { get; set; }
        public Nullable<int> G { get; set; }
        public Nullable<int> GS { get; set; }
        public Nullable<int> SV { get; set; }
        public Nullable<int> IPouts { get; set; }
        public Nullable<int> H { get; set; }
        public Nullable<int> ER { get; set; }
        public Nullable<int> HR { get; set; }
        public Nullable<int> BB { get; set; }
        public Nullable<int> IBB { get; set; }
        public Nullable<int> SO { get; set; }
        public Nullable<int> WP { get; set; }
        public Nullable<int> HBP { get; set; }
        public Nullable<double> ERA { get; set; }
    }
}
