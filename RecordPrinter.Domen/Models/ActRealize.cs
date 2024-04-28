using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordPrinter.Domen.Models;

public class ActRealize
{
    public int Id { get; set; }
    public int MasterId { get; set; }
    public Master? Master { get; set; }

    public int RequestId { get; set; }
    public Request? Request { get; set; }
    public DateTime Date { get; set; }
}
