namespace DataAccess.Entities
{
    using System;
    
    public partial class FieldingYear1_cust_Result
    {
        public string playerID { get; set; }
        public int Year { get; set; }
        public string ZTeam { get; set; }
        public short Posn { get; set; }
        public Nullable<double> inn { get; set; }
        public Nullable<short> po { get; set; }
        public Nullable<short> a { get; set; }
        public Nullable<short> e { get; set; }
        public Nullable<short> dp { get; set; }
        public Nullable<short> Rtot { get; set; }
        public Nullable<short> RtotYr { get; set; }
        public Nullable<short> Skill { get; set; }
    }
}
