using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebFormsApp.Data;

namespace WebFormsApp.Service.Abstract
{
    public interface IDBContextEntity
    {
        DbSet<Users> Users { get; set; }
        DbSet<Students> Students { get; set; }
        Database Database { get; }

        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
