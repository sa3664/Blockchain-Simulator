using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using BlockchainSimulator.Models;
using Newtonsoft.Json;

namespace BlockchainSimulator.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
        [HttpPost]
        public ActionResult BlockView()
        {           

            var block2 = new BlockController().Mine();
            var _block = JsonConvert.DeserializeObject<Block>(block2);
            return PartialView("BlockView", _block);
        }

        [HttpPost, ValidateInput(false)]
        public EmptyResult POSTBlockView(string param1)
        {
            return new EmptyResult();
        }
        public ActionResult BlockChainSimulator()
        {
            ViewBag.Title = "Cryptocurrency Blockchain Simulator";
            //var block1=new BlockController().
            var block2 = new BlockController().Mine();
            var _block = JsonConvert.DeserializeObject<Block>(block2);
            //using (var client = new HttpClient())
            //{
            //    client.BaseAddress = new Uri("https://localhost:44319");
            //    //HTTP GET
            //    var responseTask = client.GetAsync("/api/block/mine");

            //    responseTask.Wait();

            //    var result = responseTask.Result;
            //    if (result.IsSuccessStatusCode)
            //    {
            //        // Block block =  JsonConvert.DeserializeObject<Block>(result.Content.ToString());

            //        var block = result.Content.ReadAsAsync<Block>();
            //        block.Wait();
            //        return View(block.Result);
            //    }
            //    else //web api sent error response 
            //    {
            //        //log response status here..


            //        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            //    }
            //}
            ////Block _block = block;
            return View(_block);
        }
    }
}
