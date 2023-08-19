
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
        #endregion

        //public int Licznik { get; private set; }

        private IList<TiaPortalProcess> _tiaPortalProcessList;
        public MainWindow()
        {
            InitializeComponent();

            _traceWriter = new TraceWriter(lBoxTraceWriter);
            _apiWrapper = new ApiWrapper(_traceWriter);
            _projectGeneratorService = new ProjectGeneratorService(_traceWriter, _apiWrapper);
            _apiWrapper.Licznik = 0;
            this.DataContext = _apiWrapper;     // tak też działa :) ale można też tak jak poniżej robić przypisanie do DataContext
            //dla poszczególnych zmiennych jak przy pierwszych próbach poniżej
            //tBoxLicznik.DataContext = _apiWrapper;
            //tBoxZapis.DataContext = _apiWrapper; // bez tej linii nie zadziała BINDING Text="{Binding iValue}" 
            // czyli tak na chłopski rozum trzeba wskazać klasę w której znajduje się iValue żeby binding w XAML zadziałał
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
    }
}
