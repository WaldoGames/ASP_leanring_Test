using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using WebApplication3.Models;
using ConfigurationManager = System.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Xml.Linq;
using Microsoft.AspNetCore.Hosting.Server;
using MySql.Data.MySqlClient;

using DbLayer;
using DBLayer.Models;
using Microsoft.AspNetCore;
using System.Runtime.CompilerServices;
using Microsoft.DotNet.Scaffolding.Shared.Project;

namespace WebApplication3.Controllers
{
    public class TestController : Controller
    {
        public IWebHostEnvironment WebHost;
        public TestController(IWebHostEnvironment webHost)
        {
            WebHost = webHost;
        }

        // GET: testController
        public ActionResult Index()
        {
            
            TestModelCollection models = new TestModelCollection();
            models.ModelList = new List<TestModelView>();
            List<Test> test = new DBContext().testCall1();

            test = test.OrderByDescending(o => o.id).ToList();

            foreach (var test_Obj in test)
            {
                TestModelView model = new TestModelView();
                model.text = test_Obj.text;
                model.disc = test_Obj.disc;
                model.id = test_Obj.id;
                model.ImagePath = test_Obj.imgpath;
                models.ModelList.Add(model);
            }

            return View(models);
        }
    

        // GET: testController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: testController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: testController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TestModelUpload collection)
        {
            try
            {
                DBContext DB = new DBContext();

                string filename = UploadImage(collection);

                DB.TryCreateTest(collection.text, collection.disc, filename);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: testController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: testController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: testController/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                DBContext DB = new DBContext();
                DB.TryDeleteTest(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // POST: testController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                DBContext DB = new DBContext();
                DB.TryDeleteTest(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private string UploadImage(TestModelUpload testModelUpload)
        {
            string fileName = null;

            if (testModelUpload.image != null)
            {
                

                string path = Path.Combine(WebHost.WebRootPath, "Images");
                string Filename = Guid.NewGuid()+testModelUpload.image.FileName;
                string FilePath = Path.Combine(path, Filename);
                fileName = Filename;
                using (var fileSteam = new FileStream(FilePath, FileMode.Create))
                {
                    testModelUpload.image.CopyTo(fileSteam);
                }
            }
            return fileName;
        }
    }
}
