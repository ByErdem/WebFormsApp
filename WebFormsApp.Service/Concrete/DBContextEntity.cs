﻿using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using WebFormsApp.Data;
using WebFormsApp.Service.Abstract;

namespace WebFormsApp.Service.Concrete
{
    public class DBContextEntity : DbContext, IDBContextEntity
    {
        public DBContextEntity() : base(ConnectionString()) { }

        public new Database Database => base.Database;

        public static string ConnectionString()
        {
            string server = "DESKTOP-RI6HN90";
            string databaseName = "NetCad";
            string userName = "sa";
            string password = "0";
            string model = "WebFormsAppModel";
            string metaData = "metadata=res://*/" + model + ".csdl|res://*/" + model + ".ssdl|res://*/" + model + ".msl";
            string provider = "provider=System.Data.SqlClient";
            return $"{metaData};{provider};provider connection string=\"data source={server};database={databaseName};user id={userName};password={password};MultipleActiveResultSets=True;App=EntityFramework\"";
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }

        public override async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }

        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Students> Students { get; set; }



    }
}
