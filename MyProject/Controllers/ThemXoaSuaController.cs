using MyProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyProject.Controllers {
    public class ThemXoaSuaController : Controller {
        //
        // GET: /ThemXoaSua/
        public ActionResult ThemXoaSua() {
            return View();
        }
        QLBanQuanAoDataContext db = new QLBanQuanAoDataContext();
        
        public ActionResult ThemSanPham(string tenSP, string moTa, string gioiTinh, decimal? giaBan, decimal? giaNhap, string anh, int? maLoaiSP, int? maNCC, int? soLuongTon) {
            if (Session["Admin"] == null) {
                return RedirectToAction("DangNhap", "DangNhap");
            }
            SanPham sanPham = new SanPham();
            if (sanPham == null) {
                return HttpNotFound();
            }
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams, "MaloaiSP", "TenLoaiSP");
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps, "MaNCC", "TenNCC");
            if (tenSP != "" && moTa != "" && gioiTinh != "" && giaBan != null && giaNhap != null && anh != "" && maLoaiSP != null && maNCC != null && soLuongTon != null) {
                sanPham.TenSP = tenSP;
                sanPham.MoTa = moTa;
                sanPham.GioiTinh = gioiTinh;
                sanPham.GiaBan = giaBan;
                sanPham.GiaNhap = giaNhap;
                sanPham.Anh = anh;
                sanPham.MaLoaiSP = maLoaiSP;
                sanPham.MaNCC = maNCC;
                if (soLuongTon <= 0) {
                    sanPham.SoLuongTon = 1;
                }
                else {
                    sanPham.SoLuongTon = soLuongTon;
                }
                db.SanPhams.InsertOnSubmit(sanPham);
                db.SubmitChanges();
                return View(sanPham);
            }
            db.SubmitChanges();
            return View();
        }

        public ActionResult XoaSanPham(int maSP) {
            if (Session["Admin"] == null) {
                return RedirectToAction("DangNhap", "DangNhap");
            }
            SanPham sanPham = db.SanPhams.Single(ma => ma.MaSP == maSP);
            if (sanPham == null) {
                return HttpNotFound();
            }
            db.SanPhams.DeleteOnSubmit(sanPham);
            db.SubmitChanges();
            return RedirectToAction("DanhMucCacSanPham", "Admin");
        }

        public ActionResult ChiTietSanPham(int maSP) {
            if (Session["Admin"] == null) {
                return RedirectToAction("DangNhap", "DangNhap");
            }
            SanPham sanPham = db.SanPhams.Single(ma => ma.MaSP == maSP);
            if (sanPham == null) {
                return HttpNotFound();
            }
            else {
                return View(sanPham);
            }
        }
        [ValidateInput(false)] // Cho phép nhập đoạn mã html vào csdl. Nhập đoạn mã html ở thẻ input nào thì khi binding ra giao diện nhớ ghi @Html.Raw(data hiển thị). VD: Xem ở view chi tiết chỗ @Html.Raw(Model.MoTa).
            // Do mô tả chứa đoạn mã html ( <br />) nên phải sử dụng cú pháp razor mvc @Html.Raw(). Đọc đoạn mã html
        public ActionResult SuaSanPham(int maSP, string tenSP, string moTa, string gioiTinh, decimal? giaBan, decimal? giaNhap, string anh, int? maLoaiSP, int? maNCC, int? soLuongTon) {
            if (Session["Admin"] == null) {
                return RedirectToAction("DangNhap", "DangNhap");
            }
            SanPham sanPham = db.SanPhams.Single(ma => ma.MaSP == maSP);
            if (sanPham == null) {
                return HttpNotFound();
            }
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams, "MaloaiSP", "TenLoaiSP");
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps, "MaNCC", "TenNCC");
            if (tenSP != "" && moTa != "" && gioiTinh != "" && giaBan != null && giaNhap != null && anh != "" && maLoaiSP != null && maNCC != null && soLuongTon != null) {
                sanPham.TenSP = tenSP;
                sanPham.MoTa = moTa;
                sanPham.GioiTinh = gioiTinh;
                sanPham.GiaBan = giaBan;
                sanPham.GiaNhap = giaNhap;
                sanPham.Anh = anh;
                sanPham.MaLoaiSP = maLoaiSP;
                sanPham.MaNCC = maNCC;
                if (soLuongTon <= 0) {
                    sanPham.SoLuongTon = 1;
                }
                else {
                    sanPham.SoLuongTon = soLuongTon;
                }
                db.SubmitChanges();
                return View(sanPham);
            }
            return View();
        }

        public ActionResult timKiemSanPham(string tenSP) {
            if (Session["Admin"] == null) {
                return RedirectToAction("DangNhap", "DangNhap");
            }
            if (!string.IsNullOrEmpty(tenSP)) {
                var query = from sp in db.SanPhams where sp.TenSP.Contains(tenSP) || sp.LoaiSanPham.TenLoaiSP.Contains(tenSP) select sp;
                if (query.Count() == 0) {
                    return RedirectToAction("thongBaoRong", "ThemXoaSua");
                }
                return View(query);
            }
            return View();
        }

        public ActionResult thongBaoRong() {
            if (Session["Admin"] == null) {
                return RedirectToAction("DangNhap", "DangNhap");
            }
            ViewBag.stringEmpty = "Không tìm thấy sản phẩm";
            return View();
        }

        public ActionResult QuanLiDonHang() {
            if (Session["Admin"] == null) {
                return RedirectToAction("DangNhap", "User");
            }
            var loadData = db.ChiTietHoaDons;
            return View(loadData);
        }
        [HttpPost]
        public ActionResult DuyetDonHang(int maHD) {
            HoaDon hd = db.HoaDons.SingleOrDefault(n => n.MaHD.Equals(maHD));
            hd.TinhTrang = true;
            db.SubmitChanges();
            return RedirectToAction("QuanLiDonHang", "ThemXoaSua");
        }

        [HttpPost]
        public ActionResult HuyDH(int maHD) {
            HoaDon hd = db.HoaDons.SingleOrDefault(n => n.MaHD.Equals(maHD));
            db.HoaDons.DeleteOnSubmit(hd);
            db.SubmitChanges();
            return RedirectToAction("QuanLiDonHang", "ThemXoaSua");
        }

        public ActionResult QuanLiKhachHang() {
            ViewBag.GetList = from a in db.HoaDons
                              join b in db.KhachHangs
                              on a.MaKH equals b.MaKH
                              select new HDKhachHangModel {
                                  MaKH = b.MaKH,
                                  TenKH = b.TenKH,
                                  TaiKhoan = b.TaiKhoan,
                                  MatKhau = b.MatKhau,
                                  SoDienThoai = b.SDT,
                                  MaHD = a.MaHD,
                                  TinhTrang = (bool)a.TinhTrang,
                              };
            return View(ViewBag.GetList);
        }
        [HttpPost]
        public ActionResult XoaTaiKhoan(int maKH) {
            KhachHang kh = db.KhachHangs.SingleOrDefault(n => n.MaKH.Equals(maKH));
            db.KhachHangs.DeleteOnSubmit(kh);
            db.SubmitChanges();
            return RedirectToAction("QuanLiKhachHang", "ThemXoaSua");
        }
    }
}
