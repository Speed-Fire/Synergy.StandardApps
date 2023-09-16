﻿using Synergy.StandardApps.Notes.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Synergy.StandardApps.Notes.SubPages
{
    /// <summary>
    /// Логика взаимодействия для NoteViewPage.xaml
    /// </summary>
    public partial class NoteViewPage : Page
    {
        public NoteViewPage()
        {
            InitializeComponent();

            DataContext = new NoteViewVM();
        }
    }
}