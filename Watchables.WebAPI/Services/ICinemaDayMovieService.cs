﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Watchables.WebAPI.Services
{
    public interface ICinemaDayMovieService
    {
        List<Model.CinemaDayMovie> Get();
        Model.CinemaDayMovie GetById(int id);
    }
}