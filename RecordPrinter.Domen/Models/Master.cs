using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordPrinter.Domen.Models;

public class Master
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public DateOnly Birthday { get; set; }

    public IEnumerable<ActRealize?> ActRealize { get; set;}
}
