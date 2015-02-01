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
    public class ArtistsController : ApiController
    {
        public HttpResponseMessage Get(string name)
        {
            HttpResponseMessage responseMessage;
            using (var context = new ChinookEntities())
            {
                var artists = context.Artists.Where(a => a.Name.Contains(name)).AsEnumerable();
                var artistsDTO = new List<ArtistDTO>();
                if (artists != null)
                {
                    foreach (var artist in artists)
                    {
                        artistsDTO.Add(new ArtistDTO { ArtistId = artist.ArtistId, Name = artist.Name });
                    }
                    responseMessage = Request.CreateResponse<List<ArtistDTO>>(HttpStatusCode.OK, artistsDTO);
                }
                else
                {
                    responseMessage = Request.CreateErrorResponse(HttpStatusCode.NotFound, new HttpError("Artist not found!"));
                }


            }
            return responseMessage;
        }
    }
}