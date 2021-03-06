﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Watchables.WebAPI.Database;
using Watchables.WebAPI.Exceptions;

namespace Watchables.WebAPI.Services
{
    public class ShowsService : IShowsService
    {
        //Dependency injection
        private readonly _160304Context _context;
        private readonly IMapper _mapper;
        public ShowsService(_160304Context context, IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }

        public List<Model.Show> Get(Model.Requests.ShowSearchRequest request) {

            var query = _context.Shows.AsQueryable();
            if (!string.IsNullOrWhiteSpace(request.Title)) query = query.Where(m => m.Title.ToLower().StartsWith(request.Title.ToLower()));
            if (!string.IsNullOrWhiteSpace(request.Genre)) query = query.Where(m => m.Genre.ToLower().StartsWith(request.Genre.ToLower()));
            if (request.Year >= 0) query = query.Where(m => m.Year >= request.Year);
            if (request.Rating >= 0) query = query.Where(m => m.Rating >= request.Rating);
            if (!string.IsNullOrWhiteSpace(request.Ongoing) && request.Ongoing == "true") query = query.Where(m => m.Ongoing);
            if (!string.IsNullOrWhiteSpace(request.Ongoing) && request.Ongoing == "false") query = query.Where(m => !m.Ongoing);

            return _mapper.Map<List<Model.Show>>(query.ToList());
        }


        public Model.Show GetById(int id) {
            return _mapper.Map<Model.Show>(_context.Shows.Find(id));
        }

        public Model.Show Insert(Model.Requests.InsertShowRequest request) {

            var show = _mapper.Map<Database.Shows>(request);
            _context.Shows.Add(show);
            _context.SaveChanges();

            return _mapper.Map<Model.Show>(_mapper.Map<Database.Shows>(request));
        }

        public Model.Show Update(int showId, Model.Requests.InsertShowRequest request) {
            var show = _context.Shows.Find(showId);
            _mapper.Map(request, show);
            _context.SaveChanges();
            return _mapper.Map<Model.Show>(show);
        }

        public string Delete(int id) {

            Helper helper = new Helper(_context);

            var validShow = false;
            foreach (var s in _context.Shows.ToList()) {
                if (s.ShowId== id) {
                    validShow= true;
                    break;
                }
            }
            if (!validShow) throw new UserException("Cannot find a show with the specified id");

            var show = _context.Shows.Find(id);

            string notificationContent = $"The show '{show.Title}', has been removed";

            helper.DeleteShowNotification(show, notificationContent, "Removal");      

            return "Show removed";
        }

    }
}
