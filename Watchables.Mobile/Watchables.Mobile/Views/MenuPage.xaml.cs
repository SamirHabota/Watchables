﻿using Watchables.Mobile.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Watchables.Mobile.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MenuPage : ContentPage
    {
        MainPage RootPage { get => Application.Current.MainPage as MainPage; }
        List<HomeMenuItem> menuItems;
        public MenuPage() {
            InitializeComponent();           

            menuItems = new List<HomeMenuItem>
            {
                new HomeMenuItem {Id = MenuItemType.Cinemas, Title="Cinemas"},
                new HomeMenuItem {Id=MenuItemType.Movies, Title="Movies"},
                new HomeMenuItem {Id=MenuItemType.Shows, Title="Shows"},
                new HomeMenuItem {Id=MenuItemType.Subscriptions, Title="Subscriptions"},
                new HomeMenuItem {Id = MenuItemType.Rotations, Title="Rotations" }
               
            };

            ListViewMenu.ItemsSource = menuItems;

            ListViewMenu.ItemSelected += async (sender, e) => {
                if (e.SelectedItem == null)
                    return;

                var id = (int)((HomeMenuItem)e.SelectedItem).Id;
                await RootPage.NavigateFromMenu(id);
            };
        
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e) {
            ListViewMenu.SelectedItem = new HomeMenuItem { Id = MenuItemType.Browse, Title = "Browse" };
        }

        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e) {
            ListViewMenu.SelectedItem = new HomeMenuItem { Id = MenuItemType.Browse, Title = "Browse" };
        }
    }
}