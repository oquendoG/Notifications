using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Notificaction;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private int _counter = 1;
    private int _previousImgNumber;
    private int _maxImageNumber;
    private string _userFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\NotificationsImg";
    protected string _routeFile = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Savings.txt";

    public MainWindow()
    {
        InitializeComponent();
        PositionWindow();
    }

    private async void MainImage_Loaded(object sender, RoutedEventArgs e)
    {
        await CreateFolder();
        await CreateFile();
        await ReadSavingsFile();

        _maxImageNumber = ReadFiles(_userFolder);
        if (_maxImageNumber == 0)
        {
            MessageBox.Show("No hay imágenes que mostrar");
            return;
        }

        await ShowImage();
    }

    private Task ShowImage()
    {
        string nombreImagen = string.Format($"{_counter}{DefineExtension(_userFolder)}");
        try
        {
            MainImage.Source = new BitmapImage(new Uri(@$"{_userFolder}\{nombreImagen}", UriKind.RelativeOrAbsolute));
        }
        catch (Exception ex)
        {
            MessageBox.Show("Ruta de imágen incorrecta");
            Console.WriteLine(ex.Message);
        }

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

        return Task.CompletedTask;
    }

    private int ReadFiles(string ruta)
    {
        string[] files = Directory.GetFiles(ruta);

        return files.Length;
    }

    private string DefineExtension(string ruta)
    {
        List<string> files = (Directory.GetFiles(ruta)).ToList();
        string hasExtension = files[0];
        if (hasExtension.Contains(".jpg"))
        {
            return ".jpg";
        }

        return ".png";
    }

    private Task CreateFolder()
    {
        if (!Directory.Exists(_userFolder))
        {
            Directory.CreateDirectory(_userFolder);
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

    private async Task ReadSavingsFile()
    {
        //leemos el archivo de Savings.txt
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

    private void Button_ClickPrevious(object sender, RoutedEventArgs e)
    {
        int previousImage = GetPreviousImageName();

        try
        {
            MainImage.Source = new BitmapImage(new Uri(@$"{_userFolder}\{previousImage}{DefineExtension(_userFolder)}", UriKind.RelativeOrAbsolute));
            _counter--;
        }
        catch (Exception ex)
        {
            MessageBox.Show("Ruta de imágen incorrecta");
            Console.WriteLine(ex.Message);
        }

    }

    private void Button_ClickNext(object sender, RoutedEventArgs e)
    {
        int currentImage = GetNextImageName();

        if (currentImage == 0)
        {
            currentImage = 1;
        }
        try
        {
            MainImage.Source = new BitmapImage(new Uri(@$"{_userFolder}\{currentImage}{DefineExtension(_userFolder)}", UriKind.RelativeOrAbsolute));

            _counter++;
        }
        catch (Exception ex)
        {
            MessageBox.Show("Ruta de imágen incorrecta");
            Console.WriteLine(ex.Message);
        }

    }

    private int GetNextImageName()
    {
        _previousImgNumber = _counter;

        if (_previousImgNumber > _maxImageNumber)
        {
            _counter = 1;
            _previousImgNumber = 1;
        }

        return _previousImgNumber;
    }

    private int GetPreviousImageName()
    {
        _previousImgNumber = _counter - 2;

        if (_previousImgNumber == 0)
        {
            _previousImgNumber = _maxImageNumber;
        }

        if(_previousImgNumber < 0)
        {
            _previousImgNumber = _maxImageNumber - 1;
            _counter+=3;
        }

        return _previousImgNumber;
    }
}