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
    private string _routeFile = @"c:\ProgramData\Notifications\Savings.txt";
    private string _routeFolder = @"c:\ProgramData\Notifications";
    private string _routeFolderImg = @"c:\ProgramData\NotificationsImg";

    public MainWindow()
    {
        InitializeComponent();
        PositionWindow();
    }

    private async void MainImage_Loaded(object sender, RoutedEventArgs e)
    {
        await CreateFolder();
        await CreateFile();
        await ReadConfigFile();

        //_route = Environment.CurrentDirectory;
        //_finalRoute = Directory.GetParent(_route).Parent.Parent.FullName;
        _maxImageNumber = ReadFiles(_routeFolderImg);
        if (_maxImageNumber == 0)
        {
            MessageBox.Show("No hay imágenes que mostrar");
            return;
        }

        string nombreImagen = string.Format($"{_counter}.png");
        MainImage.Source = new BitmapImage(new Uri(@$"{_routeFolderImg}\{nombreImagen}"));

        _counter++;

        if (_counter > _maxImageNumber)
            _counter = 1;
        try
        {
            File.WriteAllText(_routeFile, _counter.ToString());
        }
        catch (Exception ex)
        {
            MessageBox.Show("No se puede acceder al sistema de archivos");
            Console.WriteLine(ex.Message);
        }
    }

    private int ReadFiles(string ruta)
    {
        string[] files = Directory.GetFiles(ruta);

        return files.Length;
    }

    private Task CreateFolder()
    {
        if (!Directory.Exists(_routeFolder))
        {
            Directory.CreateDirectory(_routeFolder);
        }

        if (!Directory.Exists(_routeFolderImg))
        {
            Directory.CreateDirectory(_routeFolderImg);
        }

        return Task.CompletedTask;
    }

    private Task CreateFile()
    {
        //Creamos el archivo de configuración
        if (!File.Exists(_routeFile))
        {
            using FileStream file = File.Create(_routeFile);
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
            using StreamReader reader = new(_routeFile);
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
