using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MeetingPlan
{
  public class Program
  {
    private static MeetingRepository repository;

    public static void Main()
    {
      repository = new MeetingRepository();
      ShowMainMenu();
    }

    private static void ShowMainMenu()
    {
      Console.Clear();
      Console.WriteLine("1. Просмотр встреч в указанную дату");
      Console.WriteLine("2. Добавить встречу");
      Console.WriteLine("0. Выход");

      var choice = Console.ReadLine();

      switch (choice)
      {
        case "1":
          ShowMeetings();
          break;
        case "2":
          MeetingCreation();
          break;
        case "0":
          Environment.Exit(1);
          break;
      }
    }

    private static void MeetingCreation()
    {
      Console.Clear();
      Console.WriteLine("Укажите тему встречи");
      var name = Console.ReadLine();
      Console.WriteLine("Укажите дату и время начала в виде \"dd/mm/yyyy HH:MM\"");
      var startDate = Console.ReadLine();
      Console.WriteLine("Укажите дату и время окончания \"dd/mm/yyyy HH:MM\"");
      var endDate = Console.ReadLine();
      Console.WriteLine("Укажите за сколько минут до начала напомнить");
      var remindTime = Console.ReadLine();

      var meeting = new Meeting(name, DateTime.Parse(startDate), DateTime.Parse(endDate),
        int.Parse(remindTime));
      Console.WriteLine(repository.Add(meeting));
      Console.ReadLine();
      ShowMainMenu();
    }

    private static void ShowMeetings()
    {
      Console.Clear();
      Console.WriteLine("Введите дату для просмотра в виде \"dd/mm/yyyy\"");
      var date = DateTime.Parse(Console.ReadLine());
      Console.Clear();
      Console.WriteLine("1. Удалить встречу");
      Console.WriteLine("2. Изменить встречу");
      Console.WriteLine("3. Сохранить в файл");
      Console.WriteLine("0. Назад\n");

      var meetings = repository.GetOnDate(date.Date).ToList();
      var i = 0;
      foreach (var meeting in meetings)
      {
        Console.WriteLine(i++ + ") " + meeting);
      }

      var choice = Console.ReadLine();

      switch (choice)
      {
        case "1":
          Console.WriteLine("Укажите индекс встречи");
          repository.Remove(meetings[int.Parse(Console.ReadLine())].Id);
          ShowMainMenu();
          break;
        case "2":
          Console.WriteLine("Укажите индекс встречи");
          MeetingChange(meetings[int.Parse(Console.ReadLine())]);
          break;
        case "3":
          SaveToFile(meetings);
          ShowMainMenu();
          break;
        case "0":
          ShowMainMenu();
          break;
        default:
          ShowMainMenu();
          break;
      }
    }

    private static void MeetingChange(Meeting meeting)
    {
      Console.Clear();
      Console.WriteLine("1. Изменить название");
      Console.WriteLine("2. Изменить дату начала");
      Console.WriteLine("3. Изменить дату оконяания");
      Console.WriteLine("4. Изменить время предупреждения");
      Console.WriteLine("0. Выход");

      var choice = Console.ReadLine();

      switch (choice)
      {
        case "1":
          Console.Clear();
          Console.WriteLine("Введите новое название");
          meeting.Name = Console.ReadLine();
          MeetingChange(meeting);
          break;
        case "2":
          Console.Clear();
          Console.WriteLine("Введите новую дату начала");
          meeting.StartTime = DateTime.Parse(Console.ReadLine());
          MeetingChange(meeting);
          break;
        case "3":
          Console.Clear();
          Console.WriteLine("Введите новую дату окончания");
          meeting.EndTime = DateTime.Parse(Console.ReadLine());
          MeetingChange(meeting);
          break;
        case "4":
          Console.Clear();
          Console.WriteLine("Введите новое время напоминания");
          meeting.RemindTime = meeting.StartTime.AddMinutes(-int.Parse(Console.ReadLine()));
          MeetingChange(meeting);
          break;
        case "0":
          ShowMainMenu();
          break;
      }
    }

    private static async void SaveToFile(IEnumerable<Meeting> meetings)
    {
      var writePath = "Meetings.txt";

      try
      {
        using (StreamWriter sw = new StreamWriter(writePath, false, System.Text.Encoding.Default))
        {
          foreach (var meeting in meetings)
          {
            await sw.WriteLineAsync(meeting.ToString());
          }
        }
        Console.Clear();
        Console.WriteLine("Запись выполнена");
        Console.ReadLine();
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
      }
    }
  }
  
}
