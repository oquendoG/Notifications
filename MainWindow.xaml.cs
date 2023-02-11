using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Notificaction;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private int _counter;
    private int _maxImageNumber;
    private string _route;
    private string _finalRoute;

    public MainWindow()
    {
        InitializeComponent();
        PositionWindow();
    }

    private async void MainImage_Loaded(object sender, RoutedEventArgs e)
    {
        await CreateFile();
        await ReadConfigFile();

        _route = Environment.CurrentDirectory;
        _finalRoute = Directory.GetParent(_route).Parent.Parent.FullName;
        _maxImageNumber = ReadFiles(@$"{_finalRoute}\Assets");
        string nombreImagen = string.Format($"{_counter}.png");

        MainImage.Source = new BitmapImage(new Uri(@$"{_finalRoute}\Assets\{nombreImagen}"));

        _counter++;

        if (_counter > _maxImageNumber)
            _counter = 1;
        try
        {

            File.WriteAllText(@"c:\Configuration.txt", _counter.ToString());
        } catch(Exception ex)
        {
            MessageBox.Show("No se puedo escribir en el archivo");
            Console.WriteLine(ex.Message);
        }
    }

    private int ReadFiles(string ruta)
    {
        string[] files = Directory.GetFiles(ruta);

        return files.Length;
    }

    private Task CreateFile()
    {
        //Creamos el archivo de configuración
        if (!File.Exists(@"c:\Configuration.txt"))
        {
            using FileStream file = File.Create(@"c:\Configuration.txt");
            byte[] miInfo = new UTF8Encoding(true).GetBytes("1");
            file.Write(miInfo, 0, miInfo.Length);
        }

        return Task.CompletedTask;
    }

    private async Task ReadConfigFile()
    {
        //leemos el archivo de configuracion
        try
        {
            using StreamReader reader = new(@"c:\Configuration.txt");
            _counter = int.Parse(await reader.ReadLineAsync());
        }
        catch (Exception e)
        {
            MessageBox.Show("El archivo no se pudo leer compruebe permisos");
            Console.WriteLine(e.Message);
        }

    }

    private void PositionWindow()
    {
        double screenWidth = SystemParameters.PrimaryScreenWidth;
        double screenHeight = SystemParameters.PrimaryScreenHeight;
        double windowWidth = this.Width;
        double windowHeight = this.Height;

        this.Left = screenWidth - windowWidth;
        this.Top = screenHeight - (windowHeight + 40);
    }
}
