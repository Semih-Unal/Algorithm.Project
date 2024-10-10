// See https://aka.ms/new-console-template for more information
using LenaSoftware.Algorithm.Project.Models;
class Program
{
    private static readonly List<EventModel> eventModels = new List<EventModel>
        {
          new EventModel { Id= 1, StartTime = TimeOnly.Parse("10:00"), EndTime = TimeOnly.Parse("12:00"), Location= "A", Priority= 50},
          new EventModel { Id= 2, StartTime = TimeOnly.Parse("10:00"), EndTime = TimeOnly.Parse("11:00"), Location= "B", Priority= 30},
          new EventModel { Id= 3, StartTime = TimeOnly.Parse("11:30"), EndTime = TimeOnly.Parse("12:30"), Location= "A", Priority= 40},
          new EventModel { Id= 4, StartTime = TimeOnly.Parse("14:30"), EndTime = TimeOnly.Parse("16:00"), Location= "C", Priority= 70},
          new EventModel { Id= 5, StartTime = TimeOnly.Parse("14:25"), EndTime = TimeOnly.Parse("15:30"), Location= "B", Priority= 60},
          new EventModel { Id= 6, StartTime = TimeOnly.Parse("13:00"), EndTime = TimeOnly.Parse("14:00"), Location= "D", Priority= 80}
        };

    private static readonly List<DurationBetweenLocation> durationBetweenLocations = new List<DurationBetweenLocation>
        {
            new DurationBetweenLocation {  From = "A", To = "B", DurationMinutes = 15 },
            new DurationBetweenLocation {  From = "A", To = "C", DurationMinutes = 20 },
            new DurationBetweenLocation {  From = "A", To = "D", DurationMinutes = 10 },
            new DurationBetweenLocation {  From = "B", To = "C", DurationMinutes = 5 },
            new DurationBetweenLocation {  From = "B", To = "D", DurationMinutes = 25 },
            new DurationBetweenLocation {  From = "C", To = "D", DurationMinutes = 25 }
        };
    static void Main(string[] args)
    {

        //ilk etkinlik
        EventModel FirstEventModel = eventModels.OrderBy(e => e.StartTime).ThenByDescending(x => x.Priority).First();
        EventModel? model = FirstEventModel;
        int total = 0;
        List<EventModel> eventsAttended = new List<EventModel>();
        do
        {
            eventsAttended.Add(model);
            model = getEventModel(model);
        }
        while (model != null);
        Console.WriteLine("Katılınabilecek Maksimum Etkinlik Sayısı: " + eventsAttended.Count);
        Console.WriteLine("Katılınabilecek Etkinliklerin ID'leri: " + string.Join(",", eventsAttended.Select(x=>x.Id).ToArray()));
        Console.WriteLine("Toplam Değer: " + eventsAttended.Sum(x=>x.Priority));

    }
    /// <summary>
    /// En son girilen etkinliğe göre bir sonraki etkinliği bulur.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    private static EventModel getEventModel(EventModel model)
    {
        var nextModels = eventModels.Where(x => x.StartTime >= model.EndTime).OrderByDescending(x => x.Priority).ToList();

        foreach (var next in nextModels)
        {
            int? duration = getDurationBetweenLocation(model.Location,next.Location);
            if(duration==null) return null;
            TimeOnly startTime= model.StartTime.AddMinutes(duration??0);
            if (startTime<= next.StartTime)
            {
                return next;
            }
        }

        return null;
    }
    /// <summary>
    /// Gönderilen iki lokasyona göre aradaki süreyi döndürür.
    /// </summary>
    /// <param name="loc1"></param>
    /// <param name="loc2"></param>
    /// <returns></returns>
    private static int? getDurationBetweenLocation(string loc1,string loc2)
    {
        //İki lokasyon süresi A'dan B'ye yada B'den A'ya aynı oluğundan '||' operatörü kullanıldı
        DurationBetweenLocation model = durationBetweenLocations.First(x => (x.From == loc1 || x.To == loc1) && (x.From == loc2 || x.To == loc2));
        return model == null ? null : model.DurationMinutes;
    }
}