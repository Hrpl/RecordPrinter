using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordPrinter.Domen.Models;

public class Request
{
    public int Id { get; set; }
    public int PrinterId { get; set; }
    public Printer? Printer { get; set; } 

    public DateTime Date { get; set; }
    
    public string Problem { get; set; }
    public string Status { get; set; }

    public ActRealize? ActRealize { get; set; }
}
