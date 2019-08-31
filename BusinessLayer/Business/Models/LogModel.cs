namespace Business.Models
{
    using DataAccess.Entities;
    using System;

    public class LogModel : IEntityModel<Log>
    {
        public DateTime Time { get; set; }

        public string System { get; set; }

        public string User { get; set; }

        public string Event { get; set; }

        public string Group { get; set; }

        public string Viewers { get; set; }

        public string Message { get; set; }

        public Log ToEntity()
        {
            return new Log
            {
                Time = this.Time,
                System = this.System,
                User = this.User,
                Event = this.Event,
                Group = this.Group,
                Viewers = this.Viewers,
                Message = this.Message
            };
        }

        public void ToModel(Log entity)
        {
            if (entity != null)
            {
                this.Time = entity.Time;
                this.System = entity.System;
                this.User = entity.User;
                this.Event = entity.Event;
                this.Group = entity.Group;
                this.Viewers = entity.Viewers;
                this.Message = entity.Message;
            }
        }
    }
}
