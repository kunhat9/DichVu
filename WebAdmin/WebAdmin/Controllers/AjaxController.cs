using CORE.Tables;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using WebAdmin.Helpers;
using WebAdmin.Models;

namespace WebAdmin.Controllers
{
    public class AjaxController : BaseController
    {
        public JsonResult FileUpload(string username)
        {
            AjaxResultModel Result = new AjaxResultModel();

            try
            {
                List<string> filePaths = new List<string>();
                if (Request.Files.Count > 0)
                {
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        filePaths.Add(SaveFile(files[i]));
                    }
                }

                Result.Result = string.Join("#", filePaths);

                Result.Code = 0;
            }
            catch
            {
                Result.Code = 2000;
                Result.Result = "Ajax Error From Server";
            }

            return Json(new JsonResult() { Data = Result });
        }

        private string SaveFile(HttpPostedFileBase file)
        {
            try
            {
                string folderPath = "/UploadedFiles";// Configuration.AppConfigInfo.;
                string filePath = "";

                var fileName = Path.GetFileName(file.FileName);
                var extention = Path.GetExtension(file.FileName);
                var filenamewithoutextension = Path.GetFileNameWithoutExtension(file.FileName);
                if (!folderPath.EndsWith("/"))
                    filePath += "/" + DateTime.Now.ToString("yyyy_MM_dd") + "/";
                else
                    filePath += DateTime.Now.ToString("yyyy_MM_dd") + "/";
                IOHelper.CreateFolder(Server.MapPath(folderPath) + filePath);

                filePath += DateTime.Now.ToString("yyyyMMddHHmmss") + extention;
                file.SaveAs(Server.MapPath(folderPath) + filePath);

                return folderPath + filePath;
            }
            catch (Exception)
            {
                return "";
            }
        }

        #region Upload Photo

        public byte[] GetUploadedPhoto(FileUpload File)
        {
            using (Stream PhotoStream = File.PostedFile.InputStream)
            {
                long photoStreamLength = PhotoStream.Length;
                byte[] photoBytes = new byte[photoStreamLength + 1];
                PhotoStream.Read(photoBytes, 0, (int)photoStreamLength);
                return photoBytes;
            }
        }

        #endregion

        public JsonResult InsertBlog(string blog_name, string blog_content, string blog_type, bool blog_show)
        {
            AjaxResultModel Result = new AjaxResultModel();

            try
            {
                int id = Blogs_Service.Insert(new TB_BLOGS
                {
                    BlogName = blog_name,
                    BlogContent = blog_content,
                    BlogType = blog_type,
                    BlogDateCreate = DateTime.Now,
                    BlogIsShow = blog_show,
                    BlogUserId = UserId
                });

                if (id > 0)
                {
                    if (Request.Files.Count > 0)
                    {
                        for (int i = 0; i < Request.Files.Count; i++)
                        {
                            HttpPostedFileBase file = Request.Files[i];
                            string fileName = Path.GetFileName(file.FileName);
                            string filePath = SaveFile(file);
                            Files_Service.Insert(new TB_FILES
                            {
                                FileOrg = fileName,
                                FilePath = filePath,
                                FileData = "",
                                FileStatus = "A",
                                FileType = "BLOG",
                                FileReferenceId = "" + id
                            });
                        }
                    }

                    Result.Code = 00;
                    Result.Result = "Thành công";
                }
                else
                {
                    Result.Code = 1;
                    Result.Result = "Không thành công";
                }
            }
            catch (Exception Ex)
            {
                Result.Code = 2000;
                Result.Result = "Có lỗi xảy ra. Vui lòng thử lại sau hoặc liên hệ với người quản trị.";
                CORE.Helpers.IOHelper.WriteLog(StartUpPath, IpAddress, "UpdatePassword :", Ex.Message, Ex.ToString());
            }

            return Json(Result);
        }
        public JsonResult UpdateBlog(int blog_id, string blog_name, string blog_content, string blog_type, bool blog_show)
        {
            AjaxResultModel Result = new AjaxResultModel();

            try
            {
                TB_BLOGS blog = Blogs_Service.GetById(blog_id);
                blog.BlogName = blog_name;
                blog.BlogContent = blog_content;
                blog.BlogType = blog_type;
                blog.BlogIsShow = blog_show;
                blog.BlogUserId = UserId;

                if (Blogs_Service.Update(blog))
                {
                    if (Request.Files.Count > 0)
                    {
                        for (int i = 0; i < Request.Files.Count; i++)
                        {
                            HttpPostedFileBase file = Request.Files[i];
                            string fileName = Path.GetFileName(file.FileName);
                            string filePath = SaveFile(file);
                            Files_Service.Insert(new TB_FILES
                            {
                                FileOrg = fileName,
                                FilePath = filePath,
                                FileData = "",
                                FileStatus = "A",
                                FileType = "BLOG",
                                FileReferenceId = "" + blog_id
                            });
                        }
                    }

                    Result.Code = 000;
                    Result.Result = "Thành công";
                }
                else
                {
                    Result.Code = 001;
                    Result.Result = "Không thành công";
                }
            }
            catch (Exception Ex)
            {
                Result.Code = 2000;
                Result.Result = "Có lỗi xảy ra. Vui lòng thử lại sau hoặc liên hệ với người quản trị.";
                CORE.Helpers.IOHelper.WriteLog(StartUpPath, IpAddress, "UpdatePassword :", Ex.Message, Ex.ToString());
            }

            return Json(Result);
        }

        public JsonResult InsertSlider(string slider_name, string slider_content, bool slider_show)
        {
            AjaxResultModel Result = new AjaxResultModel();

            try
            {
                if (Request.Files.Count != 1)
                {
                    Result.Code = 1;
                    Result.Result = "Dữ liệu không hợp lệ. Cần 1 ảnh chạy cho Slider";
                    return Json(Result);
                }

                int id = Sliders_Service.Insert(new TB_SLIDERS
                {
                    SliderName = slider_name,
                    SliderContent = slider_content,
                    SliderDateCreate = DateTime.Now,
                    SliderIsShow = slider_show,
                    SliderUserId = UserId
                });

                if (id > 0)
                {
                    HttpPostedFileBase file = Request.Files[0];
                    string fileName = Path.GetFileName(file.FileName);
                    string filePath = SaveFile(file);
                    if (Files_Service.Insert(new TB_FILES
                    {
                        FileOrg = fileName,
                        FilePath = filePath,
                        FileData = "",
                        FileStatus = "A",
                        FileType = "SLIDER",
                        FileReferenceId = "" + id
                    }))
                    {
                        Result.Code = 00;
                        Result.Result = "Thành công";
                    }
                    else
                    {
                        Result.Code = 1;
                        Result.Result = "Không thành công";
                    }
                }
                else
                {
                    Result.Code = 1;
                    Result.Result = "Không thành công";
                }
            }
            catch (Exception Ex)
            {
                Result.Code = 2000;
                Result.Result = "Có lỗi xảy ra. Vui lòng thử lại sau hoặc liên hệ với người quản trị.";
                CORE.Helpers.IOHelper.WriteLog(StartUpPath, IpAddress, "UpdatePassword :", Ex.Message, Ex.ToString());
            }

            return Json(Result);
        }
        public JsonResult UpdateSlider(int slider_id, string slider_name, string slider_content, bool slider_show)
        {
            AjaxResultModel Result = new AjaxResultModel();

            try
            {
                TB_SLIDERS slider = Sliders_Service.GetById(slider_id);
                slider.SliderName = slider_name;
                slider.SliderContent = slider_content;
                slider.SliderIsShow = slider_show;
                slider.SliderUserId = UserId;

                if (Sliders_Service.Update(slider))
                {
                    if (Request.Files.Count > 0)
                    {
                        for (int i = 0; i < Request.Files.Count; i++)
                        {
                            HttpPostedFileBase file = Request.Files[i];
                            string fileName = Path.GetFileName(file.FileName);
                            string filePath = SaveFile(file);
                            Files_Service.Insert(new TB_FILES
                            {
                                FileOrg = fileName,
                                FilePath = filePath,
                                FileData = "",
                                FileStatus = "A",
                                FileType = "SLIDER",
                                FileReferenceId = "" + slider_id
                            });
                        }
                    }

                    Result.Code = 000;
                    Result.Result = "Thành công";
                }
                else
                {
                    Result.Code = 001;
                    Result.Result = "Không thành công";
                }
            }
            catch (Exception Ex)
            {
                Result.Code = 2000;
                Result.Result = "Có lỗi xảy ra. Vui lòng thử lại sau hoặc liên hệ với người quản trị.";
                CORE.Helpers.IOHelper.WriteLog(StartUpPath, IpAddress, "UpdatePassword :", Ex.Message, Ex.ToString());
            }

            return Json(Result);
        }

        // Loại dịch vụ
        public JsonResult InsertServiceType(string type_code, string type_name, string type_status, string type_type, List<TypeDetail> details)
        {
            AjaxResultModel Result = new AjaxResultModel();

            try
            {
                string xml = "<Root>";
                xml += "<Type>"
                    + "<TypeCode>" + type_code + "</TypeCode>"
                    + "<TypeName>" + type_name + "</TypeName>"
                    + "<TypeStatus>" + type_status + "</TypeStatus>"
                    + "<TypeType>" + type_type + "</TypeType>"
                    + "</Type>";
                xml += "<Rows>";
                foreach (TypeDetail detail in details)
                {
                    xml += "<Detail>"
                    + "<DetailId>" + detail.DetailId + "</DetailId>"
                    + "<DetailName>" + detail.DetailName + "</DetailName>"
                    + "<DetailType>" + detail.DetailType + "</DetailType>"
                    + "</Detail>"; ;
                }
                xml += "</Rows>";
                xml += "</Root>";

                if (Types_Service.Insert(xml))
                {
                    Result.Code = 000;
                    Result.Result = "Thành công";
                }
                else
                {
                    Result.Code = 001;
                    Result.Result = "Không thành công";
                }
            }
            catch (Exception Ex)
            {
                Result.Code = 2000;
                Result.Result = "Có lỗi xảy ra. Vui lòng thử lại sau hoặc liên hệ với người quản trị.";
                CORE.Helpers.IOHelper.WriteLog(StartUpPath, IpAddress, "UpdatePassword :", Ex.Message, Ex.ToString());
            }

            return Json(Result);
        }
        public JsonResult UpdateServiceType(string type_code, string type_name, string type_status, string type_type, List<TypeDetail> details)
        {
            AjaxResultModel Result = new AjaxResultModel();

            try
            {
                string xml = "<Root>";
                xml += "<Type>"
                    + "<TypeCode>" + type_code + "</TypeCode>"
                    + "<TypeName>" + type_name + "</TypeName>"
                    + "<TypeStatus>" + type_status + "</TypeStatus>"
                    + "<TypeType>" + type_type + "</TypeType>"
                    + "</Type>";
                xml += "<Rows>";
                foreach (TypeDetail detail in details)
                {
                    xml += "<Detail>"
                    + "<DetailId>" + detail.DetailId + "</DetailId>"
                    + "<DetailName>" + detail.DetailName + "</DetailName>"
                    + "<DetailType>" + detail.DetailType + "</DetailType>"
                    + "</Detail>"; ;
                }
                xml += "</Rows>";
                xml += "</Root>";

                if (Types_Service.Insert(xml))
                {
                    Result.Code = 000;
                    Result.Result = "Thành công";
                }
                else
                {
                    Result.Code = 001;
                    Result.Result = "Không thành công";
                }
            }
            catch (Exception Ex)
            {
                Result.Code = 2000;
                Result.Result = "Có lỗi xảy ra. Vui lòng thử lại sau hoặc liên hệ với người quản trị.";
                CORE.Helpers.IOHelper.WriteLog(StartUpPath, IpAddress, "UpdatePassword :", Ex.Message, Ex.ToString());
            }

            return Json(Result);
        }

        // Dịch vụ
        public JsonResult InsertService(string service_name, decimal service_price, string service_unit, string service_base, string service_content, string service_status, string service_type, List<TypeDetail> details)
        {
            AjaxResultModel Result = new AjaxResultModel();

            try
            {
                string xml = "<Root>";
                xml += "<Service>"
                    + "<ServiceName>" + service_name + "</ServiceName>"
                    + "<ServicePrice>" + service_price + "</ServicePrice>"
                    + "<ServiceUnit>" + service_unit + "</ServiceUnit>"
                    + "<ServiceBase>" + service_base + "</ServiceBase>"
                    + "<ServiceContent>" + service_content + "</ServiceContent>"
                    + "<ServiceStatus>" + service_status + "</ServiceStatus>"
                    + "<ServiceTypeCode>" + service_type + "</ServiceTypeCode>"
                    + "</Service>";
                xml += "<Rows>";
                foreach (TypeDetail detail in details)
                {
                    xml += "<Detail>"
                    + "<DetailId>" + detail.DetailId + "</DetailId>"
                    + "<DetailName>" + (detail.DetailName ?? "") + "</DetailName>"
                    + "</Detail>"; ;
                }
                xml += "</Rows>";
                xml += "</Root>";

                string id = "";
                if (Services_Service.Insert(xml, out id))
                {
                    if (Request.Files.Count > 0)
                    {
                        HttpPostedFileBase file = Request.Files[0];
                        string fileName = Path.GetFileName(file.FileName);
                        string filePath = SaveFile(file);
                        if (Files_Service.Insert(new TB_FILES
                        {
                            FileOrg = fileName,
                            FilePath = filePath,
                            FileData = "",
                            FileStatus = "A",
                            FileType = "SERVICE",
                            FileReferenceId = id
                        }))
                        {
                            Result.Code = 00;
                            Result.Result = "Thành công";
                        }
                        else
                        {
                            Result.Code = 1;
                            Result.Result = "Không thành công";
                        }
                    }
                    else
                    {
                        Result.Code = 00;
                        Result.Result = "Thành công";
                    }
                }
                else
                {
                    Result.Code = 001;
                    Result.Result = "Không thành công";
                }
            }
            catch (Exception Ex)
            {
                Result.Code = 2000;
                Result.Result = "Có lỗi xảy ra. Vui lòng thử lại sau hoặc liên hệ với người quản trị.";
                CORE.Helpers.IOHelper.WriteLog(StartUpPath, IpAddress, "UpdatePassword :", Ex.Message, Ex.ToString());
            }

            return Json(Result);
        }
        public JsonResult UpdateService(int service_id, string service_name, decimal service_price, string service_unit, string service_base, string service_content, string service_status, string service_type, List<TypeDetail> details)
        {
            AjaxResultModel Result = new AjaxResultModel();

            try
            {
                string xml = "<Root>";
                xml += "<Service>"
                    + "<ServiceId>" + service_id + "</ServiceId>"
                    + "<ServiceName>" + service_name + "</ServiceName>"
                    + "<ServicePrice>" + service_price + "</ServicePrice>"
                    + "<ServiceUnit>" + service_unit + "</ServiceUnit>"
                    + "<ServiceBase>" + service_base + "</ServiceBase>"
                    + "<ServiceContent>" + service_content + "</ServiceContent>"
                    + "<ServiceStatus>" + service_status + "</ServiceStatus>"
                    + "<ServiceTypeCode>" + service_type + "</ServiceTypeCode>"
                    + "</Service>";
                xml += "<Rows>";
                foreach (TypeDetail detail in details)
                {
                    xml += "<Detail>"
                    + "<DetailId>" + detail.DetailId + "</DetailId>"
                    + "<DetailName>" + (detail.DetailName ?? "") + "</DetailName>"
                    + "</Detail>"; ;
                }
                xml += "</Rows>";
                xml += "</Root>";

                string id = "";
                if (Services_Service.Insert(xml, out id))
                {
                    if (Request.Files.Count > 0)
                    {
                        HttpPostedFileBase file = Request.Files[0];
                        string fileName = Path.GetFileName(file.FileName);
                        string filePath = SaveFile(file);
                        if (Files_Service.Insert(new TB_FILES
                        {
                            FileOrg = fileName,
                            FilePath = filePath,
                            FileData = "",
                            FileStatus = "A",
                            FileType = "SERVICE",
                            FileReferenceId = id
                        }))
                        {
                            Result.Code = 00;
                            Result.Result = "Thành công";
                        }
                        else
                        {
                            Result.Code = 1;
                            Result.Result = "Upload file Không thành công";
                        }
                    }
                    else
                    {
                        Result.Code = 00;
                        Result.Result = "Thành công";
                    }
                }
                else
                {
                    Result.Code = 001;
                    Result.Result = "Không thành công";
                }
            }
            catch (Exception Ex)
            {
                Result.Code = 2000;
                Result.Result = "Có lỗi xảy ra. Vui lòng thử lại sau hoặc liên hệ với người quản trị.";
                CORE.Helpers.IOHelper.WriteLog(StartUpPath, IpAddress, "UpdatePassword :", Ex.Message, Ex.ToString());
            }

            return Json(Result);
        }

        // Thực đơn
        public JsonResult InsertMenu(int menu_id, int menu_service_id, string menu_name, int menu_num, decimal menu_price, string menu_status, string menu_note, List<MenuGroup> details)
        {
            AjaxResultModel Result = new AjaxResultModel();

            try
            {
                string xml = "<Root>";
                xml += "<Menu>"
                    + "<MenuId>" + menu_id + "</MenuId>"
                    + "<MenuName>" + menu_name + "</MenuName>"
                    + "<MenuNum>" + menu_num + "</MenuNum>"
                    + "<MenuPrice>" + menu_price + "</MenuPrice>"
                    + "<MenuStatus>" + menu_status + "</MenuStatus>"
                    + "<MenuNote>" + menu_note + "</MenuNote>"
                    + "<MenuServiceId>" + menu_service_id + "</MenuServiceId>"
                    + "</Menu>";
                xml += "<Groups>";
                foreach (MenuGroup group in details)
                {
                    xml += "<Group>"
                        + "<GroupId>" + group.GroupId + "</GroupId>"
                        + "<GroupName>" + (group.GroupName ?? "") + "</GroupName>";
                    xml += "<Rows>";
                    foreach (MenuDetail detail in group.GroupDetails)
                    {
                        xml += "<Detail>"
                            + "<DetailId>" + detail.DetailId + "</DetailId>"
                            + "<DetailName>" + detail.DetailName + "</DetailName>"
                            + "</Detail>";
                    }
                    xml += "</Rows>";
                    xml += "</Group>";

                }
                xml += "</Groups>";
                xml += "</Root>";

                string id = "";
                if (Menus_Service.Insert(xml))
                {
                    Result.Code = 00;
                    Result.Result = "Thành công";
                }
                else
                {
                    Result.Code = 001;
                    Result.Result = "Không thành công";
                }
            }
            catch (Exception Ex)
            {
                Result.Code = 2000;
                Result.Result = "Có lỗi xảy ra. Vui lòng thử lại sau hoặc liên hệ với người quản trị.";
                CORE.Helpers.IOHelper.WriteLog(StartUpPath, IpAddress, "UpdatePassword :", Ex.Message, Ex.ToString());
            }

            return Json(Result);
        }
        public JsonResult UpdateMenu(int menu_id, int menu_service_id, string menu_name, int menu_num, decimal menu_price, string menu_status, string menu_note, List<MenuGroup> details)
        {
            AjaxResultModel Result = new AjaxResultModel();

            try
            {
                string xml = "<Root>";
                xml += "<Menu>"
                    + "<MenuId>" + menu_id + "</MenuId>"
                    + "<MenuName>" + menu_name + "</MenuName>"
                    + "<MenuNum>" + menu_num + "</MenuNum>"
                    + "<MenuPrice>" + menu_price + "</MenuPrice>"
                    + "<MenuStatus>" + menu_status + "</MenuStatus>"
                    + "<MenuNote>" + menu_note + "</MenuNote>"
                    + "<MenuServiceId>" + menu_service_id + "</MenuServiceId>"
                    + "</Menu>";
                xml += "<Groups>";
                if (details == null)
                {
                    Result.Code = 01;
                    Result.Result = "Chưa có Nhóm món ăn";
                    return Json(Result);
                }
                foreach (MenuGroup group in details)
                {
                    xml += "<Group>"
                        + "<GroupId>" + group.GroupId + "</GroupId>"
                        + "<GroupName>" + (group.GroupName ?? "") + "</GroupName>";
                    xml += "<Rows>";
                    if (group.GroupDetails == null)
                    {
                        Result.Code = 01;
                        Result.Result = "Chưa có Món ăn trong Nhóm: " + (group.GroupName ?? "");
                        return Json(Result);
                    }
                    foreach (MenuDetail detail in group.GroupDetails)
                    {
                        xml += "<Detail>"
                            + "<DetailId>" + detail.DetailId + "</DetailId>"
                            + "<DetailName>" + detail.DetailName + "</DetailName>"
                            + "</Detail>";
                    }
                    xml += "</Rows>";
                    xml += "</Group>";
                }
                xml += "</Groups>";
                xml += "</Root>";

                if (Menus_Service.Insert(xml))
                {
                    Result.Code = 00;
                    Result.Result = "Thành công";
                }
                else
                {
                    Result.Code = 001;
                    Result.Result = "Không thành công";
                }
            }
            catch (Exception Ex)
            {
                Result.Code = 2000;
                Result.Result = "Có lỗi xảy ra. Vui lòng thử lại sau hoặc liên hệ với người quản trị.";
                CORE.Helpers.IOHelper.WriteLog(StartUpPath, IpAddress, "UpdatePassword :", Ex.Message, Ex.ToString());
            }

            return Json(Result);
        }

        public JsonResult InsertSupplier(string supplier_code, string supplier_name, string supplier_state, string supplier_note)
        {
            AjaxResultModel Result = new AjaxResultModel();

            try
            {
                if (Suppliers_Service.Insert(new TB_SUPPLIERS
                {
                    SupplierCode = supplier_code,
                    SupplierName = supplier_name,
                    SupplierState = supplier_state,
                    SupplierNote = supplier_note
                }))
                {
                    Result.Code = 000;
                    Result.Result = "Thành công";
                }
                else
                {
                    Result.Code = 001;
                    Result.Result = "Không thành công";
                }
            }
            catch (Exception Ex)
            {
                Result.Code = 2000;
                Result.Result = "Có lỗi xảy ra. Vui lòng thử lại sau hoặc liên hệ với người quản trị.";
                CORE.Helpers.IOHelper.WriteLog(StartUpPath, IpAddress, "UpdatePassword :", Ex.Message, Ex.ToString());
            }

            return Json(Result);
        }
        public JsonResult UpdateSupplier(string supplier_code, string supplier_name, string supplier_state, string supplier_note)
        {
            AjaxResultModel Result = new AjaxResultModel();

            try
            {
                TB_SUPPLIERS t = Suppliers_Service.GetById(supplier_code);
                t.SupplierName = supplier_name;
                t.SupplierState = supplier_state;
                t.SupplierNote = supplier_note;

                if (Suppliers_Service.Update(t))
                {
                    Result.Code = 000;
                    Result.Result = "Thành công";
                }
                else
                {
                    Result.Code = 001;
                    Result.Result = "Không thành công";
                }
            }
            catch (Exception Ex)
            {
                Result.Code = 2000;
                Result.Result = "Có lỗi xảy ra. Vui lòng thử lại sau hoặc liên hệ với người quản trị.";
                CORE.Helpers.IOHelper.WriteLog(StartUpPath, IpAddress, "UpdatePassword :", Ex.Message, Ex.ToString());
            }

            return Json(Result);
        }

        public JsonResult InsertCategory(string category_code, string category_name, string category_state, string category_note)
        {
            AjaxResultModel Result = new AjaxResultModel();

            try
            {
                if (Categories_Service.Insert(new TB_CATEGORIES
                {
                    CategoryCode = category_code,
                    CategoryName = category_name,
                    CategoryState = category_state,
                    CategoryNote = category_note
                }))
                {
                    Result.Code = 000;
                    Result.Result = "Thành công";
                }
                else
                {
                    Result.Code = 001;
                    Result.Result = "Không thành công";
                }
            }
            catch (Exception Ex)
            {
                Result.Code = 2000;
                Result.Result = "Có lỗi xảy ra. Vui lòng thử lại sau hoặc liên hệ với người quản trị.";
                CORE.Helpers.IOHelper.WriteLog(StartUpPath, IpAddress, "UpdatePassword :", Ex.Message, Ex.ToString());
            }

            return Json(Result);
        }
        public JsonResult UpdateCategory(string category_code, string category_name, string category_state, string category_note)
        {
            AjaxResultModel Result = new AjaxResultModel();

            try
            {
                TB_CATEGORIES t = Categories_Service.GetById(category_code);
                t.CategoryName = category_name;
                t.CategoryState = category_state;
                t.CategoryNote = category_note;

                if (Categories_Service.Update(t))
                {
                    Result.Code = 000;
                    Result.Result = "Thành công";
                }
                else
                {
                    Result.Code = 001;
                    Result.Result = "Không thành công";
                }
            }
            catch (Exception Ex)
            {
                Result.Code = 2000;
                Result.Result = "Có lỗi xảy ra. Vui lòng thử lại sau hoặc liên hệ với người quản trị.";
                CORE.Helpers.IOHelper.WriteLog(StartUpPath, IpAddress, "UpdatePassword :", Ex.Message, Ex.ToString());
            }

            return Json(Result);
        }

        public JsonResult InsertProduct(string product_code, string product_name, decimal product_price, string product_state, string product_supplierCode, string product_categoryCode)
        {
            AjaxResultModel Result = new AjaxResultModel();

            try
            {
                if (Products_Service.Insert(new TB_PRODUCTS
                {
                    ProductCode = product_code,
                    ProductName = product_name,
                    ProductPrice = product_price,
                    ProductState = product_state,
                    ProductSupplierCode = product_supplierCode,
                    ProductCategoryCode = product_categoryCode
                }))
                {
                    if (Request.Files.Count > 0)
                    {
                        for (int i = 0; i < Request.Files.Count; i++)
                        {
                            HttpPostedFileBase file = Request.Files[i];
                            string fileName = Path.GetFileName(file.FileName);
                            string filePath = SaveFile(file);
                            Files_Service.Insert(new TB_FILES
                            {
                                FileOrg = fileName,
                                FilePath = filePath,
                                FileData = "",
                                FileStatus = "A",
                                FileType = "PRODUCT",
                                FileReferenceId = product_code
                            });
                        }
                    }

                    Result.Code = 000;
                    Result.Result = "Thành công";
                }
                else
                {
                    Result.Code = 001;
                    Result.Result = "Không thành công";
                }
            }
            catch (Exception Ex)
            {
                Result.Code = 2000;
                Result.Result = "Có lỗi xảy ra. Vui lòng thử lại sau hoặc liên hệ với người quản trị.";
                CORE.Helpers.IOHelper.WriteLog(StartUpPath, IpAddress, "UpdatePassword :", Ex.Message, Ex.ToString());
            }

            return Json(Result);
        }
        public JsonResult UpdateProduct(string product_code, string product_name, decimal product_price, string product_state, string product_supplierCode, string product_categoryCode)
        {
            AjaxResultModel Result = new AjaxResultModel();

            try
            {
                TB_PRODUCTS t = Products_Service.GetById(product_code);
                t.ProductName = product_name;
                t.ProductPrice = product_price;
                t.ProductState = product_state;
                t.ProductSupplierCode = product_supplierCode;
                t.ProductCategoryCode = product_categoryCode;

                if (Products_Service.Update(t))
                {
                    if (Request.Files.Count > 0)
                    {
                        for (int i = 0; i < Request.Files.Count; i++)
                        {
                            HttpPostedFileBase file = Request.Files[i];
                            string fileName = Path.GetFileName(file.FileName);
                            string filePath = SaveFile(file);
                            Files_Service.Insert(new TB_FILES
                            {
                                FileOrg = fileName,
                                FilePath = filePath,
                                FileData = "",
                                FileStatus = "A",
                                FileType = "PRODUCT",
                                FileReferenceId = product_code
                            });
                        }
                    }

                    Result.Code = 000;
                    Result.Result = "Thành công";
                }
                else
                {
                    Result.Code = 001;
                    Result.Result = "Không thành công";
                }
            }
            catch (Exception Ex)
            {
                Result.Code = 2000;
                Result.Result = "Có lỗi xảy ra. Vui lòng thử lại sau hoặc liên hệ với người quản trị.";
                CORE.Helpers.IOHelper.WriteLog(StartUpPath, IpAddress, "UpdatePassword :", Ex.Message, Ex.ToString());
            }

            return Json(Result);
        }

        public JsonResult UpdateRegister(int reg_id, string reg_status)
        {
            AjaxResultModel Result = new AjaxResultModel();

            try
            {
                TB_REGISTERS t = Registers_Service.GetById(reg_id);
                t.RegisterStatus = reg_status;
                if (Registers_Service.UpdateStatus(t.RegisterId,reg_status))
                {
                    Result.Code = 000;
                    Result.Result = "Thành công";
                }
                else
                {
                    Result.Code = 001;
                    Result.Result = "Không thành công";
                }
            }
            catch (Exception Ex)
            {
                Result.Code = 2000;
                Result.Result = "Có lỗi xảy ra. Vui lòng thử lại sau hoặc liên hệ với người quản trị.";
                CORE.Helpers.IOHelper.WriteLog(StartUpPath, IpAddress, "UpdatePassword :", Ex.Message, Ex.ToString());
            }

            return Json(Result);
        }

        public JsonResult UpdateOrder(string ord_id, string ord_status)
        {
            AjaxResultModel Result = new AjaxResultModel();

            try
            {
                TB_ORDERS t = Orders_Service.GetById(ord_id);
                t.OrderState = ord_status;

                if (Orders_Service.Update(t))
                {
                    Result.Code = 000;
                    Result.Result = "Thành công";
                }
                else
                {
                    Result.Code = 001;
                    Result.Result = "Không thành công";
                }
            }
            catch (Exception Ex)
            {
                Result.Code = 2000;
                Result.Result = "Có lỗi xảy ra. Vui lòng thử lại sau hoặc liên hệ với người quản trị.";
                CORE.Helpers.IOHelper.WriteLog(StartUpPath, IpAddress, "UpdatePassword :", Ex.Message, Ex.ToString());
            }

            return Json(Result);
        }

        public JsonResult InsertUser(TB_USERS user)
        {
            AjaxResultModel Result = new AjaxResultModel();

            try
            {

                if (Users_Service.Insert(user))
                {
                    Result.Code = 00;
                    Result.Result = "Thành công";
                }
                else
                {
                    Result.Code = 1;
                    Result.Result = "Thao tác không thành công";
                }
            }
            catch (Exception Ex)
            {
                Result.Code = 2000;
                Result.Result = "Có lỗi xảy ra. Vui lòng thử lại sau hoặc liên hệ với người quản trị.";
                CORE.Helpers.IOHelper.WriteLog(StartUpPath, IpAddress, "UpdatePassword :", Ex.Message, Ex.ToString());
            }

            return Json(Result);
        }
        public JsonResult UpdateUser(TB_USERS user)
        {
            AjaxResultModel Result = new AjaxResultModel();

            try
            {

                if (Users_Service.Update(user))
                {
                    Result.Code = 00;
                    Result.Result = "Thành công";
                }
                else
                {
                    Result.Code = 1;
                    Result.Result = "Thao tác không thành công";
                }
            }
            catch (Exception Ex)
            {
                Result.Code = 2000;
                Result.Result = "Có lỗi xảy ra. Vui lòng thử lại sau hoặc liên hệ với người quản trị.";
                CORE.Helpers.IOHelper.WriteLog(StartUpPath, IpAddress, "UpdatePassword :", Ex.Message, Ex.ToString());
            }

            return Json(Result);
        }

        public JsonResult InsertVoucher(TB_VOUCHERS voucher)
        {
            AjaxResultModel Result = new AjaxResultModel();

            try
            {

                if (Voucher_Service.Insert(voucher))
                {
                    Result.Code = 00;
                    Result.Result = "Thành công";
                }
                else
                {
                    Result.Code = 1;
                    Result.Result = "Thao tác không thành công";
                }
            }
            catch (Exception Ex)
            {
                Result.Code = 2000;
                Result.Result = "Có lỗi xảy ra. Vui lòng thử lại sau hoặc liên hệ với người quản trị.";
                CORE.Helpers.IOHelper.WriteLog(StartUpPath, IpAddress, "UpdatePassword :", Ex.Message, Ex.ToString());
            }

            return Json(Result);
        }
        public JsonResult UpdateVoucher(TB_VOUCHERS voucher)
        {
            AjaxResultModel Result = new AjaxResultModel();

            try
            {

                if (Voucher_Service.Update(voucher))
                {
                    Result.Code = 00;
                    Result.Result = "Thành công";
                }
                else
                {
                    Result.Code = 1;
                    Result.Result = "Thao tác không thành công";
                }
            }
            catch (Exception Ex)
            {
                Result.Code = 2000;
                Result.Result = "Có lỗi xảy ra. Vui lòng thử lại sau hoặc liên hệ với người quản trị.";
                CORE.Helpers.IOHelper.WriteLog(StartUpPath, IpAddress, "UpdatePassword :", Ex.Message, Ex.ToString());
            }

            return Json(Result);
        }
    }

    public class TypeDetail
    {
        public int DetailId { get; set; }
        public string DetailName { get; set; }
        public string DetailType { get; set; }
    }

    public class MenuGroup
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public List<MenuDetail> GroupDetails { get; set; }
    }

    public class MenuDetail
    {
        public int DetailId { get; set; }
        public string DetailName { get; set; }
    }
}