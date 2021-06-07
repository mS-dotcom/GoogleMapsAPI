using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InterGlobal.Models;
using Newtonsoft.Json;

namespace InterGlobal.Controllers
{
    public class ViewModel
    {
        public IEnumerable<hizmet_noktalari> hizmets { get; set; }

        public IEnumerable<teslimat_noktalari> teslims { get; set; }

        public IEnumerable<iletisim_bilgileri> migros { get; set; }

        public IEnumerable<tCity_District_Street_Town> ililcemahalle { get; set; }
    }

    public class BayiBulController : Controller
    {
        // GET: BayiBul
        [HttpPost]
        public JsonResult konumlar(string iladi)
        {
            igc_bayilerEntities db = new igc_bayilerEntities();
            //db.tCity_District_Street_Town.Where(x => x.il == iladi).Select(m => m.ilce).Distinct().ToList());
            var jsonIlceler = JsonConvert.SerializeObject(db.iletisim_bilgileri.Where(x => x.il == iladi).Select(m => m.ilce).Distinct().ToList());
            return Json(jsonIlceler);
           
        }
        public ActionResult Index()
        {
            igc_bayilerEntities db = new igc_bayilerEntities();
            ViewModel vm = new ViewModel();
            vm.hizmets = db.hizmet_noktalari.Where(x => x.lat != null && x.lng != null).ToList();
            vm.teslims = db.teslimat_noktalari.Where(x => x.lat != null && x.lng != null).ToList();
            vm.ililcemahalle = db.tCity_District_Street_Town.ToList();
            vm.migros = db.iletisim_bilgileri.ToList();
            return View(vm);
        }
        //BOŞ OLAN HİZMET NOKTALARINI EKLEME
        public ActionResult HizmetNoktasiKonumEkle()
        {
            igc_bayilerEntities db = new igc_bayilerEntities();
            hizmet_noktalari nokta = new hizmet_noktalari();
            nokta = db.hizmet_noktalari.Where(x => x.lat == null && x.lng == null).FirstOrDefault();
            try
            {
                ViewBag.info = db.iletisim_bilgileri.FirstOrDefault(x => x.Magaza_No == nokta.id);
            }
            catch (Exception)
            {

            }
            ViewBag.doluveri = 112/100*db.hizmet_noktalari.Where(x => x.lat != null && x.lng != null).ToList().Count;
            return View(nokta);
        }
        [HttpPost]
        public ActionResult HizmetNoktasiKonumEkle(hizmet_noktalari hzmt)
        {
            igc_bayilerEntities db = new igc_bayilerEntities();
            hizmet_noktalari nokta = new hizmet_noktalari();
            nokta = db.hizmet_noktalari.Where(x => x.lat == null && x.lng == null).FirstOrDefault();
            nokta.lat = hzmt.lat;
            nokta.lng = hzmt.lng;
            db.SaveChanges();
            return RedirectToAction("HizmetNoktasiKonumEkle");
        }
        //BOŞ OLAN TESLİM NOKTALARINI EKLE
        public ActionResult TeslimatNoktasiKonumEkle()
        {
            ViewModel vm = new ViewModel();
            igc_bayilerEntities db = new igc_bayilerEntities();
            teslimat_noktalari nokta = new teslimat_noktalari();
            vm.teslims = db.teslimat_noktalari.Where(x => x.lat == null && x.lng == null).ToList();
            vm.migros = db.iletisim_bilgileri.ToList();
            try
            {
                int magazaid = vm.teslims.FirstOrDefault().Magaza_id;
                ViewBag.info = db.iletisim_bilgileri.FirstOrDefault(x => x.Magaza_No == magazaid);
            }
            catch (Exception e)
            {

            }
            ViewBag.doluveri = 112 / 100 * db.hizmet_noktalari.Where(x => x.lat != null && x.lng != null).ToList().Count;
            return View(vm);
        }
        [HttpPost]
        public ActionResult TeslimatNoktasiKonumEkle(teslimat_noktalari tslm)
        {
            igc_bayilerEntities db = new igc_bayilerEntities();
            teslimat_noktalari nokta = new teslimat_noktalari();   
            nokta = db.teslimat_noktalari.Where(x => x.lat == null && x.lng == null).FirstOrDefault();
            nokta.lat = tslm.lat;
            nokta.lng = tslm.lng;
            db.SaveChanges();
            return RedirectToAction("TeslimatNoktasiKonumEkle");
        }
    }
}