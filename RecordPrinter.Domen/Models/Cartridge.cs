using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordPrinter.Domen.Models;

public class Cartridge
{
    public int Id { get; set; }
    public string Model { get; set; }
    public string Manufacturer { get; set; }
    public string Type { get; set; }
    public string Color { get; set; }
    public int Resurse  { get; set; }
    
    public IEnumerable<Printer> Printers { get; set; }
    
}
