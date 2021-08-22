using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Diagnostics;
using Microsoft.VisualBasic.FileIO;

namespace TPDPInteractiveMap
{
    public static class Globals
    {
        public static List<Cases> globalCases = new List<Cases>();
    }

    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string startupPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        public static string startupResources = $"{startupPath}\\Resources";
        public static string townPath = $"{startupResources}\\gn_dat5.arc\\map\\town";
        public static string[] neededFiles = new string[] {
            "town0Base.png",
            "town1Base.png",
            "town2Base.png",
            "town3Base.png",
            "MapToTownMapData.csv",
            "townMapData0.csv",
            "townMapData1.csv",
            "townMapData2.csv",
            "townMapData3.csv"
        };

        [Serializable]
        class MissingFileException : Exception
        {
            public MissingFileException(){}
        }

        public MainWindow()
        {
            checkBinFolder();
            InitializeComponent();
            LoadFiles();
        }

        /// <summary>
        /// Regarde si les fichiers contenus dans le dossier de ressources sont bien là et qu'il n'en manque pas.
        /// </summary>
        private void checkBinFolder()
        {
            try
            {
                List<string> files = new List<string>(Directory.EnumerateFiles(startupResources));
                for (int i = 0; i < files.Count; i++)
                {
                    files[i] = Path.GetFileName(files[i]);
                }
                foreach (string item in neededFiles)
                {
                    if (!files.Contains(item))
                    {
                        throw new MissingFileException();
                    }
                }
            }
            catch (DirectoryNotFoundException)
            {
                Directory.CreateDirectory(startupResources);
            }
            catch (MissingFileException)
            {
                if (Properties.Settings.Default.GamePath == "")
                {
                    System.Windows.Forms.FolderBrowserDialog browse = new System.Windows.Forms.FolderBrowserDialog();
                    browse.Description = "Where is located your game folder ?";
                    browse.ShowDialog();

                    if (browse.SelectedPath == "")
                    {
                        MessageBox.Show("No folden choosen, closing this program.");
                        Close();
                    }
                    Properties.Settings.Default.GamePath = browse.SelectedPath;
                    Properties.Settings.Default.Save();
                }
                extractGameFiles();
                foreach (string file in neededFiles)
                {
                    File.Copy(townPath + "\\" + file, startupResources + "\\" + file, true);
                }
                // Suppression de ce qui a été extrait par extractGameFiles()
                DirectoryInfo di = new DirectoryInfo(startupResources);
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }
            }
        }

        /// <summary>
        /// Extrait les fichiers du jeu en utilisant diffgen.exe pour chercher les fichiers de gn_dat5.arc/map/town
        /// </summary>
        private void extractGameFiles()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = false;
            startInfo.FileName = "diffgen.exe";
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.Arguments = $"-i {Properties.Settings.Default.GamePath} -o {startupResources} --extract";

            try
            {
                // Start the process with the info we specified.
                // Call WaitForExit and then the using statement will close.
                using (Process exeProcess = Process.Start(startInfo))
                {
                    exeProcess.WaitForExit();
                }
            }
            catch
            {
                MessageBox.Show("Something went wrong. Terminating.");
                Properties.Settings.Default.GamePath = "";
                Properties.Settings.Default.Save();
                Close();
            }
        }

        /// <summary>
        /// Load les fichiers correspondant aux différentes map, Gensokyo = town0base.png, etc.
        /// </summary>
        private void LoadFiles()
        {
            //ImageGensokyo
            BitmapImage source = new BitmapImage();
            source.BeginInit();
            source.StreamSource = new MemoryStream(File.ReadAllBytes($"{startupResources}\\town0Base.png"));
            source.EndInit();
            ImageGensokyo.ImageSource = source;
            importGensokyoNodes();

            //Underworld
            source = new BitmapImage();
            source.BeginInit();
            source.StreamSource = new MemoryStream(File.ReadAllBytes($"{startupResources}\\town1Base.png"));
            source.EndInit();
            ImageUnderworld.Source = source;

            //Makai
            source = new BitmapImage();
            source.BeginInit();
            source.StreamSource = new MemoryStream(File.ReadAllBytes($"{startupResources}\\town2Base.png"));
            source.EndInit();
            ImageMakai.Source = source;

            //???
            source = new BitmapImage();
            source.BeginInit();
            source.StreamSource = new MemoryStream(File.ReadAllBytes($"{startupResources}\\town3Base.png"));
            source.EndInit();
            ImageUnknown.Source = source;
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Gère les grid de la map de Gensokyo
        /// </summary>
        private void importGensokyoNodes()
        {
            GensokyoMap map = new GensokyoMap();

            List<ColumnDefinition> gridColumns = map.returnColumns();
            List<RowDefinition> gridRows = map.returnRows();
            for (int i = 0; i < gridColumns.Count; i++)
            {
                gridColumns[i].Tag = i;
                gensokyoMap.ColumnDefinitions.Add(gridColumns[i]);
            }
            for (int j = 0; j < gridRows.Count; j++)
            {
                gridRows[j].Tag = j;
                gensokyoMap.RowDefinitions.Add(gridRows[j]);
            }

            // Créer les cases par rapport aux grids.
            List<Cases> cases = new List<Cases>();
            int minX = 0;
            int minY = 0;
            for (int i = 0; i < gridRows.Count; i++)
            {
                for (int j = 0; j < gridColumns.Count; j++)
                {
                    Cases case1 = new Cases();
                    case1.positionX = j;
                    case1.positionY = i;
                    case1.minX = minX;
                    case1.maxX = minX + 16;
                    case1.minY = minY;
                    case1.maxY = minY + 16;
                    cases.Add(case1);
                    minX += 16;
                }
                minY += 16;
            }
            // Enregistre les cases dans la variable globale Globals.globalCases accessible de partout (static)
            Globals.globalCases = cases;

            // Remplissage de la grid
            using (TextFieldParser parser = new TextFieldParser($"{startupResources}\\townMapData0.csv"))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                parser.ReadLine();
                int j = 0;
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    int iconNum;
                    Globals.globalCases[j].name = fields[4];
                    iconNum = Convert.ToInt32(fields[2]);
                    // Remet les images correctement (icons vers iconX.png)
                    switch (iconNum)
                    {
                        case 1:
                            iconNum = 0;
                            break;
                        case 3:
                            iconNum = 2;
                            break;
                        case 5:
                            iconNum = 4;
                            break;
                        default:
                            iconNum = 0;
                            break;
                    }
                    for (int i = 0; i < fields.Length; i++)
                    {
                        // Créer les images dans les différentes cases
                        Image Mole = new Image();
                        Mole.Width = 16;
                        Mole.Height = 16;
                        ImageSource MoleImage = new BitmapImage(new Uri($"pack://application:,,,/Images/icon{iconNum}.png"));
                        Mole.Source = MoleImage;
                        Grid.SetRow(Mole, Globals.globalCases[j].positionY);
                        Grid.SetColumn(Mole, Globals.globalCases[j].positionX + 1);
                        gensokyoMap.Children.Add(Mole);
                    }
                    j++;
                }
            }
        }

        /// <summary>
        /// Check la position de la souris du joueur et change le nom affiché à côté de celle-ci.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GensokyoMap_MouseMove(object sender, MouseEventArgs e)
        {
            Point point = e.GetPosition(gensokyoMap);
            int X = 0;
            int Y = 0;
            string name;
            foreach (Cases case1 in Globals.globalCases)
            {
                if (case1.minX < point.X && case1.maxX > point.X)
                {
                    X = case1.positionX;
                }
                if (case1.minY < point.Y && case1.maxY > point.Y)
                {
                    Y = case1.positionY;
                }
            }
            int index;
            if (Y == 0 && X == 0)
                index = 0;
            else
            {
                // Index est 28 * Y, pour changer de row, et + X pour une nouvelle colonne, - 1 parce que sinon ça merde
                index = (28 * Y) + X - 1;
            }
            name = Globals.globalCases[index].name;
            if (name == "0")
                LabelGensokyo.Content = "";
            else
                LabelGensokyo.Content = name;
            Thickness thicc = new Thickness();
            thicc.Left = point.X;
            thicc.Top = point.Y - 16;
            thicc.Right = 0;
            thicc.Bottom = 0;
            LabelGensokyo.Margin = thicc;
            //pos.Text = $"{X}, {Y}, {index}";
        }

        /// <summary>
        /// Gère la petite fenêtre qui se lance quand on clique sur un endroit sur la map (qui n'a pas un nom égal à "0").
        /// Si c'est le cas, alors ne rien faire, sinon lancer la fenêtre, check les valeurs dans le fichier de conf json.
        /// Puis, si la configuration n'existe pas, alors la créer entièrement vide.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gensokyoMap_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point point = e.GetPosition(gensokyoMap);
            int X = 0;
            int Y = 0;
            string name;
            foreach (Cases case1 in Globals.globalCases)
            {
                if (case1.minX < point.X && case1.maxX > point.X)
                {
                    X = case1.positionX;
                }
                if (case1.minY < point.Y && case1.maxY > point.Y)
                {
                    Y = case1.positionY;
                }
            }
            int index;
            if (Y == 0 && X == 0)
                index = 0;
            else
            {
                index = (28 * Y) + X - 1;
            }
            name = Globals.globalCases[index].name;
            if (name == "0")
                return;
            Utils util = new Utils();
            List<LocationInfo> locationInfos = util.readJson();
            LocationInfo clickedLocation = locationInfos[0];
            bool found = false;
            // Si la configuration n'existe pas, la créer puis la lire, sinon on la lis juste.
            for (int i = 0; i < locationInfos.Count; i++)
            {
                if (locationInfos[i].Index == index)
                {
                    found = true;
                    clickedLocation = locationInfos[i];
                }
            }
            if (!found)
            {
                clickedLocation = Utils.createLocationInfo(name, index);
                locationInfos.Add(clickedLocation);

                using (FileStream file = new FileStream(Utils.configFile, FileMode.Open))
                {
                    var options = new JsonSerializerOptions { WriteIndented = true };
                    string jsonString = JsonSerializer.Serialize(locationInfos, options);
                    byte[] bytes = Encoding.UTF8.GetBytes(jsonString);
                    file.Write(bytes, 0, bytes.Length);
                }
            }
            MapWindow mapWindow = new MapWindow();
            mapWindow.Title = clickedLocation.Name;
            mapWindow.Show();
        }
    }
}
