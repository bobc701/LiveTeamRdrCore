namespace DataAccess.Entities
{
    using System;
    
    public partial class GamesByPosn1_cust_Result
    {
        public string playerID { get; set; }
        public string ZTeam { get; set; }
        public int yearID { get; set; }
        public int G_all { get; set; }
        public int GS { get; set; }
        public Nullable<int> G_p { get; set; }
        public Nullable<int> G_c { get; set; }
        public Nullable<int> G_1b { get; set; }
        public Nullable<int> G_2b { get; set; }
        public Nullable<int> G_3b { get; set; }
        public Nullable<int> G_ss { get; set; }
        public Nullable<int> G_lf { get; set; }
        public Nullable<int> G_cf { get; set; }
        public Nullable<int> G_rf { get; set; }
        public Nullable<int> G_of { get; set; }
        public int G_dh { get; set; }
    }
}
