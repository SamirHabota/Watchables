﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Watchables.Model.Requests;
using Flurl.Http;
using Flurl;

namespace Watchables.WinUI.Forms.Cinema
{
    public partial class AddEditCinemaForm : Form
    {

        private int? _cinemaId = null;
        private readonly APIService _apiService = new APIService("cinemas");
        private MenuForm _menuForm;

        public AddEditCinemaForm(MenuForm menuForm, int? cinemaId = null) {
            InitializeComponent();
            _cinemaId = cinemaId;
            _menuForm = menuForm;
        }

        private void Close_Click(object sender, EventArgs e) {
            this.Close();
        }

        private async void AddEditCinemaForm_Load(object sender, EventArgs e) {
            if (_cinemaId.HasValue) {
                Model.Cinema cinema = await _apiService.GetById<Model.Cinema>(_cinemaId);
                Cinema.Text = cinema.Name;
                Address.Text = cinema.Address;
                StreetNumber.Value = cinema.StreetNumber;
                Location.Text = cinema.Location;
                PhoneNumber.Text = cinema.PhoneNumber;
                Rating.Text = cinema.Rating.ToString();
                Description.Text = cinema.Description;
                ImageLink.Text = cinema.ImageLink;
            }
            
        }

        private async void SaveBtn_Click(object sender, EventArgs e) {

            var messageBox = new CustomMessageBox();

            if (string.IsNullOrWhiteSpace(Cinema.Text) || Cinema.Text.Length<=4) {
                messageBox.Show("The cinema field requires 4 letters!", "error");
                return;
            }

            if (string.IsNullOrWhiteSpace(Address.Text) || Address.Text.Length <= 5) {
              
                return;
            }

            if (StreetNumber.Value<=0 || StreetNumber.Value >100) {
              
                return;
            }


            InsertCinemaRequest request = new InsertCinemaRequest() {
            Name=Cinema.Text,
            Address=Address.Text,
            StreetNumber = (int)StreetNumber.Value,
            Location = Location.Text,
            PhoneNumber = PhoneNumber.Text,
            Rating = decimal.Parse(Rating.Text),
            Description = Description.Text,
            ImageLink = ImageLink.Text
            };

            if (_cinemaId.HasValue) {
                await _apiService.Update<Model.Cinema>(_cinemaId, request);                
            }
            else {
                await _apiService.Insert<Model.Cinema>(request);               
            }
            this.Close();
            foreach (Form frm in _menuForm.MdiChildren) {
                frm.Close();
            }
            CinemasForm form = new CinemasForm {
                MdiParent = _menuForm,
                Dock = DockStyle.Fill
            };           
            form.Show();
            if (_cinemaId.HasValue) {
                messageBox.Show("Cinema updated successfully!", "success");
            }
            else {
                messageBox.Show("Cinema added successfully!", "success");
            }
        }       
    }
}