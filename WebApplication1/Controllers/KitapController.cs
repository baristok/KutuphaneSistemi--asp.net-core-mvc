using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication1.Models;
using WebApplication1.Utility;

namespace WebApplication1.Controllers
{
    
    public class KitapController : Controller
    {
        
        private readonly IKitapRepository _kitapRepository;
        private readonly IKitapTuruRepository _kitapTuruRepository;
        public readonly IWebHostEnvironment _webHostEnvironment;

        public KitapController(IKitapRepository kitapRepository, IKitapTuruRepository kitapTuruRepository, IWebHostEnvironment webHostEnvironment)
        {
            _kitapRepository = kitapRepository;
            _kitapTuruRepository = kitapTuruRepository;
            _webHostEnvironment = webHostEnvironment;
        }
        [Authorize(Roles = "Admin,Ogrenci")]
        public IActionResult Index()
        {
            // List<Kitap> objKitapList = _kitapRepository.GetAll().ToList();
            List<Kitap> objKitapList = _kitapRepository.GetAll(includeProps:"KitapTuru").ToList();
            return View(objKitapList);
        }
        [Authorize(Roles = UserRoles.Role_Admin)]
        public IActionResult EkleGuncelle(int? id)
        {
            IEnumerable<SelectListItem> KitapTuruList = _kitapTuruRepository.GetAll().Select(k => new SelectListItem
            {
                Text = k.Ad,
                Value = k.Id.ToString()
            });
            ViewBag.KitapTuruList = KitapTuruList;
            if (id == 0 || id == null)
            {
                //ekle
                return View();
            }
            else
            {
                //guncelle
                Kitap? kitapVt = _kitapRepository.Get(u => u.Id == id);
                if (kitapVt == null)
                {
                    return NotFound();
                }

                return View(kitapVt);
            }
        }
        [Authorize(Roles = UserRoles.Role_Admin)]
        [HttpPost]
        public IActionResult EkleGuncelle(Kitap kitap,IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string kitapPath = Path.Combine(wwwRootPath, @"img");

                if (file != null)
                {
                    using (var fileStream = new FileStream(Path.Combine(kitapPath, file.FileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    kitap.ResimURL = @"\img\" + file.FileName;
                }

                if (kitap.Id ==0 )
                {
                    _kitapRepository.Ekle(kitap);
                    TempData["basarili"] = "Yeni Kitap Başarıyla Oluşturuldu";
                }
                else
                {
                    _kitapRepository.Guncelle(kitap);
                    TempData["basarili"] = "Kitap Güncelleme Başarılı";
                }
                
                _kitapRepository.Kaydet(); //save changes yapılmazsa bilgiler kaydedilmez veri tabanına.
                return RedirectToAction("Index", "Kitap");
            }

            return View();
        }

        /*
        public IActionResult Guncelle(int? id)
        {
            if (id == 0 || id == null)
            {
                return NotFound();
            }

            Kitap? kitapVt = _kitapRepository.Get(u => u.Id == id);
            if (kitapVt == null)
            {
                return NotFound();
            }

            return View(kitapVt);
        }*/

        
        // [HttpPost]
        // public IActionResult Guncelle(Kitap kitap)
        // {
        //     if (ModelState.IsValid)
        //     {
        //         _kitapRepository.Guncelle(kitap);
        //         _kitapRepository.Kaydet(); //save changes yapılmazsa bilgiler kaydedilmez veri tabanına.
        //         TempData["basarili"] = "Kitap Başarıyla Güncellendi";
        //         return RedirectToAction("Index", "Kitap");
        //     }
        //
        //     return View();
        // }

        [Authorize(Roles = UserRoles.Role_Admin)]
        public IActionResult Sil(int? id)
        {
            if (id == 0 || id == null)
            {
                return NotFound();
            }

            Kitap? kitapVt = _kitapRepository.Get(u => u.Id == id);
            if (kitapVt == null)
            {
                return NotFound();
            }

            return View(kitapVt);
        }
        [Authorize(Roles = UserRoles.Role_Admin)]
        [HttpPost, ActionName("Sil")]
        public IActionResult SilPOST(int? id)
        {
            Kitap? kitap = _kitapRepository.Get(u => u.Id == id);
            if (kitap == null)
            {
                return NotFound();
            }

            _kitapRepository.Sil(kitap);
            _kitapRepository.Kaydet();
            TempData["basarili"] = "Kitap Başarıyla Silindi";
            return RedirectToAction("Index", "Kitap");
        }
    }
}