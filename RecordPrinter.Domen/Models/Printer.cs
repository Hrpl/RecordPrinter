using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordPrinter.Domen.Models;

public class Printer
{
    public int Id { get; set; }
    public int CartridgeId { get; set; }
    public Cartridge? Cartridge { get; set; }
    public string Model { get; set; }
    public string Manufacturer { get; set; }
    public string Description { get; set; }
    public string Type { get; set; }
    public int Place { get; set; }

    public IEnumerable<Request> Requests { get; set; }
}
