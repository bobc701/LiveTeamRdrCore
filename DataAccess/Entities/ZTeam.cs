namespace DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class ZTeam
    {
        public int yearID { get; set; }
        public string teamID { get; set; }
        public string ZTeam1 { get; set; }
        public string lgID { get; set; }
        public string LineName { get; set; }
        public string City { get; set; }
        public string NickName { get; set; }
        public bool UsesDH { get; set; }
    }
}
