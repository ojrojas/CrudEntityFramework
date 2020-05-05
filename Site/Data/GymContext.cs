using KallpaBox.Core.Entities;
using System.Data.Entity;

namespace KallpaBox.Site.Data
{
    public class GymContext : DbContext
    {
        public GymContext() : base("name=GymContext") { }

        public DbSet<Client> Clientes { get; set; }
        public DbSet<FingerPrint> Fingers { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<ServiceGym> ServicesGyms { get; set; }
        public DbSet<ServiceGymContract> ServiceGymContracts { get; set; }
        public DbSet<ServiceGymType> ServiceGymTypes { get; set; }
        public DbSet<SessionGym> SessionGyms { get; set; }
    }
}