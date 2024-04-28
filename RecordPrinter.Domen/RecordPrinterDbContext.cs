using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RecordPrinter.Domen.Models;

namespace RecordPrinter.Domen;

public class RecordPrinterDbContext : DbContext
{
    public DbSet<Cartridge> Cartridges { get; set; }
    public DbSet<Printer> Printers { get; set; }
    public DbSet<Request> Requests { get; set; }
    public DbSet<ActRealize> ActRealize { get; set; }
    public DbSet<Master> Master { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=RecordPrinter;Trusted_Connection=True");
    }

}
