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
    public class AlbumsController : ApiController
    {

        public HttpResponseMessage Get(string artistId)
        {
            HttpResponseMessage responseMessage;
            using (var context = new ChinookEntities())
            {
                int id = Int32.Parse(artistId);
                var albums = context.Albums.Where(a => a.ArtistId == id).AsEnumerable();
                var albumsDTO = new List<AlbumDTO>();
                if (albums != null)
                {
                    foreach (var album in albums)
                    {
                        albumsDTO.Add(new AlbumDTO { AlbumId=album.AlbumId, Title=album.Title });
                    }
                    responseMessage = Request.CreateResponse<List<AlbumDTO>>(HttpStatusCode.OK, albumsDTO);
                }
                else
                {
                    responseMessage = Request.CreateErrorResponse(HttpStatusCode.NotFound, new HttpError("No albums found!"));
                }


            }
            return responseMessage;
        }
    }
}