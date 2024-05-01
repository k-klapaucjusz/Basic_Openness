
using Siemens.Engineering;
using Siemens.Engineering.Compiler;
using Siemens.Engineering.Hmi;
using Siemens.Engineering.HW;
using Siemens.Engineering.HW.Features;
using Siemens.Engineering.SW;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
//using Basic_Openness;
using System.Globalization;
using Nauka_01;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.Win32;
using Basic_Openness.Models;
using Siemens.Engineering.SW.Blocks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Basic_Openness
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        //pierwsze testy 
        private ApiWrapper_test _apiWrapper_test = new ApiWrapper_test();
        private TiaPortalMode _tiaPortalMode;
        //ppierwsze testy


        //#region properties

        //public TiaPortalProcess CurrentTiaPortalProcess
        //{
        //    get;
        //    set;
        //}

        //public TiaPortal TiaPortal
        //{
        //    get;
        //    set;
        //}

        //public bool TiaPortalIsDisposed
        //{
        //    get;
        //    set;
        //}

        //public bool IsModified
        //{
        //    get;
        //    set;
        //}

        //public Project CurrentProject
        //{
        //    get;
        //    set;
        //}

        //public Project AvailableProject
        //{
        //    get;
        //    set;
        //}

        //public TiaPortalMode TiaPortalMode
        //{
        //    get => _tiaPortalMode;
        //    set
        //    {
        //        if (value != _tiaPortalMode)
        //        {
        //            _tiaPortalMode = value;
        //            //NotifyPropertyChanged();
        //        }
        //    }
        //}

        //public Device Device
        //{
        //    get;
        //    set;
        //}

        //#endregion // properties

        #region fields
        private ApiWrapper _apiWrapper;
        //private readonly ApiWrapper _apiWrapper;
        private readonly ProjectGeneratorService _projectGeneratorService;
        private readonly TraceWriter _traceWriter;
        private XmlWrapper _xmlWrapper;
        #endregion

        //public int Licznik { get; private set; }

        private ObservableCollection<Produkt> ListaProduktow = null;

        private IList<TiaPortalProcess> _tiaPortalProcessList;

        // zakładka Processes
        private ObservableCollection<TiaPortalProcessId> _processesIdObservableCollection = null;

        public MainWindow()
        {
            InitializeComponent();

            _traceWriter = new TraceWriter(lBoxTraceWriter);
            _apiWrapper = new ApiWrapper(_traceWriter);
            _projectGeneratorService = new ProjectGeneratorService(_traceWriter, _apiWrapper);
            _xmlWrapper = new XmlWrapper();
            _apiWrapper.Licznik = 0;
            this.DataContext = _apiWrapper;     // tak też działa :) ale można też tak jak poniżej robić przypisanie do DataContext
            //dla poszczególnych zmiennych jak przy pierwszych próbach poniżej
            //tBoxLicznik.DataContext = _apiWrapper;
            //tBoxZapis.DataContext = _apiWrapper; // bez tej linii nie zadziała BINDING Text="{Binding iValue}" 
            // czyli tak na chłopski rozum trzeba wskazać klasę w której znajduje się iValue żeby binding w XAML zadziałał
            tabItemXml.DataContext = _xmlWrapper;

            PrzygotujWiazanie(); // fragment przekopiowany z książki
            // zakładka Processes
            _processesIdObservableCollection = new ObservableCollection<TiaPortalProcessId>();
            listProcesses.ItemsSource = _processesIdObservableCollection;
            _processesIdObservableCollection.Add(new TiaPortalProcessId(1, "A")); //inicjalizacja do testów
            _processesIdObservableCollection.Add(new TiaPortalProcessId(2, "b")); //inicjalizacja do testów
            _processesIdObservableCollection.Add(new TiaPortalProcessId(3, "C")); //inicjalizacja do testów

        }

        private void PrzygotujWiazanie()
        {
            ListaProduktow = new ObservableCollection<Produkt>();
            ListaProduktow.Add(new Produkt("O1-11", "ołówek", 8, "Katowice 1"));
            ListaProduktow.Add(new Produkt("PW-20", "pióro wieczne", 75, "Katowice 2"));
            ListaProduktow.Add(new Produkt("DZ-10", "długopis żelowy", 1121,
            "Katowice 1"));
            ListaProduktow.Add(new Produkt("DZ-12", "długopis kulkowy", 280,
            "Katowice 2"));
            lstProdukty.ItemsSource = ListaProduktow;

            CollectionView widok = (CollectionView)CollectionViewSource.GetDefaultView(lstProdukty.ItemsSource);
            widok.SortDescriptions.Add(new SortDescription("Magazyn", ListSortDirection.Ascending));
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            etykietka.Content = _apiWrapper.Licznik++;
        }

        //private void OpenTiaPortal(object sender, RoutedEventArgs e)
        //{

        //    TiaPortal = new TiaPortal(TiaPortalMode.WithUserInterface);
        //    TiaPortalIsDisposed = false;
        //    CurrentTiaPortalProcess = TiaPortal?.GetCurrentProcess();
        //    tblProcesID.Text = $"{ CurrentTiaPortalProcess.Id}";
        //    _tiaPortalProcessList = new List<TiaPortalProcess>();


        //}


        private void btn_GetCurrentProcesses_Click(object sender, RoutedEventArgs e)
        {
            IList<string> processIds = _apiWrapper_test.DoGetTiaPortalProcesses();

            string processIdsString = string.Join("\n", processIds);
            tblCurrentProcesses.Text = processIdsString;



        }

        private void btn_OpenTia_Click(object sender, RoutedEventArgs e)
        {
            _projectGeneratorService.OpenTiaPortal();
        }

        private void btnTiaGuiModeSubmit_Click(object sender, RoutedEventArgs e)
        {
            //if (TiaWithGui.IsChecked == true)
            //{
            //    _apiWrapper.TiaPortalMode = TiaPortalMode.WithUserInterface;
            //}
            //else if(TiaWithoutGui.IsChecked == true)
            //{
            //    _apiWrapper.TiaPortalMode = TiaPortalMode.WithoutUserInterface;
            //}
            //else _apiWrapper.TiaPortalMode = TiaPortalMode.WithUserInterface;

            tBoxOdczyt.Text = _apiWrapper.iValue;

        }

        private void btnProjectsOpnNewTiaClick(object sender, RoutedEventArgs e)
        {
            _apiWrapper.DoOpenTiaPortal();
            _apiWrapper.DoGetTiaPortalProcesses();
            Console.WriteLine($"Number of processes: {_apiWrapper.ProcessList.Count}");
            if (_apiWrapper.ProcessList.Count > 0)
            {
                _processesIdObservableCollection.Clear();
                foreach (var itemProcess in _apiWrapper.ProcessList)
                {
                    var mode = itemProcess.Mode == TiaPortalMode.WithoutUserInterface ? " Without UI" : " With UI";
                    _processesIdObservableCollection.Add(new TiaPortalProcessId(itemProcess.Id, mode));

                    //if (_processesIdObservableCollection.Any(item => item.ProcessId == itemProcess.Id))
                    //    ;
                    //else
                    //{
                    //    var mode = itemProcess.Mode == TiaPortalMode.WithoutUserInterface ? " Without UI" : " With UI";
                    //    _processesIdObservableCollection.Add(new TiaPortalProcessId(itemProcess.Id, mode));
                    //}
                }
            }

            if (_apiWrapper.TiaPortal != null)
            {
                Console.WriteLine($"{this.ToString()} -- TiaPortal not null -- {_apiWrapper.DoGetCurrentTiaProcessId()} ");
                textBoxProjectCurrentProcess.Text = _apiWrapper.DoGetCurrentTiaProcessId();
            }
            else
            {
                Console.WriteLine($"{this.ToString()} -- TiaPortal is NULL ");
                textBoxProjectCurrentProcess.Text = "opening failed";
            }



        }

        private void btnProjectsConnectTiaInstanceClick(object sender, RoutedEventArgs e)
        {
            int index = listProcesses.SelectedIndex;
            TiaPortalProcessId tiaPortalProcessId = listProcesses.SelectedItem as TiaPortalProcessId;
            if (tiaPortalProcessId != null)
            {
                Console.WriteLine($"OBJECT: listProcesses, Selected index: {index}, Process id: {tiaPortalProcessId.ProcessId}");
                _apiWrapper.DoConnectTiaPortal(tiaPortalProcessId.ProcessId);
                _apiWrapper.DoLoadProject();
                _apiWrapper.DoGetTiaPortalProcesses();
                //_apiWrapper.DoGetCurrentTiaProcessId(); // tylko zwraca CurrentTiaPortalProcess - gdzieś inndziej to pole jest ustawiane
                //textBoxProjectCurrentProcess.Text = _apiWrapper.CurrentTiaPortalProcess != null ? _apiWrapper.CurrentTiaPortalProcess.Id.ToString() : "null";
                Console.WriteLine($"Przycisk: Connect TIA instance => sprawdzenie jak zmienia się CurrentTiaPortalProcess = {_apiWrapper.CurrentTiaPortalProcess?.Id.ToString()}");
            }
            if (_apiWrapper.TiaPortal != null)
            {
                _apiWrapper.NrOfProjects = _apiWrapper.TiaPortal.Projects.Count;
                Console.WriteLine($"NrOfProjcets: {_apiWrapper.TiaPortal.Projects.Count}");
                //ta linia ciągle wywala błąd !!!
                //textBoxProjectsDefaultProjectName.Text = _apiWrapper.TiaPortal.Projects?.FirstOrDefault().ToString();

            }


        }

        private void btnProjectsOpenProjectClick(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Title = "Select project";
            bool? result = openFileDialog.ShowDialog();
            if (result == true)
            {
                Console.WriteLine($"Wybrany plik: {openFileDialog.FileName}");
                _apiWrapper.DoOpenProject(openFileDialog.FileName);
            }

            //od tego jutro zacząć
            //_apiWrapper.TiaPortal.Projects.FirstOrDefault().Name; //
            // sprawdzić ilość projektów
            _apiWrapper.NrOfProjects = _apiWrapper.TiaPortal != null ? _apiWrapper.TiaPortal.Projects.Count : 0;



        }

        private void btnProjectRefreshClick(object sender, RoutedEventArgs e)
        {
            _apiWrapper.DoGetTiaPortalProcesses();
            if (_apiWrapper.ProcessList.Count > 0)
            {
                _processesIdObservableCollection.Clear();
                foreach (var itemProcess in _apiWrapper.ProcessList)
                {
                    var mode = itemProcess.Mode == TiaPortalMode.WithoutUserInterface ? " Without UI" : " With UI";
                    _processesIdObservableCollection.Add(new TiaPortalProcessId(itemProcess.Id, mode));
                }
            }
            if (_apiWrapper.CurrentProject != null)
            {
                Console.WriteLine($"CurrentProject.Path: {_apiWrapper.CurrentProject.Path}");
            }
            else
                Console.WriteLine($"CurrentProject.Path: NULL");

        }


        private void btnProjcetCloseTiaPortalClick(object sender, RoutedEventArgs e)
        {
            _apiWrapper.DoCloseTiaPortal();

        }

        private void btnProjectsCloseProjectClick(object sender, RoutedEventArgs e)
        {
            _apiWrapper.DoCloseProject();
        }

        //private void btnProjectsTest1Click(object sender, RoutedEventArgs e)
        //{
        //    Tests.DisplayCompositionInfos(_apiWrapper.TiaPortal.Projects.FirstOrDefault());

        //}

        private void btnProjectsTreeViewClick(object sender, RoutedEventArgs e)
        {

            TreeNode rootNode = new TreeNode("TIA PORTAL");

            // Dodaj kolejne węzły zgodnie z hierarchią Twojej struktury danych
            foreach (var project in _apiWrapper.TiaPortal.Projects)
            {
                var projectNode = new TreeNode(project.Name);
                rootNode.Children.Add(projectNode);

                SoftwareContainer softwareContainerFromProject = ((IEngineeringServiceProvider)project).GetService<SoftwareContainer>();
                //SoftwareContainer softwareContainer1 = ((IEngineeringServiceProvider)deviceItem).GetService<SoftwareContainer>();
                if (softwareContainerFromProject != null)
                {
                    Console.WriteLine("TREE - softwareContainerFromProject NOT null");
                }
                else { Console.WriteLine("TREE - softwareContainerFromProject == NULL :("); }

                foreach (var device in project.Devices)
                {
                    var deviceNode = new TreeNode(device.Name);
                    projectNode.Children.Add(deviceNode);

                    SoftwareContainer softwareContainer = ((IEngineeringServiceProvider)device).GetService<SoftwareContainer>();
                    if (softwareContainer != null)
                    {
                        PlcSoftware software = softwareContainer.Software as PlcSoftware;



                        foreach (var programBlock in software.BlockGroup.Blocks)
                        {
                            var programBlockNode = new TreeNode(programBlock.Name);
                            deviceNode.Children.Add(programBlockNode);

                            //foreach(var deviceContainer in deviceItem.Container.DeviceItems)
                            //{
                            //    var deviceContainerNode = new TreeNode(deviceContainer.Name);
                            //    deviceItemNode.Children.Add(deviceContainerNode);
                            //}

                        }


                    }
                    else
                    {
                        Console.WriteLine("TREE - softwareContainer == NULL :(");
                    }
                    foreach (var deviceItem in device.DeviceItems)
                    {
                        var deviceItemNode = new TreeNode(deviceItem.Name);
                        deviceNode.Children.Add(deviceItemNode);
                        //szukanie software :)
                        Siemens.Engineering.HW.DeviceItem deviceItemToGetService = deviceItem as Siemens.Engineering.HW.DeviceItem;
                        SoftwareContainer softwareContainer1 = deviceItemToGetService.GetService<SoftwareContainer>();
                        //SoftwareContainer softwareContainer1 = ((IEngineeringServiceProvider)deviceItem).GetService<SoftwareContainer>();
                        if (softwareContainer1 != null)
                        {
                            PlcSoftware software = softwareContainer1.Software as PlcSoftware;
                            if (software != null)
                            {
                                _apiWrapper.PlcSoftwares.Add(software);
                            }
                            Console.WriteLine($"TREE - softwareContainer1 NOT null :) {deviceItem.Name} % {software.BlockGroup.Name}");
                            //PlcBlock codeBlock = ((IEngineeringServiceProvider)software.BlockGroup.).GetService<CodeBlock>();
                            foreach (var programBlock in software.BlockGroup.Blocks)
                            {
                                TreeNode programBlockNode;
                                if (programBlock is PlcBlock)
                                {
                                    string blockType = programBlock.GetType().ToString();
                                    blockType = blockType.Substring(blockType.LastIndexOf(".") + 1);
                                    programBlockNode = new TreeNode(programBlock.Name, programBlock.Number.ToString(), blockType);
                                }
                                else
                                    { programBlockNode = new TreeNode(programBlock.Name); }
                                deviceItemNode.Children.Add(programBlockNode);

                            }
                            foreach (var blockGroup in software.BlockGroup.Groups)
                            {
                                var blockGrupNode = new TreeNode(blockGroup.Name);
                                deviceItemNode.Children.Add(blockGrupNode);

                                foreach (var programBlock in blockGroup.Blocks)
                                {
                                    string blockType = programBlock.GetType().ToString();
                                    blockType = blockType.Substring(blockType.LastIndexOf(".") + 1);
                                    var programBlockNode = new TreeNode(programBlock.Name, programBlock.Number.ToString(), blockType);
                                    blockGrupNode.Children.Add(programBlockNode);
                                }
                            }

                            foreach (var blockGroupTest in software.BlockGroup.Groups)
                            {
                                Console.WriteLine(blockGroupTest.Name);
                            }

                        }
                        else { Console.WriteLine("TREE - softwareContainer1 == NULL :("); }
                    }

                }
            }
            treeView.Items.Clear();
            treeView.Items.Add(rootNode);
            //PlcSoftware plcSoftware = _apiWrapper.TiaPortal.Projects[0].Devices.PlcDevices[0].PlcSoftware;
        }

        private void treeViewProjectSelected(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            bool blockNotFound = false;
            _apiWrapper.ProjectTreeLeaf = (TreeNode)e.NewValue;
            Console.WriteLine($"TREE VIEW: Selected leaf = {_apiWrapper.ProjectTreeLeaf?.Name}");
            if (_apiWrapper.ProjectTreeLeaf != null && _apiWrapper.ProjectTreeLeaf.Name != String.Empty)
            {       //!!!!! odniesienie do elementu 0 listy - może być więcej niż jedno urządzenie zawierające PlcSoftware.
                    //!!!!! trzeba to poprawić żeby było bardziej uniwersalnie
                if (_apiWrapper.PlcSoftwares[0].BlockGroup.Blocks.Find(_apiWrapper.ProjectTreeLeaf.Name) != null)
                {
                    // check blocks in main branch 
                    _apiWrapper.ProjectSelectedPlcBlock = _apiWrapper.PlcSoftwares[0].BlockGroup.Blocks.Find(_apiWrapper.ProjectTreeLeaf.Name);

                    Console.WriteLine($"TREE VIEW: SELECTED BLOCK = {_apiWrapper.ProjectSelectedPlcBlock.Name}");
                    // teraz trzeba w xaml zrobić wyświetlenie właściwości bloku: nazwa, typ, data itd
                    // poźniej przyciski export i import
                }
                else
                    blockNotFound = true;
                foreach (var group in _apiWrapper.PlcSoftwares[0].BlockGroup.Groups)
                {
                    // check blocks inside folders
                    if(group.Blocks.Find(_apiWrapper.ProjectTreeLeaf.Name) != null)
                    {
                        _apiWrapper.ProjectSelectedPlcBlock = group.Blocks.Find(_apiWrapper.ProjectTreeLeaf.Name);
                        break;
                    }
                    blockNotFound = true;
                }
                if (blockNotFound)
                {
                    Console.WriteLine($"TREE VIEW: Not found block #{_apiWrapper.ProjectTreeLeaf.Name}");
                }

            }
        }

        private void btnProjectsExportFolderClikc(object sender, RoutedEventArgs e)
        {
            //ExportFolder

            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

            // Ustaw opcje wyboru folderu
            folderBrowserDialog.Description = "Select folder";
            folderBrowserDialog.RootFolder = Environment.SpecialFolder.Desktop;

            // Wyświetl okno dialogowe i sprawdź czy użytkownik wybrał folder
            DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                // Pobierz ścieżkę wybranego folderu
                _apiWrapper.ExportFolder = folderBrowserDialog.SelectedPath;

                // Wyświetl wybraną ścieżkę
                Console.WriteLine("Selected folder: " + _apiWrapper.ExportFolder);
            }
            else
            {
                Console.WriteLine("Anulowano wybór folderu.");
            }


        }

        private void btnProjectsExportBlockClick(object sender, RoutedEventArgs e)
        {
            if (_apiWrapper.ExportFolder != null) 
            {
                System.IO.FileInfo newFile = new System.IO.FileInfo(_apiWrapper.ExportFolder+@"\"+_apiWrapper.ProjectSelectedPlcBlock.Name+@".xml");
                _apiWrapper.ProjectSelectedPlcBlock.Export(newFile , ExportOptions.None);
                // gdy eksportowany plik już istnieje (albo tylko gdy istnieje i jest otwarty w jakimś edytorze - to muszę potwierdzić)
                // to pojawia się wyjątke który trzeba obsłużyć. 
                // Obsługa wyjątku ma polegać na wyświetleniu okienka z pytaniem czy nadpisać istniejący
                //  - nadpisanie prawdopodobnie się nie uda bo plik jest już otwarty w innym programie
                //  - wyświtlić komunikat, że nadpisanie jest niemożliwe. 
                //  - jeśli będzie możliwe, to usunąć plik i spróbować ponownie exportować
            }

        }

        private void btnProjectsImportBlockClick(object sender, RoutedEventArgs e)
        {
            string blockNames = "";
            IList<PlcBlock> blocks = _apiWrapper.PlcSoftwares[0].BlockGroup.Blocks.Import(new System.IO.FileInfo(_apiWrapper.ImportFile), ImportOptions.Override);
            foreach(var block in blocks)
            {
                blockNames += blockNames + ": "+ block.Name;
            }
            Console.WriteLine($"IMPORTED FILES LIST: {blockNames}");

        }

        private void btnProjectsImportSelectFileClikc(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Title = "Select file to import";
            bool? result = openFileDialog.ShowDialog();
            if (result == true)
            {
                Console.WriteLine($"IMPORT FILE: {openFileDialog.FileName}");
                _apiWrapper.ImportFile = openFileDialog.FileName;
            }

        }

        private void btnXmlGenerateInterfaceClick(object sender, RoutedEventArgs e)
        {
            //XElement section = new XElement("Interface", new XAttribute("Atrybut", "hasiok"));
            //section.Add(new XElement("Sections", new XAttribute("xmlns", "http://www.siemens.com/automation/Openness/SW/Interface/v5")));
            //_xmlWrapper.GeneratedXml = section.ToString();

            XNamespace ns = "http://www.siemens.com/automation/Openness/SW/Interface/v5";
            XElement section = new XElement("Interface", new XAttribute("Atrybut", "hasiok"));
            section.Add(new XElement(ns + "Sections"));
            //section.Elements("Section").Where - do nauki
            

            //XNamespace ns = "http://www.siemens.com/automation/Openness/SW/Interface/v5";
            //XElement section = new XElement("Interface",
            //    new XAttribute(XNamespace.Xmlns + "customPrefix", ns.NamespaceName),
            //    new XAttribute("Atrybut", "hasiok"));
            //section.Add(new XElement(ns + "Sections"));

            _xmlWrapper.GeneratedXml = section.ToString();
            //textBlockXmlGeneratedXml.Text = _xmlWrapper.GeneratedXml;
            Console.WriteLine($"XML GENERATOR: {_xmlWrapper.GeneratedXml}");
        }
    }
}
