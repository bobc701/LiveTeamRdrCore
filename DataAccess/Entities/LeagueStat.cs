namespace DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class LeagueStat
    {
        public int yearID { get; set; }
        public string lgID { get; set; }
        public Nullable<int> G { get; set; }
        public Nullable<int> PA { get; set; }
        public int AB { get; set; }
        public Nullable<int> R { get; set; }
        public int H { get; set; }
        public int C2B { get; set; }
        public int C3B { get; set; }
        public int HR { get; set; }
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
        public Nullable<int> IPouts { get; set; }
    }
}
