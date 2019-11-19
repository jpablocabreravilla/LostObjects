namespace LostObjects.Backend.Models
{
    using System.Web;
    using Common.Models;
    public class ObjectView:Objectt
    {
        public HttpPostedFileBase ImageFile { get; set; }
    }
}