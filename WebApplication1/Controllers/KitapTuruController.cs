using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Utility;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = UserRoles.Role_Admin)]
    public class KitapTuruController : Controller
    {

        
        private readonly IKitapTuruRepository _kitapTuruRepository;

        public KitapTuruController(IKitapTuruRepository context)
        {
            _kitapTuruRepository = context;
        }
        public IActionResult Index()
        {
            List<KitapTuru> objKitapTuruList = _kitapTuruRepository.GetAll().ToList();
            return View(objKitapTuruList);
        }

        public IActionResult Ekle()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Ekle(KitapTuru kitapTuru)
        {
            if (ModelState.IsValid)
            {
                _kitapTuruRepository.Ekle(kitapTuru);
                _kitapTuruRepository.Kaydet(); //save changes yapılmazsa bilgiler kaydedilmez veri tabanına.
                TempData["basarili"] = "Yeni Kitap Türü Başarıyla Oluşturuldu";
                return RedirectToAction("Index","KitapTuru"); 
            }

            return View();

        }
        
        public IActionResult Guncelle(int? id)
        {
            if (id == 0 || id== null)
            {
                return NotFound();
            }

            KitapTuru? kitapTuruVt = _kitapTuruRepository.Get(u => u.Id==id);
            if (kitapTuruVt== null)
            {
                return NotFound();
            }
            return View(kitapTuruVt);
        }
        [HttpPost]
        public IActionResult Guncelle(KitapTuru kitapTuru)
        {
            if (ModelState.IsValid)
            {
                _kitapTuruRepository.Guncelle(kitapTuru);
                _kitapTuruRepository.Kaydet(); //save changes yapılmazsa bilgiler kaydedilmez veri tabanına.
                TempData["basarili"] = "Kitap Türü Başarıyla Güncellendi";
                return RedirectToAction("Index","KitapTuru"); 
            }

            return View();

        }
        
        public IActionResult Sil(int? id)
        {
            if (id == 0 || id== null)
            {
                return NotFound();
            }

            KitapTuru? kitapTuruVt = _kitapTuruRepository.Get(u => u.Id==id);
            if (kitapTuruVt== null)
            {
                return NotFound();
            }
            return View(kitapTuruVt);
        }
        
        [HttpPost, ActionName("Sil")]
        public IActionResult SilPOST(int? id)
        {
            KitapTuru? kitapTuru = _kitapTuruRepository.Get(u => u.Id == id);
            if (kitapTuru == null)
            {
                return NotFound();
            }

            _kitapTuruRepository.Sil(kitapTuru);
            _kitapTuruRepository.Kaydet();
            TempData["basarili"] = "Kitap Türü Başarıyla Silindi";
            return RedirectToAction("Index", "KitapTuru");
        }
        
        
    }
}
