using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiKnockout.Model;
using WebApiKnockout.Model.DTO;

namespace WebApiKnockout.Controllers
{
    public class GenresController : ApiController
    {
        // GET api/<controller>
        public HttpResponseMessage Get()
        {
            HttpResponseMessage responseMessage;
            using (var context = new ChinookEntities())
            {
                var genres = context.Genres.AsEnumerable();
                var genresDTO = new List<GenreDTO>();
                foreach (var genre in genres)
                {
                    genresDTO.Add(new GenreDTO { GenreId = genre.GenreId, Name = genre.Name });
                }
                responseMessage = Request.CreateResponse<IEnumerable<GenreDTO>>(HttpStatusCode.OK, genresDTO);

            }
            return responseMessage;
        }
    }
}