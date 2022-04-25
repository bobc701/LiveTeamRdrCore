namespace DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class ZFielding
    {
        public string playerID { get; set; }
        public string lahmanID { get; set; }
        public string nameFirst { get; set; }
        public string nameLast { get; set; }
        public Nullable<short> p1 { get; set; }
        public Nullable<short> p2 { get; set; }
        public Nullable<short> p3 { get; set; }
        public Nullable<short> p4 { get; set; }
        public Nullable<short> p5 { get; set; }
        public Nullable<short> p6 { get; set; }
        public Nullable<short> p7 { get; set; }
        public Nullable<short> p8 { get; set; }
        public Nullable<short> p9 { get; set; }
        public string Source { get; set; }
        public Nullable<System.DateTime> debut { get; set; }
        public Nullable<System.DateTime> finalGame { get; set; }
    }
}
