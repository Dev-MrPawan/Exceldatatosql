namespace Excel_Data.Models
{
    public class Regmodel
    {
        public string indexid { get; set; }
        public string username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string tdate { get; set; }
        public string email { get; set; }
        public string result { get; set; }
    }

    public class ScoreResult
    {
        public string name { get; set; }
        public string age { get; set; }
        public string email { get; set; }
    }

    public class up
    {
        public string indexid { get; set; }
        public string username { get; set; }
        public string filename { get; set; }
        public string tdate { get; set; }
    }

    public class uplist { 
    
        public List<up> list { get; set; }
    }

    public class ex
    {
        public string indexid { get; set; }
        public string name { get; set; }
        public string age { get; set; }
        public string email { get; set; }
        public string tdate { get; set; }
    }

    public class exlist
    {
        public List<ex> list { get; set; }
    }
}
