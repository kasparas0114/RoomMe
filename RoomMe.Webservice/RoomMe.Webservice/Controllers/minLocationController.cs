﻿using RoomMe.Webservice.Models;
using RoomMe.Webservice.Models.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace RoomMe.Webservice.Controllers
{
    public class minLocationController : ApiController
    {

        private RoomMeWebserviceContext db = new RoomMeWebserviceContext();

        // GET api/minlocation
        public IQueryable<APILocation> Get()
        {
            var models = db.Locations;

            var minModels = new List<APILocation>();

            foreach (var model in models)
            {
                minModels.Add(model.ToAPIModel());
            }

            return minModels as IQueryable<APILocation>;
        }

        // GET api/minlocation/5
        public async Task<IHttpActionResult> Get(int id)
        {
            Location model = await db.Locations.FindAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            return Ok(model.ToAPIModel());
        }
    }
}
