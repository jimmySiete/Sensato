using Newtonsoft.Json;
using PlugginCreation.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace PlugginCreation.Controllers
{
    public class TestController : BaseController
    {
        // GET: Test

        public JsonResult CatchingModel(DateTime sDate, DateTime eDate, string JsonParams)
        {
            //DateTime sDate, DateTime eDate, int filter, int actualPage, bool isAsc, string orderBy, bool literacy PARAMETROS ORIGINALES
            var NewModelDeserialized = JsonConvert.DeserializeObject<JsonParams>(JsonParams);
            decimal totalRecords = db.tb_tableGrid.Count();
            decimal totalPages = 0;
            decimal recordsShowed = NewModelDeserialized.filter * NewModelDeserialized.actualPage;
            if (totalRecords % NewModelDeserialized.filter != 0)
            {
                totalPages = Math.Ceiling(totalRecords / NewModelDeserialized.filter);
            } 
            else {
                totalPages = totalRecords / NewModelDeserialized.filter;
            } //total de paginas
            int initialPage = 1;
            decimal finalPage = totalPages;
            var elementsShowedBefore=0;
            
            //Se llena una lista con los datos que se van a mostrar en la tabla
            List<tbGridModel> dbRecords = db.tb_tableGrid.Select(x => new tbGridModel()
            {
                Bootstrap = x.Bootstrap,
                BootstrapVersion = x.BootstrapVersion,
                DateID = x.DateID,
                StartDate = x.StartDate,
                EndDate = x.EndDate

            }).ToList();

            //Page es el indicador de pagina en la que estamos,cuantos registros se muestran y cuantos ya se mostraron
            List<tbGridModel> PAGE;
            if (NewModelDeserialized.actualPage > 1) {
               PAGE = dbRecords.Skip(NewModelDeserialized.filter * (NewModelDeserialized.actualPage - 1)).Take(NewModelDeserialized.filter).ToList();
            }
            else
            {
               PAGE = dbRecords.Skip(elementsShowedBefore).Take(NewModelDeserialized.filter).ToList();
            }

            //Ordenamiento, si está activada esta función, por columna ordena de manera ascendiente o descendiente la información
            if (NewModelDeserialized.literacy)
            {
                switch (NewModelDeserialized.orderBy)
                {
                    case "IDEmpleado":
                        if (NewModelDeserialized.isAsc)
                        {
                            PAGE = PAGE.OrderBy(x => x.DateID).ToList();
                        } else
                        {
                            PAGE = PAGE.OrderByDescending(x => x.DateID).ToList();
                        }
                        break;

                    case "Nombre":
                        if (NewModelDeserialized.isAsc)
                        {
                            PAGE = PAGE.OrderBy(x => x.StartDate).ToList();
                        } else
                        {
                            PAGE = PAGE.OrderByDescending(x => x.StartDate).ToList();
                        }
                        break;

                    case "Puesto":
                        if (NewModelDeserialized.isAsc)
                        {
                            PAGE = PAGE.OrderBy(x => x.EndDate).ToList();
                        } else
                        {
                            PAGE = PAGE.OrderByDescending(x => x.EndDate).ToList();
                        }
                        break;

                    case "JustBootstrap":
                        if (NewModelDeserialized.isAsc)
                        {
                            PAGE = PAGE.OrderBy(x => x.Bootstrap).ToList();
                        }
                        else
                        {
                            PAGE = PAGE.OrderByDescending(x => x.Bootstrap).ToList();
                        }
                        break;

                    case "VersionBootstrap":
                        if (NewModelDeserialized.isAsc)
                        {
                            PAGE = PAGE.OrderBy(x => x.BootstrapVersion).ToList();
                        }
                        else
                        {
                            PAGE = PAGE.OrderByDescending(x => x.BootstrapVersion).ToList();
                        }
                        break;
                }
            }

            //Lista de listas tipo string
            List<Array> GeneralList = new List<Array>();

            foreach (var item in PAGE)
            {
                List<string> StringList = new List<string>()
                {
                    item.DateID.ToString(),
                    item.StartDate.ToString(),
                    item.EndDate.ToString(),
                    item.Bootstrap.ToString(),
                    item.BootstrapVersion
                };
                GeneralList.Add(StringList.ToArray());
            }

            if (recordsShowed>totalRecords)
            {
                recordsShowed = totalRecords;
            }
            //retorno lista de listas, ese resultado se itera del lado de la vista
            return Json(new { model = GeneralList, items = totalRecords, view = recordsShowed, totalPages = totalPages, firstPage = initialPage, numPage = NewModelDeserialized.actualPage }, JsonRequestBehavior.AllowGet);
        }

        //Si está activada la función de detalles, aqui se escoge la informacion que se va a mostrar 
        public PartialViewResult LoadingDetails(string row)
        {
            var intRow = Convert.ToInt32(row);
            tb_Employees employees = db.tb_Employees.Find(intRow);
            return PartialView(employees);
        }
    }
}