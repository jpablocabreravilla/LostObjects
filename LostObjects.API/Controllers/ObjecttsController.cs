
namespace LostObjects.API.Controllers
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Description;
    using LostObjects.API.Helpers;
    using LostObjects.Common.Models;
    using LostObjects.Domain.Models;

    [Authorize]
    public class ObjecttsController : ApiController
    {
        private DataContext db = new DataContext();

        // GET: api/Objectts
        public IQueryable<Objectt> GetObjectts()
        {
            return this.db.Objectts.OrderBy(o => o.Name);
        }

        // GET: api/Objectts/5
        [ResponseType(typeof(Objectt))]
        public async Task<IHttpActionResult> GetObjectt(int id)
        {
            Objectt objectt = await this.db.Objectts.FindAsync(id);
            if (objectt == null)
            {
                return NotFound();
            }

            return Ok(objectt);
        }

        // PUT: api/Objectts/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutObjectt(int id, Objectt objectt)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != objectt.ObjectId)
            {
                return BadRequest();
            }
            if (objectt.ImageArray != null && objectt.ImageArray.Length > 0)
            {
                var stream = new MemoryStream(objectt.ImageArray);
                var guid = Guid.NewGuid().ToString();
                var file = $"{guid}.jpg";
                var folder = "~/Content/Objects";
                var fullPath = $"{folder}/{file}";
                var response = FilesHelper.UploadPhoto(stream, folder, file);

                if (response)
                {
                    objectt.ImagePath = fullPath;
                }
            }
            this.db.Entry(objectt).State = EntityState.Modified;

            try
            {
                await this.db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ObjecttExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(objectt);
        }

        // POST: api/Objectts
        [ResponseType(typeof(Objectt))]
        public async Task<IHttpActionResult> PostObjectt(Objectt objectt)
        {
            objectt.IsDelivered = false;
            objectt.PublishOn = DateTime.Now.ToLocalTime();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(objectt.ImageArray != null && objectt.ImageArray.Length > 0)
            {
                var stream = new MemoryStream(objectt.ImageArray);
                var guid = Guid.NewGuid().ToString();
                var file = $"{guid}.jpg";
                var folder = "~/Content/Objects";
                var fullPath = $"{folder}/{file}";
                var response = FilesHelper.UploadPhoto(stream, folder, file);

                if (response)
                {
                    objectt.ImagePath = fullPath;
                }
            }

            this.db.Objectts.Add(objectt);
            await this.db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = objectt.ObjectId }, objectt);
        }

        // DELETE: api/Objectts/5
        [ResponseType(typeof(Objectt))]
        public async Task<IHttpActionResult> DeleteObjectt(int id)
        {
            Objectt objectt = await this.db.Objectts.FindAsync(id);
            if (objectt == null)
            {
                return NotFound();
            }

            this.db.Objectts.Remove(objectt);
            await this.db.SaveChangesAsync();

            return Ok(objectt);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ObjecttExists(int id)
        {
            return this.db.Objectts.Count(e => e.ObjectId == id) > 0;
        }
    }
}