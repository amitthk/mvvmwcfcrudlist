﻿using MvvmWcfCrudList.Common.Messaging;
using MvvmWcfCrudList.Views;
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

namespace MvvmWcfCrudList
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IEventAggregator _eventAggregator;

        public MainWindow()
        {
            _eventAggregator = App.eventAggregator;
            InitializeComponent();
            _MainFrame.NavigationService.Navigate(new Home());
            _eventAggregator.Subscribe<NavMessage>(NavigateToPage);
        }

        private void NavigateToPage(NavMessage message)
        {
            object viewObject = message.ViewObject;
            object navigationState = message.NavigationStateParams;

            if ((viewObject!=null)&&(navigationState!=null))
            {
                _MainFrame.NavigationService.Navigate(viewObject, navigationState);
                return;
            }
            else if (viewObject!=null)
            {
                _MainFrame.NavigationService.Navigate(viewObject);
                return;
            }

            //Silverlight
            string queryStringParams = message.QueryStringParams == null ? "" : GetQueryString(message);
            string uri = string.Format("/Views/{0}.xaml{1}", message.PageName, queryStringParams);
            _MainFrame.NavigationService.Navigate(new Uri(uri, UriKind.Relative));
        }

        private string GetQueryString(NavMessage message)
        {
            string qstr = null;
            if (message.QueryStringParams != null)
            {
                qstr = string.Concat(message.QueryStringParams.Select(x => x.Key + "=" + x.Value).ToList<string>().ToArray());
                qstr = "?" + qstr;
            }
            return (qstr);
        }
    }
}
