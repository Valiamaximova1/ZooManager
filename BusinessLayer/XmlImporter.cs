using BusinessLayer.DTOs;
using Data;
using Models;
using Shared.Enums;
using Shared.ImportExport;
using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;


public class XmlImporter
{
    public string ImportEventsFromXml(string xmlPath, List<EventDto> existingEvents)
    {
        if (!File.Exists(xmlPath))
            return "Файлът не е намерен.";
        //работи с Event list
        XmlSerializer serializer = new XmlSerializer(typeof(EventList));
        EventList eventList;

        try
        {
            //ред по ред чете като текст
            using var reader = new StreamReader(xmlPath);
            //превръща xml в обект от класа EventList
            eventList = (EventList)serializer.Deserialize(reader)!;
        }
        catch
        {
            return "Форматът на XML файла не е валиден.";
        }

        var factory = new ZooDbContextFactory();
        using var context = factory.CreateDbContext(Array.Empty<string>());

        foreach (var dto in eventList.Events)
        {
            if (!Enum.TryParse<EventType>(dto.Type, out var type))
                return $"Невалиден тип събитие: {dto.Type}";

            bool alreadyExists = existingEvents.Any(e => e.Title == dto.Title && e.Date.Date == dto.Date.Date);
            if (!alreadyExists)
            {
                var newEvent = new Event
                {
                    Title = dto.Title,
                    Type = type,
                    Date = dto.Date,
                    Description = dto.Description
                };
                context.Events.Add(newEvent);
            }
        }

        context.SaveChanges();
        return "OK"; 
    }
}


