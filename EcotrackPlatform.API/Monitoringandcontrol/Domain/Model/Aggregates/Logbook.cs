using System.Diagnostics;

namespace EcotrackPlatform.API.Monitoringandcontrol.Domain.Model.Aggregates;

public class Logbook
{
    public int Id { get; private set; }
    public string Activity { get; private set; }
    public DateTime Duration {get; private set;}
    public int Volume {get; private set;}
    public string Evident {get; private set;}
    public int ParcelId { get;  private set; }
    
    private Logbook(){}
    
    public Logbook(string activity, DateTime duration, int volume, string evident)
    {
        this.Activity = activity;
        this.Duration = duration;
        this.Volume = volume;
        this.Evident = evident;
    }
}