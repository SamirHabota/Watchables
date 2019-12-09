﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Watchables.WinUI.Forms.User
{
    public partial class UsersForm : Form
    {

        private readonly APIService _aspiService = new APIService("users");

        public UsersForm() {
            InitializeComponent();
            dgvUsers.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(178, 8, 55);
            dgvUsers.EnableHeadersVisualStyles = false;
        }

        private async void UsersForm_Load(object sender, EventArgs e) {
            var users = await _aspiService.Get<List<Model.User>>(null);
            dgvUsers.AutoGenerateColumns = false;
            var list = new List<Model.dgvLists.UserItem>();
            foreach(var user in users) {
                var Object = new Model.dgvLists.UserItem() {
                    Address = user.Address,
                    BirthDate = user.BirthDate.Date,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Mail = user.Mail,
                    PhoneNumber = user.PhoneNumber,
                    UserId = user.UserId,
                    Username = user.Account.Username,
                    Active=user.Active
                };
                list.Add(Object);
            }
            list.Sort((a, b) => a.FirstName.CompareTo(b.FirstName));
            dgvUsers.DataSource = list;
        }

        private async void searchBtn_Click(object sender, EventArgs e) {
            Model.Requests.UserSearchRequest search = new Model.Requests.UserSearchRequest() {
                FirstName = FNSrch.Text,
                LastName=LNSrch.Text, 
                Userneme=UNSrch.Text
            };
            var users = await _aspiService.Get<List<Model.User>>(search);
            var list = new List<Model.dgvLists.UserItem>();
            foreach (var user in users) {
                var Object = new Model.dgvLists.UserItem() {
                    Address = user.Address,
                    BirthDate = user.BirthDate.Date,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Mail = user.Mail,
                    PhoneNumber = user.PhoneNumber,
                    UserId = user.UserId,
                    Username = user.Account.Username
                };
                list.Add(Object);
            }
            list.Sort((a, b) => a.FirstName.CompareTo(b.FirstName));
            dgvUsers.DataSource = list;
        }

        private async void clearSearch_Click(object sender, EventArgs e) {
            var users = await _aspiService.Get<List<Model.User>>(null);
            dgvUsers.AutoGenerateColumns = false;
            var list = new List<Model.dgvLists.UserItem>();
            foreach (var user in users) {
                var Object = new Model.dgvLists.UserItem() {
                    Address = user.Address,
                    BirthDate = user.BirthDate.Date,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Mail = user.Mail,
                    PhoneNumber = user.PhoneNumber,
                    UserId = user.UserId,
                    Username = user.Account.Username
                };
                list.Add(Object);
            }
            list.Sort((a, b) => a.FirstName.CompareTo(b.FirstName));
            dgvUsers.DataSource = list;
            FNSrch.Text = "";
            LNSrch.Text = "";
            UNSrch.Text = "";
        }
    }
}
