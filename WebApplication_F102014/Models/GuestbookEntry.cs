using System;
using System.Data.Entity;

namespace WebApplication_F102014.Models
{
    public class GuestbookEntry
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public DateTime DateAdded { get; set; }
        public int UserId { get; set; }
    }

    public class GuestbookContext : DbContext
    {
        public GuestbookContext() : base("WebsiteDB") { }

        public DbSet<GuestbookEntry> Entries { get; set; }

        public System.Data.Entity.DbSet<WebApplication_F102014.Models.User> Users { get; set; }
    }
}