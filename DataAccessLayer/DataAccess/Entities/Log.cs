namespace DataAccess.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Log
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LogId { get; set; }

        public DateTime Time { get; set; }

        public string System { get; set; }

        public string User { get; set; }

        public string Event { get; set; }

        public string Group { get; set; }

        public string Viewers { get; set; }

        public string Message { get; set; }
    }
}
