﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Watchables.WebAPI.Database;
using Watchables.WebAPI.Exceptions;

namespace Watchables.WebAPI.Services
{
    public class InitializeService : IInitializeService
    {
        //Dependency injection
        private readonly _160304Context _context;
        public InitializeService (_160304Context context) {
            _context = context;
        }

        public string AddDays() {

            if (!_context.AiringDays.Any()) {
                var monday = new AiringDays() {
                    Name = "Monday",
                    StartsAt = "09:00",
                    EndsAt = "23:30"
                };
                var tuesday = new AiringDays() {
                    Name = "Tuesday",
                    StartsAt = "09:00",
                    EndsAt = "23:30"
                };
                var wednesday = new AiringDays() {
                    Name = "Wednesday",
                    StartsAt = "09:00",
                    EndsAt = "23:30"
                };
                var thursday = new AiringDays() {
                    Name = "Thursday",
                    StartsAt = "09:00",
                    EndsAt = "23:30"
                };
                var friday = new AiringDays() {
                    Name = "Firday",
                    StartsAt = "09:00",
                    EndsAt = "23:30"
                };
                var saturday = new AiringDays() {
                    Name = "Saturday",
                    StartsAt = "09:00",
                    EndsAt = "00:00"
                };
                var sunday = new AiringDays() {
                    Name = "Sunday",
                    StartsAt = "11:00",
                    EndsAt = "23:00"
                };
                _context.AiringDays.AddRange(monday, tuesday, wednesday, thursday, friday, saturday, sunday);
                _context.SaveChanges();
                return "Initialized";
            }

            else throw new UserException("The database is already initialized with airing days!");
        }

        public string AddMovies() {
            if (!_context.Movies.Any()) {

                var movie1 = new Database.Movies() {
                    Title = "Spider-Man: Far from Home",
                    Year = 2019,
                    Duration = "2:49",
                    Rating = (decimal)7.6,
                    Description = "Following the events of Avengers: Endgame (2019), Spider-Man must step up to take on new threats in a world that has changed forever.",
                    Cast = "Tom Holland, Samuel L. Jackson, Jake Gyllenhaal",
                    TrailerLink = "https://www.youtube.com/embed/Nt9L1jCKGnE",
                    ImageLink = "https://m.media-amazon.com/images/M/MV5BMGZlNTY1ZWUtYTMzNC00ZjUyLWE0MjQtMTMxN2E3ODYxMWVmXkEyXkFqcGdeQXVyMDM2NDM2MQ@@._V1_SY1000_CR0,0,674,1000_AL_.jpg",
                    Standalone = false,
                    Price = (decimal)20.00
                };

                var movie2 = new Database.Movies() {
                    Title = "Joker",
                    Year = 2019,
                    Duration = "2:25",
                    Rating = (decimal)8.8,
                    Description= "In Gotham City, mentally-troubled comedian Arthur Fleck is disregarded and mistreated by society. He then embarks on a downward spiral of revolution and bloody crime. This path brings him face-to-face with his alter-ego: 'The Joker'.",
                    Cast= " Joaquin Phoenix, Robert De Niro, Zazie Beetz",
                    TrailerLink= "https://www.youtube.com/embed/zAGVQLHvwOY",
                    ImageLink= "https://m.media-amazon.com/images/M/MV5BNGVjNWI4ZGUtNzE0MS00YTJmLWE0ZDctN2ZiYTk2YmI3NTYyXkEyXkFqcGdeQXVyMTkxNjUyNQ@@._V1_SY1000_CR0,0,674,1000_AL_.jpg",
                    Standalone=false,
                    Price = (decimal)20.00
                };

                _context.Movies.AddRange(movie1, movie2);
                _context.SaveChanges();


                return "Initialized";
            }
            else throw new UserException("The database is already initialized with movies!");
        }
    }
}