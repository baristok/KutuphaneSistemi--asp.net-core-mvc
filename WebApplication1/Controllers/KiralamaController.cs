using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication1.Models;
using WebApplication1.Utility;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = UserRoles.Role_Admin)]
    public class KiralamaController : Controller
    {
        private readonly IKiralamaRepository _kiralamaRepository;
        private readonly IKitapRepository _kitapRepository;
        public readonly IWebHostEnvironment _webHostEnvironment;

        public KiralamaController(IKiralamaRepository kiralamaRepository, IKitapRepository kitapRepository, IWebHostEnvironment webHostEnvironment)
        {
            _kiralamaRepository = kiralamaRepository;
            _kitapRepository = kitapRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            // List<Kitap> objKitapList = _kitapRepository.GetAll().ToList();
            List<Kiralama> objKiralamaList = _kiralamaRepository.GetAll(includeProps:"Kitap").ToList();
            return View(objKiralamaList);
        }

        public IActionResult EkleGuncelle(int? id)
        {
            IEnumerable<SelectListItem> KitapList = _kitapRepository.GetAll().Select(k => new SelectListItem
            {
                Text = k.KitapAdi,
                Value = k.Id.ToString()
            });
            ViewBag.KitapList = KitapList;
            if (id == 0 || id == null)
            {
                //ekle
                return View();
            }
            else
            {
                //guncelle
                Kiralama? kiralamaVt = _kiralamaRepository.Get(u => u.Id == id);
                if (kiralamaVt == null)
                {
                    return NotFound();
                }

                return View(kiralamaVt);
            }
        }

        [HttpPost]
        public IActionResult EkleGuncelle(Kiralama kiralama)
        {
            if (ModelState.IsValid)
            {
                if (kiralama.Id ==0 )
                {
                    _kiralamaRepository.Ekle(kiralama);
                    TempData["basarili"] = "Yeni Kiralama İşlemi Başarıyla Oluşturuldu";
                }
                else
                {
                    _kiralamaRepository.Guncelle(kiralama);
                    TempData["basarili"] = "Kiralama Güncelleme Başarılı";
                }
                
                _kiralamaRepository.Kaydet(); //save changes yapılmazsa bilgiler kaydedilmez veri tabanına.
                return RedirectToAction("Index", "Kiralama");
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

        public IActionResult Sil(int? id)
        {
            
            IEnumerable<SelectListItem> KitapList = _kitapRepository.GetAll().Select(k => new SelectListItem
            {
                Text = k.KitapAdi,
                Value = k.Id.ToString()
            });
            ViewBag.KitapList = KitapList;
            
            
            if (id == 0 || id == null)
            {
                return NotFound();
            }

            Kiralama? kiralamaVt = _kiralamaRepository.Get(u => u.Id == id);
            if (kiralamaVt == null)
            {
                return NotFound();
            }

            return View(kiralamaVt);
        }

        [HttpPost, ActionName("Sil")]
        public IActionResult SilPOST(int? id)
        {
            Kiralama? kiralama  = _kiralamaRepository.Get(u => u.Id == id);
            if (kiralama == null)
            {
                return NotFound();
            }

            _kiralamaRepository.Sil(kiralama);
            _kiralamaRepository.Kaydet();
            TempData["basarili"] = "Kiralanan Kitap Başarıyla Silindi";
            return RedirectToAction("Index", "Kiralama");
        }
    }
}