﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Watchables.Mobile.ViewModels
{
    public class MoviesViewModel : BaseViewModel
    {

        public ObservableCollection<Model.Movie> Movies { get; set; } = new ObservableCollection<Model.Movie>();
        public ObservableCollection<string> Genres { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<int> Ratings { get; set; } = new ObservableCollection<int>() {0, 1, 2, 4, 5,6,7,8,9,10 };
        public ICommand ShowMoviesCommand { get; set; }
        public ICommand ForMeButton { get; set; }
        public ICommand AllMoviesButton { get; set; }
        public ICommand SearchCommand { get; set; }
        public APIService _moviesApi = new APIService("movies");
        public APIService _usersApi = new APIService("users");

        string _genre = string.Empty;
        public string Genre {
            get { return _genre; }
            set {
                SetProperty(ref _genre, value);
                if (value != null) {
                    ShowMoviesCommand.Execute(null);
                }
                             
            }
        }

        string _showFilters = string.Empty;
        public string ShowFilters {
            get { return _showFilters; }
            set {
                SetProperty(ref _showFilters, value);
             
            }
        }

        int _rating = 0;
        public int Rating{
            get { return _rating; }
            set {
                SetProperty(ref _rating, value);
                ShowMoviesCommand.Execute(null);
            }
        }

        string _search = string.Empty;
        public string Search {
            get { return _search; }
            set {
                SetProperty(ref _search, value);
                SearchCommand.Execute(null);
            }
        }


        public MoviesViewModel() {
            ShowMoviesCommand = new Command(async () => await ShowMovies());
            AllMoviesButton = new Command(async () => await ShowMovies());
            ForMeButton = new Command(async () => await ForMe());
            SearchCommand = new Command(async () => await OnSearch());
        }

        public async Task OnSearch() {
            if (Search != null) {
                Genre = "All";
                Rating = 0;
                Model.Requests.MovieSearchRequest request = new Model.Requests.MovieSearchRequest() {
                    Title = Search
                };
                var movies = await _moviesApi.Get<List<Model.Movie>>(request);
                Movies.Clear();
                movies.Sort((a, b) => a.Title.CompareTo(b.Title));
                foreach (var movie in movies) {
                    Movies.Add(movie);
                }
            }
        }

        public async Task ForMe() {
            ShowFilters = "False";
            Movies.Clear();
            var movies = await _usersApi.GetItems<List<Model.Movie>>(APIService.User.UserId, "RecommendedMovies");
            foreach(var movie in movies) {
                Movies.Add(movie);
            }
        }

        public async Task ShowMovies() {

            ShowFilters="True";           

            Model.Requests.MovieSearchRequest request = new Model.Requests.MovieSearchRequest();
            if (Genre != null) {
                request.Genre = Genre;
                if (request.Genre == "All") request.Genre = "";
            }

            request.Rating = Rating;

           
            var movies = await _moviesApi.Get<List<Model.Movie>>(request);
            movies.RemoveAll(m => !m.Standalone);
            movies.Sort((a, b) => a.Title.CompareTo(b.Title));

            Movies.Clear();
            foreach (var movie in movies) {
                Movies.Add(movie);
            }
            if (Genres.Count == 0) {
                var list = new List<string>();               
                foreach (var movie in movies) {
                    list.Add(movie.Genre);
                }
                list = list.Distinct().ToList();
                list.Sort();
                list.Insert(0, "All");
                foreach (var genre in list) {
                    Genres.Add(genre);
                }
            }

        }

    }
}
