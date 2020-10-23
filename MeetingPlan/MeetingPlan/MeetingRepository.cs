using System;
using System.Collections.Generic;
using System.Linq;

namespace MeetingPlan
{
  public class MeetingRepository
  {
    private static readonly ICollection<Meeting> MeetingsList = new List<Meeting>();

    public string Add(Meeting meeting)
    {
      if (meeting.StartTime <= DateTime.Now)
      {
        return "Дата начала должна быть позже нынешней";
      }

      var meetings = GetOnDate(meeting.StartTime.Date);
      var unsuitables = meetings.FirstOrDefault(m =>
        meeting.StartTime >= m.StartTime && meeting.StartTime <= m.EndTime ||
        meeting.EndTime >= m.StartTime && meeting.EndTime <= m.EndTime);
      if (unsuitables != null)
      {
        return "Конфликт времени с другой встречей";
      }

      MeetingsList.Add(meeting);
      return "Встреча успешно добавлена";
    }

    public Meeting Get(Guid id)
    {
      return MeetingsList.FirstOrDefault(entity => entity.Id == id);
    }

    public void Remove(Guid id)
    {
      MeetingsList.Remove(this.Get(id));
    }

    public IEnumerable<Meeting> GetOnDate(DateTime date)
    {
      return MeetingsList.Where(meeting => meeting.StartTime.Date.Equals(date.Date)).OrderBy(meeting=>meeting.StartTime);
    }
  }
}
