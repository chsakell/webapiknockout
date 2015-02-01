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
    public class TracksController : ApiController
    {

        public HttpResponseMessage Get(string albumId)
        {
            HttpResponseMessage responseMessage;
            using (var context = new ChinookEntities())
            {
                int id = Int32.Parse(albumId);
                List<TrackDTO> tracksDTO = new List<TrackDTO>();
                var tracks = context.Tracks.Where(t => t.AlbumId == id).AsEnumerable();
                if (tracks.Count() > 0)
                {
                    foreach (var track in tracks)
                    {

                        tracksDTO.Add(new TrackDTO
                        {
                            TrackId = track.TrackId,
                            Name = track.Name,
                            Genre = track.Genre.Name,
                            Milliseconds = track.Milliseconds,
                            UnitPrice = track.UnitPrice
                        });
                    }

                    responseMessage = Request.CreateResponse<List<TrackDTO>>(HttpStatusCode.OK, tracksDTO);
                }
                else
                {
                    responseMessage = Request.CreateErrorResponse(HttpStatusCode.NotFound, new HttpError("No tracks found for this album"));
                }
            }

            return responseMessage;
        }


        // PUT api/<controller>/5
        public HttpResponseMessage Put(int id, TrackDTO track)
        {
            HttpResponseMessage responseMessage;
            using (var context = new ChinookEntities())
            {
                var oldTrack = context.Tracks.Find(id);
                if (oldTrack != null)
                {
                    int newGenreId = context.Genres.Where(g => g.Name == track.Genre).SingleOrDefault().GenreId;
                    oldTrack.Name = track.Name;
                    oldTrack.GenreId = newGenreId;
                    oldTrack.Milliseconds = track.Milliseconds;
                    oldTrack.UnitPrice = track.UnitPrice;

                    try
                    {
                        context.SaveChanges();
                        responseMessage = Request.CreateResponse<TrackDTO>(HttpStatusCode.OK, track);
                    }
                    catch (Exception ex)
                    {
                        responseMessage = Request.CreateErrorResponse(HttpStatusCode.BadRequest,
                            new HttpError("Track wasn't updated!"));
                    }
                }
                else {
                    responseMessage = Request.CreateErrorResponse(HttpStatusCode.BadRequest,
                            new HttpError("Track wasn't found!"));
                }
            }

            return responseMessage;
        }
    }
}