using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using OfficeOpenXml;
using RecordPrinter.Domen;
using RecordPrinter.Domen.Models;
using Microsoft.Office.Interop.Word;
using Word = Microsoft.Office.Interop.Word;
using Microsoft.VisualBasic.ApplicationServices;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;

namespace RecordPrinter.WPF;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : System.Windows.Window
{
    
    public MainWindow()
    {
        InitializeComponent();

        filterRequest.ItemsSource = new List<string> { "Найти все", "Только выполненные", "Только не выполненные"};
    }

    private void btnExport_Click(object sender, RoutedEventArgs e)
    {
    }

    private List<DataRequest> FillRequestGrid()
    {
        using (RecordPrinterDbContext db = new RecordPrinterDbContext())
        {
            var list = db.Requests.Select(x => new DataRequest
            {
                Id = x.Id,
                PrinterId = x.PrinterId,
                Date = x.Date,
                Problem = x.Problem,
                Status = x.Status
            }).ToList();

            return list;
        }
    }

    private List<DataPrinter> FillPrinterGrid()
    {
        using (RecordPrinterDbContext db = new RecordPrinterDbContext())
        {
            var list = db.Printers.Select(x => new DataPrinter
            {
                Id = x.Id,
                CartridgeId = x.CartridgeId,
                Model = x.Model,
                Manufacturer = x.Manufacturer,
                Description = x.Description,
                Type = x.Type,
                Place = x.Place
            }).ToList();

            return list;
        }
    }

    private void ViewRequest_Click(object sender, RoutedEventArgs e)
    {
        RequestBlock.Visibility = Visibility.Visible;
        PrinterBlock.Visibility = Visibility.Collapsed;
        CreateRequestForm.Visibility = Visibility.Collapsed;
        CreateRealizeRequesForm.Visibility = Visibility.Collapsed;

        requestGrid.ItemsSource = FillRequestGrid();
    }

    private void ViewPrinter_Click(object sender, RoutedEventArgs e)
    {
        
        PrinterBlock.Visibility = Visibility.Visible;
        CreateRequestForm.Visibility = Visibility.Collapsed;
        CreateRealizeRequesForm.Visibility = Visibility.Collapsed;
        RequestBlock.Visibility = Visibility.Collapsed;

        dataGrid.ItemsSource = FillPrinterGrid();
    }

    private void CreateNewRequest_Click(object sender, RoutedEventArgs e)
    {
        PrinterBlock.Visibility = Visibility.Collapsed;
        RequestBlock.Visibility = Visibility.Collapsed;
        CreateRealizeRequesForm.Visibility = Visibility.Collapsed;
        CreateRequestForm.Visibility = Visibility.Visible;
        using (RecordPrinterDbContext db = new RecordPrinterDbContext())
        {
            if(AllPrinter.Items.Count == 0)
            {
                var printers = db.Printers.ToList();
                foreach (var printer in printers)
                {
                    AllPrinter.Items.Add(printer.Model);
                }
            }
        }
    }

    private void CreateRequestButton(object sender, RoutedEventArgs e)
    {
        using (RecordPrinterDbContext db = new RecordPrinterDbContext())
        {
            var printer = db.Printers.Where(p => p.Model == AllPrinter.SelectedItem.ToString()).First();
            Request request = new Request()
            {
                PrinterId = printer.Id,
                Date = DateTime.Now,
                Problem = ProblemText.Text,
                Status = "Create"
            };
            db.Requests.Add(request);
            try
            {
                // Создание нового экземпляра приложения Word
               /* Word.Application wordApp = new Word.Application();
                Document wordDoc = wordApp.Documents.Add();

                // Вставка текста в документ
                wordApp.Selection.TypeText($"Заявка для принтера {printer.Model} в {request.Date} создана, с проблемой: {ProblemText.Text}");

                // Сохранение документа под новым именем
                object fileFormat = WdSaveFormat.wdFormatDocumentDefault;
                wordDoc.SaveAs($"C:\\Users\\Денис\\Desktop\\{printer.Model}_{request.Date}", ref fileFormat);

                // Закрытие документа и приложения Word
                wordDoc.Close();
                wordApp.Quit();*/


                db.SaveChanges();
                MessageBox.Show("Заявка создана", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка создания заявки", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private void CreateNewActRealzie_Click(object sender, RoutedEventArgs e)
    {
        CreateRealizeRequesForm.Visibility = Visibility.Visible;
        PrinterBlock.Visibility = Visibility.Collapsed;
        CreateRequestForm.Visibility = Visibility.Collapsed;
        RequestBlock.Visibility = Visibility.Collapsed;
        using (RecordPrinterDbContext db = new RecordPrinterDbContext())
        {
            if (AllMaster.Items.Count == 0)
            {
                var masters = db.Master.ToList();
                foreach (var master in masters)
                {
                    AllMaster.Items.Add(master.Name);
                }
            }
        }
    }

    private void CreateActRealizeButton(object sender, RoutedEventArgs e)
    {
        using (RecordPrinterDbContext db = new RecordPrinterDbContext())
        {
            var master = db.Master.Where(p => p.Name == AllMaster.SelectedItem.ToString()).First();
            var request = db.Requests.Find(Convert.ToInt32(NumberRequest.Text));
            if(request != null)
            {
                if (request.Status != "Complete")
                {
                    ActRealize act = new ActRealize()
                    {
                        RequestId = request.Id,
                        Date = DateTime.Now,
                        MasterId = master.Id
                    };
                    request.Status = "Complete";
                    db.ActRealize.Add(act);
                    db.Requests.Update(request);
                    try
                    {
                        db.SaveChanges();
                        MessageBox.Show("Заявка выполнена", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Произошла ошибка сохранения", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Заявка уже была выполнена", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else {
                MessageBox.Show("Заявки не существует", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }
    }

    private void searchPrinters_TextChanged(object sender, TextChangedEventArgs e)
    {
        var current = FillPrinterGrid();
        if (SearchPrinter.Text.Length > 0)
        {
            current = current.Where(p => p.Model.Contains(SearchPrinter.Text)).ToList();
        }
        dataGrid.ItemsSource = current;
    }

    private void CleanSearch(object sender, RoutedEventArgs e)
    {
        dataGrid.ItemsSource = FillPrinterGrid();
        requestGrid.ItemsSource = FillRequestGrid();
        SearchPrinter.Text = "";
    }

    private void filterRequest_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        using (RecordPrinterDbContext db = new RecordPrinterDbContext())
        {
            var current = FillRequestGrid();

            if (filterRequest.SelectedIndex != -1)
            {
                if (filterRequest.SelectedValue == "Только выполненные")
                {
                    current = current.Where(u => u.Status == "Complete").ToList();
                }
                else if (filterRequest.SelectedValue == "Только не выполненные")
                {
                    current = current.Where(u => u.Status == "Create").ToList();
                }
            }
            requestGrid.ItemsSource = current;
        }
            
    }
}

internal class DataPrinter
{
    public int Id { get; set; }
    public int CartridgeId { get; set; }
    public string Model { get; set; }
    public string Manufacturer { get; set; }
    public string Description { get; set; }
    public string Type { get; set; }
    public int Place { get; set; }

}

internal class DataRequest : Request
{
    public int Id { get; set; }
    public int PrinterId { get; set; }

    public DateTime Date { get; set; }

    public string Problem { get; set; }
    public string Status { get; set; }

}