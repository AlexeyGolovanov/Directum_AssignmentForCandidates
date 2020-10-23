using System;
using System.Timers;

namespace MeetingPlan
{
  public class Meeting
  {
    public Meeting(string name, DateTime startTime, DateTime endTime, int reminderTime)
    {
      this.Id = Guid.NewGuid();
      this.Name = name;
      this.StartTime = startTime;
      this.EndTime = endTime;
      this.RemindTime = this.StartTime.AddMinutes(-reminderTime);

      this.timer = new Timer {Interval = 60000};
      this.timer.Start();
      this.timer.Elapsed += this.CheckTime;
      this.Remind += RemindMessage;
    }

    private readonly Timer timer;

    public delegate void ReminderHandler(string message);

    public event ReminderHandler Remind;

    public Guid Id { get; }

    public string Name { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public DateTime RemindTime { get; set; }

    public override string ToString()
    {
      return Name + " " + this.StartTime.ToString("MM/dd/yyyy HH:mm") + " - " + this.EndTime.ToString("MM/dd/yyyy HH:mm");
    }

    private void CheckTime(object source, ElapsedEventArgs e)
    {
      if (DateTime.Now >= this.RemindTime)
      {
        this.Remind?.Invoke("Встреча " + this.Name + " Начнется в " + this.StartTime.ToString("MM/dd/yyyy HH:mm"));

        if (DateTime.Now >= this.StartTime)
        {
          this.timer.Stop();
        }
      }
    }

    private void RemindMessage(string message)
    {
      Console.WriteLine(message);
    }
  }
}
