using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using _7semester_ASP_SecondTask;
using _7semester_ASP_SecondTask.Models;

namespace _7semester_ASP_SecondTask.Data
{
	public class SlavesContext : DbContext
	{
		public SlavesContext(DbContextOptions<SlavesContext> options)
			: base(options)
		{
			Database.EnsureDeleted();
			Database.EnsureCreated();

			if (!Masters.Any())
			{
				Master widow = new Master { Name = "Widow" };
				Master oracle = new Master { Name = "Oracle" };

				Masters.AddRange(oracle, widow);

				Slave slave1 = new Slave { SkinTone = "Black", Master = null, Age = 18, Price = 100 };
				Slave slave2 = new Slave { SkinTone = "WhiterThanBlack", Master = oracle, Age = 14, Price = 200 };
				Slave slave3 = new Slave { SkinTone = "TotallyBlack", Master = widow, Age = 16, Price = 150 };
				Slave slave4 = new Slave { SkinTone = "Black", Master = oracle, Age = 18, Price = 300 };
				Slave slave5 = new Slave { SkinTone = "WhiterThanBlack", Master = widow, Age = 14, Price = 250 };
				Slave slave6 = new Slave { SkinTone = "TotallyBlack", Master = null, Age = 16, Price = 350 };

				Slaves.AddRange(slave1, slave2, slave3, slave4, slave5, slave6);

				SaveChanges();
			}
		}

		public DbSet<_7semester_ASP_SecondTask.Models.Slave> Slaves { get; set; } = default!;
		public DbSet<_7semester_ASP_SecondTask.Models.Master> Masters { get; set; } = default!;
	}
}
