using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebApplication4.DataModel;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class ProductController : ApiController
    {
        testDBEntities db = new testDBEntities();
        [HttpGet]
        public IEnumerable<ProductModel> All()
        {
            return db.Products.Select(x => new ProductModel { Id = x.Id, Name = x.Name, Image = x.Image, Price = x.Price, CategoryId = x.CategoryId, SubCategoryId = x.SubCategoryId,IsActive=x.IsActive });
        }
        [HttpGet]
        public ProductModel ProductById(int id)
        {
            return db.Products.Where(x=>x.Id==id).Select(x => new ProductModel { Id = x.Id, Name = x.Name, Image = x.Image, Price = x.Price, CategoryId = x.CategoryId, SubCategoryId = x.SubCategoryId, IsActive = x.IsActive }).FirstOrDefault();
        }

        [HttpPost]
        public string Add()
        {   
            System.Collections.Specialized.NameValueCollection fc = HttpContext.Current.Request.Form;
            string request = string.Format("{0}", fc["request"]);
            ProductModel model = new ProductModel();
            model = (ProductModel)JsonConvert.DeserializeObject(request, model.GetType());

            
            HttpFileCollection httpFileCollection = HttpContext.Current.Request.Files;
            if (httpFileCollection.Count > 0)
            {
                HttpPostedFile file = httpFileCollection[0];
                if (Path.GetExtension(file.FileName).ToLower() != ".exe")
                {
                    model.Image = SaveMedia(file);                       
                }                
            }
            Product p = new Product();
            if(model.Id>0)
                p= db.Products.Where(x => x.Id == model.Id).FirstOrDefault();
            p.CategoryId = model.CategoryId;
            p.SubCategoryId = model.SubCategoryId;
            p.Name = model.Name;
            p.Price = model.Price;
            p.Image = model.Image;
            p.IsActive = model.IsActive;
            if (model.Id == 0)
                db.Products.Add(p);
            db.SaveChanges();
                return string.Empty;
        }

        public static string SaveMedia(HttpPostedFile file)
        {
            // Verify that the user selected a file
            if (file != null && file.ContentLength > 0)
            {

                // extract only the fielname
                var fileName = Path.GetFileName(file.FileName);
                var fileExt = Path.GetExtension(file.FileName);
                // store the file inside ~/App_Data/uploads folder
                if (!string.IsNullOrEmpty(fileName))
                {
                    fileName = string.Format("{0}{1}", Path.GetRandomFileName(), fileExt);
                    const string folder = "/MediaData";
                    var root = HttpContext.Current.Server.MapPath("~"+folder);
                    var dir = new DirectoryInfo(root);
                    if (!dir.Exists)
                    {
                        dir.Create();
                    }

                    var path = Path.Combine(root, fileName);

                    file.SaveAs(path);

                    return string.Format("{0}/{1}", folder, fileName);
                }
            }
            return "";
        }

    }
}
