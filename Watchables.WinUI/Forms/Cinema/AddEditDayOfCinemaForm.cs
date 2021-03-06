﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Watchables.WinUI.Forms.Cinema
{
    public partial class AddEditDayOfCinemaForm : Form
    {
        private readonly ScheduleForm _scheduleForm;
        private readonly int? _airingDayId;
        private readonly APIService _apiService = new APIService("AiringDaysOfCinema");
        private readonly Model.Requests.CinemasScheduleRequest _schedule;
        private readonly Helper _helper = new Helper();
        private readonly MenuForm _menuForm;


        public AddEditDayOfCinemaForm(ScheduleForm scheduleForm, Model.Requests.CinemasScheduleRequest schedule, MenuForm menuForm, int? airingDayId=null) {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(Screen.PrimaryScreen.Bounds.Width - 20 - this.Width, Screen.PrimaryScreen.Bounds.Height - this.Height - saveBtn.Height - 20);
            _scheduleForm = scheduleForm;
            _schedule = schedule;
            _airingDayId = airingDayId;
            _menuForm = menuForm;
        }

        private void closeBtn_Click(object sender, EventArgs e) {
            _helper.CloseForm(this, 15);
        }

        private async void AddEditDayOfCinemaForm_Load(object sender, EventArgs e) {
            datePicker.MinDate = DateTime.Now;
            if (_airingDayId.HasValue) {
                Title.Text = $"Day of: {_schedule.Cinema.Name}";                
                var airingDay = await _apiService.GetById<Model.AiringDaysOfCinema>(_airingDayId);
                if (airingDay.Date.Date == datePicker.MinDate.Date) {
                    datePicker.MinDate = DateTime.Now.AddDays(-1);
                }
                datePicker.Value = airingDay.Date;
                datePicker.MinDate = DateTime.Now;
            }
            else {
                Title.Text = $"Day to: {_schedule.Cinema.Name}";
            }
        }

        private async void reloadBtn_Click(object sender, EventArgs e) {
            if (_airingDayId.HasValue) {
                var daysApi = new APIService("AiringDaysOfCinema");
                var airingDay = await daysApi.GetById<Model.AiringDaysOfCinema>(_airingDayId);
                datePicker.Value = airingDay.Date;
            }
            else {
                datePicker.Value = DateTime.Now;
            }
        }

        private async void saveBtn_Click(object sender, EventArgs e) {
            var daysApi = new APIService("AiringDaysOfCinema");
            var messageBox = new CustomMessageBox();
            var allDays = await daysApi.Get<List<Model.AiringDaysOfCinema>>(null);
            foreach (var day in allDays) {
                if (day.Date.Date == datePicker.Value.Date && day.CinemaId == _schedule.Cinema.CinemaId) {
                    messageBox.Show("Day already generated!", "error");
                    return;
                }
            }
            var Object = new Model.Requests.InserAiringDayOfCinemaRequest() {
                Date = datePicker.Value,
                CinemaId = _schedule.Cinema.CinemaId
            };

            if (_airingDayId.HasValue) {
               
                await _apiService.Update<Model.AiringDaysOfCinema>(_airingDayId, Object);
                messageBox.Show("Airing day updated successfully", "success");
            }
            else {
                await _apiService.Insert<Model.AiringDaysOfCinema>( Object);
                messageBox.Show("Airing day added successfully", "success");
            }
            this.Close();
            _scheduleForm.Close();
            ScheduleForm form = new ScheduleForm(_schedule.Cinema.CinemaId, _menuForm) {
                MdiParent = _menuForm,
                Dock = DockStyle.Fill
            };
            form.Show();
            
        }
    }
}
