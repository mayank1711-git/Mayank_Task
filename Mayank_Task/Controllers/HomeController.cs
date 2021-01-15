using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Mayank_Task.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Mayank_Task.Services;

namespace Mayank_Task.Controllers
{
  public class HomeController : Controller
  {
    public DataRepository ds = null;

    public HomeController()
    {
      ds = new DataRepository();
    }

    public ActionResult Index()
    {
      return View();
    }

    public ActionResult About()
    {
      ViewBag.Message = "Your application description page.";

      return View();
    }

    public ActionResult Contact()
    {
      ViewBag.Message = "Your contact page.";

      return View();
    }

    public ActionResult GetUserDetail([DataSourceRequest]DataSourceRequest request)
    {
      List<RegisterViewModel> model = ds.GetUserDetail(User.Identity.Name);
      return Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
    }

    public ActionResult UpdateUserDetails([DataSourceRequest] DataSourceRequest dsRequest, RegisterViewModel model)
    {
      ds.UpdateUserDetail(model);
      return Json(ModelState.ToDataSourceResult());
    }

    public JsonResult GetDesignations()
    {
      string json = ds.GetDesignations();
      var data = JsonConvert.DeserializeObject<List<DesignationModel>>(json);
      return Json(data, JsonRequestBehavior.AllowGet);
    }

    public JsonResult GetSkillSets()
    {
      string json = ds.GetSkillSets();
      var data = JsonConvert.DeserializeObject<List<DesignationModel>>(json);
      return Json(data, JsonRequestBehavior.AllowGet);
    }

  }
}