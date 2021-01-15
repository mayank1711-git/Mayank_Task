using Mayank_Task.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace Mayank_Task.Services
{
  public class DataRepository
  {
    string masterFileName = @"~/Data Source/MasterData.js";
    string userFileName = @"~/Data Source/Users.xml";

    public string GetDesignations()
    {
      string designation = string.Empty;
      try
      {
        foreach (var line in File.ReadLines(System.Web.HttpContext.Current.Server.MapPath(masterFileName)))
        {
          if (line.Contains("Designation"))
          {
            designation = line.Substring(line.IndexOf("= ") + 2, (line.Length - (line.IndexOf("= ") + 2)));
            break;
          }
        }
      }
      catch (Exception e) { }
      return designation.Trim();
    }

    public string GetSkillSets()
    {
      string skill = string.Empty;
      try
      {
        foreach (var line in File.ReadLines(System.Web.HttpContext.Current.Server.MapPath(masterFileName)))
        {
          if (line.Contains("SkillSet"))
          {
            skill = line.Substring(line.IndexOf("= ") + 2, (line.Length - (line.IndexOf("= ") + 2)));
            break;
          }
        }
      }
      catch (Exception e) { }
      return skill.Trim();
    }

    public void SaveUserData(RegisterViewModel model)
    {
      try
      {
        XmlDocument oXmlDocument = new XmlDocument();
        oXmlDocument.Load(System.Web.HttpContext.Current.Server.MapPath(userFileName));
        XmlNodeList nodelist = oXmlDocument.GetElementsByTagName("Users");
        var x = oXmlDocument.GetElementsByTagName("Id");
        int Max = 0;
        foreach (XmlElement item in x)
        {
          int EId = Convert.ToInt32(item.InnerText.ToString());
          if (EId > Max)
          {
            Max = EId;
          }
        }
        Max = Max + 1;
        XDocument xmlDoc = XDocument.Load(System.Web.HttpContext.Current.Server.MapPath(userFileName));
        xmlDoc.Element("Users").Add(new XElement("User", new XElement("Id", Max), new XElement("Name", model.Name), new XElement("Email", model.Email), new XElement("Designation", model.Designation), new XElement("Skills", model.Skills)));
        xmlDoc.Save(System.Web.HttpContext.Current.Server.MapPath(userFileName));  
      }
      catch (Exception e) { }
    }

    public List<RegisterViewModel> GetUserDetail(string userName)
    {
      List<RegisterViewModel> userDetail = null;
      try
      {
        XDocument oXmlDocument = XDocument.Load(System.Web.HttpContext.Current.Server.MapPath(userFileName));
        var items = (from item in oXmlDocument.Descendants("User")
                     where Convert.ToString(item.Element("Email").Value) == userName
                     select new RegisterViewModel
                     {
                       Name = item.Element("Name").Value,
                       Id = Convert.ToInt32(item.Element("Id").Value),
                       Email = item.Element("Email").Value,
                       Designation = item.Element("Designation").Value,
                       Skills = item.Element("Skills").Value,
                     }).SingleOrDefault();
        if (items != null)
        {
          var designationData = JsonConvert.DeserializeObject<List<DesignationModel>>(GetDesignations());
          items.DesignationValue = designationData.Where(d => d.id == Convert.ToInt32(items.Designation)).Select(d => d.name).FirstOrDefault().ToString();
          var skillData = JsonConvert.DeserializeObject<List<DesignationModel>>(GetSkillSets());
          var selectedSkills = skillData.Where(d => items.Skills.Contains(d.id.ToString()));
          items.SkillValue = string.Empty;
          foreach(var skill in selectedSkills)
          {
            items.SkillValue = String.Concat(items.SkillValue, items.SkillValue != string.Empty ? ", " : "", skill.name);
          }
          userDetail = new List<RegisterViewModel>();
          userDetail.Add(items);
        }  
      }
      catch(Exception e){}
      return userDetail;
    }

    public void UpdateUserDetail(RegisterViewModel model)
    {
      try
      {
        XDocument xmlDoc = XDocument.Load(System.Web.HttpContext.Current.Server.MapPath(userFileName));
        var items = (from item in xmlDoc.Descendants("User") select item).ToList();
        XElement selected = items.Where(p => p.Element("Email").Value == model.Email).FirstOrDefault();
        int id = Convert.ToInt32(selected.Element("Id").Value);
        selected.Remove();
        xmlDoc.Save(System.Web.HttpContext.Current.Server.MapPath(userFileName));
        xmlDoc.Element("Users").Add(new XElement("User", new XElement("Id", id), new XElement("Name", model.Name), new XElement("Email", model.Email), new XElement("Designation", model.Designation), new XElement("Skills", model.Skills)));
        xmlDoc.Save(System.Web.HttpContext.Current.Server.MapPath(userFileName));  
      }
      catch (Exception e) { }
    }

  }
}