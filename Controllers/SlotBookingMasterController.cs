using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DOTNETCOREEXAMPLE.DataContext;
using DOTNETCOREEXAMPLE.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using CCA.Util;
using Newtonsoft.Json;

namespace DOTNETCOREEXAMPLE.Controllers
{
    public class SlotBookingMasterController : Controller
    {
        private readonly ApplicationDBContext _context;

        public SlotBookingMasterController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: SlotBookingMaster
        public async Task<IActionResult> Index()
        {
            HttpContext.Session.GetString("RegistrationTypeId");
            if (HttpContext.Session.GetString("RegistrationTypeId") == "1")
            {
                
                int intUserId = Convert.ToInt32(HttpContext.Session.GetString("UserId"));
                return View(await _context.obj_SLOT_BOOKING_MASTER.Where(m => m.USERID == intUserId).OrderByDescending(m => m.SLOTBOOKINGID).ToListAsync());
            }
            else
            {
                return View(await _context.obj_SLOT_BOOKING_MASTER.OrderByDescending(m => m.SLOTBOOKINGID).ToListAsync());
            }
        }

        // GET: SlotBookingMaster/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (HttpContext.Session.GetString("fullname") == null)
            {

                // TempData["PaymentDone"] = "Payment successfull";
                return RedirectToAction("Details", "SlotBookingMaster", new { id = id });
            }
            else
            {
                var class_SLOT_BOOKING_MASTER = await _context.obj_SLOT_BOOKING_MASTER
                   .FirstOrDefaultAsync(m => m.SLOTBOOKINGID == id);

                var PatientList = _context.obj_PATIENT_MASTER.Where(s => s.PATIENTID == class_SLOT_BOOKING_MASTER.PATIENTID).FirstOrDefault();
                ViewBag.PATIENTNAME = PatientList.PATIENTNAME;
                ViewBag.PATIENTID = PatientList.PATIENTID;
                // ViewBag.PATIENTNAME = PatientList.PATIENTNAME;
                ViewBag.AGE = PatientList.AGE;
                ViewBag.WEIGHT = PatientList.WEIGHT;
                ViewBag.ADDRESS = PatientList.ADDRESS;
                ViewBag.GENDER = PatientList.GENDER;
                ViewBag.ALTERNATEMOBILENUMBER = PatientList.ALTERNATEMOBILENUMBER;
                ViewBag.EMAIL = PatientList.EMAIL;
                ViewBag.SLOTNAME = _context.obj_SLOT_MASTER.Where(s => s.SLOTID == class_SLOT_BOOKING_MASTER.SLOTID).Select(s => s.SLOTNAME).FirstOrDefault();
                ViewBag.SERVICENAME = _context.obj_SERVICE_MASTER.Where(s => s.SERVICEID == class_SLOT_BOOKING_MASTER.SERVICETYPEID).Select(s => s.SERVICENAME).FirstOrDefault();
                ViewBag.SERVICEGROUPNAME = _context.obj_SERVICE_GROUP_MASTER.Where(s => s.SERVICEGROUPID == class_SLOT_BOOKING_MASTER.SERVICEGROUPID).Select(s => s.SERVICEGROUPNAME).FirstOrDefault();
                ViewBag.MyURL = "\\prescriptionfiles\\" + class_SLOT_BOOKING_MASTER.PRESCRIPTIONFILENAME + class_SLOT_BOOKING_MASTER.EXTENSION;
                ViewBag.MyReportURL = "\\reportfiles\\" + class_SLOT_BOOKING_MASTER.REPORTFILENAME + class_SLOT_BOOKING_MASTER.REPORTEXTENSION;
                ViewBag.MyServiceURL = "\\servicefiles\\" + class_SLOT_BOOKING_MASTER.SERVICEFILENAME + class_SLOT_BOOKING_MASTER.SERVICEEXTENSION;
                ViewBag.GetPaymentMethod = GetPaymentMethod(id);
                ViewBag.RedirectURL = "https://xraidigital.com/SlotBookingMaster/Details/" + id;
                ViewBag.OrderID = "XRAI/" + class_SLOT_BOOKING_MASTER.SLOTBOOKINGDATETIME.Month + "/" + id;
                //   TempData["PaymentDone"]="Payment successfull";
                if (id == null)
                {
                    return NotFound();
                }


                if (class_SLOT_BOOKING_MASTER == null)
                {
                    return NotFound();
                }

                return View(class_SLOT_BOOKING_MASTER);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(int id, [Bind("SLOTBOOKINGID,SLOTID,SLOTTYPEID,VISITTYPEID,SLOTBOOKINGDATETIME,SERVICETYPEID,SERVICEPROVIDERID,PHONENUMBER,PATIENTNAME,AGE,WEIGHT,GENDER,ADDRESS,ALTERNATEMOBILENUMBER,EMAIL,CREATEDDATE,SERVICEGROUPID,PAYMENTMETHOD,PRESCRIPTIONFILENAME,EXTENSION,PATIENTID,USERID,SERVICEFILENAME,REPORTFILENAME,SERVICEEXTENSION,REPORTEXTENSION")] Class_SLOT_BOOKING_MASTER class_SLOT_BOOKING_MASTER)
        {
         //   return View();
            var queryParameter = new CCACrypto();
            TempData["PaymentSuccessFull"] = "Payment Successful";
            return View("CcAvenue", new CcAvenueViewModel(queryParameter.Encrypt(BuildCcAvenueRequestParameters(id.ToString(), "1"), "E78BC2C9FDDD81E137CB8D0A5137F1F4"), "AVMC08ID14CF41CMFC", "https://secure.ccavenue.com/transaction.do?command=initiateTransaction"));
            //return View("CcAvenue", new CcAvenueViewModel(queryParameter.Encrypt(BuildCcAvenueRequestParameters("123456","1"), "E78BC2C9FDDD81E137CB8D0A5137F1F4"), "AVMC08ID14CF41CMFC", "https://test.ccavenue.com/transaction/transaction.do?command=initiateTransaction"));
        }

            // GET: SlotBookingMaster/Create
            public IActionResult Create(int id, int UserId)
        {
            ViewBag.UserId = HttpContext.Session.GetString("UserId");
           
            var phonenumber = _context.obj_PROJECT_USER_MASTER.Where(m => m.useridsno == UserId).Select(m => m.mobile).FirstOrDefault();
            ViewBag.phonenumber = phonenumber; 
            //ViewBag.CountPATIENTNAMESFORPHONENUMBER = db.obj_SLOT_BOOKING_MASTER.Where(s => s.PHONENUMBER == phonenumber).Count();
            //IEnumerable<SelectListItem> PATIENTNAMESFORPHONENUMBER = db.obj_SLOT_BOOKING_MASTER.Where(s=>s.PHONENUMBER==phonenumber).Select(c => new SelectListItem
            //{

            //    Value = c.PATIENTNAME,
            //    Text = c.PATIENTNAME

            //});
            //ViewBag.PATIENTNAMESFORPHONENUMBER = PATIENTNAMESFORPHONENUMBER;
            var PatientList = _context.obj_PATIENT_MASTER.Where(s => s.PATIENTID == id).FirstOrDefault();
            ViewBag.obj = new Class_PATIENT_MASTER() { PATIENTNAME = PatientList.PATIENTNAME };
            ViewBag.PATIENTNAME = PatientList.PATIENTNAME;
            ViewBag.PATIENTID = PatientList.PATIENTID;
            // ViewBag.PATIENTNAME = PatientList.PATIENTNAME;
            ViewBag.AGE = PatientList.AGE;
            ViewBag.WEIGHT = PatientList.WEIGHT;
            ViewBag.ADDRESS = PatientList.ADDRESS;
            ViewBag.GENDER = PatientList.GENDER;
            ViewBag.ALTERNATEMOBILENUMBER = PatientList.ALTERNATEMOBILENUMBER;
            ViewBag.EMAIL = PatientList.EMAIL;

            var selectGenderList = new List<SelectListItem>();
            selectGenderList.Add(new SelectListItem
            {

                Text = "MALE",
                Value = "MALE"

            }
                );
            selectGenderList.Add(new SelectListItem
            {

                Text = "FEMALE",
                Value = "FEMALE"

            }
                );

            ViewBag.GENDERS = selectGenderList;

            var selectPaymentMethod = new List<SelectListItem>();
            selectPaymentMethod.Add(new SelectListItem
            {

                Text = "ONLINE",
                Value = "ONLINE"

            }
                );
            selectPaymentMethod.Add(new SelectListItem
            {

                Text = "HOME PAYMENT",
                Value = "HOME PAYMENT"

            }
                );

            ViewBag.PAYMENTMETHOD = selectPaymentMethod;
            IEnumerable<SelectListItem> SLOTS = _context.obj_SLOT_MASTER.Select(c => new SelectListItem
            {

                Value = c.SLOTID.ToString(),
                Text = c.SLOTNAME

            });
            ViewBag.SLOTS = SLOTS;
            IEnumerable<SelectListItem> VISITTYPES = _context.obj_VISIT_TYPE_MASTER.Select(c => new SelectListItem
            {

                Value = c.VISITTYPEID.ToString(),
                Text = c.VISITTYPENAME

            });
            ViewBag.VISITTYPES = VISITTYPES;
            IEnumerable<SelectListItem> SERVICES = _context.obj_SERVICE_MASTER.Select(c => new SelectListItem
            {

                Value = c.SERVICEID.ToString(),
                Text = c.SERVICENAME

            });
            ViewBag.SERVICES = SERVICES;
            IEnumerable<SelectListItem> GROUPSERVICES = _context.obj_SERVICE_GROUP_MASTER.Select(c => new SelectListItem
            {

                Value = c.SERVICEGROUPID.ToString(),
                Text = c.SERVICEGROUPNAME

            });
            ViewBag.GROUPSERVICES = GROUPSERVICES;
            //IEnumerable<SelectListItem> SLOTTYPES = _context.obj_SLOT_TYPE_MASTER.Select(c => new SelectListItem
            //{

            //    Value = c.SLOTTYPEID.ToString(),
            //    Text = c.SLOTTYPENAME

            //});
            //ViewBag.SLOTTYPES = SLOTTYPES;

            //Fordate slots
            //IEnumerable<SelectListItem> SLOTS1 = db.obj_SLOT_BOOKING_MASTER.Select(c => new SelectListItem
            //{

            //    Value = c.SLOTID.ToString(),
            //    Text = c.SLOTNAME

            //});
            ViewBag.SLOTS = SLOTS;

            //fordate slots
            IEnumerable<SelectListItem> VISITORTYPES = _context.obj_VISIT_TYPE_MASTER.Select(c => new SelectListItem
            {

                Value = c.VISITTYPEID.ToString(),
                Text = c.VISITTYPENAME

            });
            ViewBag.VISITORTYPES = VISITORTYPES;
            return View();
        }

        // POST: SlotBookingMaster/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Class_SLOT_BOOKING_MASTER class_SLOT_BOOKING_MASTER, IFormFile file)
        {

            string filename ;
           // string filenamewithext = "";
            string extension = "";
            if (file == null || file.Length == 0) {
                if (class_SLOT_BOOKING_MASTER.UPLOADORCLICKPHOTO == "CLICKPHOTO")
                {

                    filename = HttpContext.Session.GetString("FileName");
                    //                    (string)TempData["FileName"];
                    extension = HttpContext.Session.GetString("Extension");
                    //(string)TempData["Extension"];
                }
                else
                {
                    filename = "";
                    extension = "";
                }
            }
            else { 
               


            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot//prescriptionfiles", file.FileName);
            //if (path != null)
            //    return Content("path not selected" + path);
            using (var stream = new FileStream(path, FileMode.Create))
            {

                await file.CopyToAsync(stream);
            }
                filename = Path.GetFileNameWithoutExtension(path).ToLowerInvariant();
                extension = Path.GetExtension(path).ToLowerInvariant();
            }
           
            class_SLOT_BOOKING_MASTER.PRESCRIPTIONFILENAME = filename;
            class_SLOT_BOOKING_MASTER.EXTENSION = extension;
            _context.obj_SLOT_BOOKING_MASTER.Add(class_SLOT_BOOKING_MASTER);
            _context.SaveChanges();
            if (class_SLOT_BOOKING_MASTER.PAYMENTMETHOD == "ONLINE")
            {
                TempData["MailSentAfterBooking"] = "You have selected online Please proceed to MAKE PAYMENT";
            }
            else
            {
                TempData["MailSentAfterBooking"] = "Booking is Confirmed";
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "SlotBookingMaster", new { id = class_SLOT_BOOKING_MASTER.SLOTBOOKINGID });
          
           // return View(class_SLOT_BOOKING_MASTER);
        }

        // GET: SlotBookingMaster/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var UserId = _context.obj_SLOT_BOOKING_MASTER.Where(m => m.SLOTBOOKINGID == id).Select(m => m.USERID).FirstOrDefault();
            var PatientId = _context.obj_SLOT_BOOKING_MASTER.Where(m => m.SLOTBOOKINGID == id).Select(m => m.PATIENTID).FirstOrDefault();
            var phonenumber = _context.obj_PROJECT_USER_MASTER.Where(m => m.useridsno == UserId).Select(m => m.mobile).FirstOrDefault();
            ViewBag.phonenumber = phonenumber;
            var PatientList = _context.obj_PATIENT_MASTER.Where(s => s.PATIENTID == PatientId).FirstOrDefault();
            ViewBag.PATIENTNAME = PatientList.PATIENTNAME;
            ViewBag.PATIENTID = PatientList.PATIENTID;
            // ViewBag.PATIENTNAME = PatientList.PATIENTNAME;
            ViewBag.AGE = PatientList.AGE;
            ViewBag.WEIGHT = PatientList.WEIGHT;
            ViewBag.ADDRESS = PatientList.ADDRESS;
            ViewBag.GENDER = PatientList.GENDER;
            ViewBag.ALTERNATEMOBILENUMBER = PatientList.ALTERNATEMOBILENUMBER;
            ViewBag.EMAIL = PatientList.EMAIL;
            var PRESCRIPTIONFILENAME = _context.obj_SLOT_BOOKING_MASTER.Where(m => m.SLOTBOOKINGID == id).Select(m => m.PRESCRIPTIONFILENAME).FirstOrDefault();
            var EXTENSION = _context.obj_SLOT_BOOKING_MASTER.Where(m => m.SLOTBOOKINGID == id).Select(m => m.EXTENSION).FirstOrDefault();

            ViewBag.MyURL = "\\prescriptionfiles\\" + PRESCRIPTIONFILENAME + EXTENSION;
            var selectGenderList = new List<SelectListItem>();
            selectGenderList.Add(new SelectListItem
            {

                Text = "MALE",
                Value = "MALE"

            }
                );
            selectGenderList.Add(new SelectListItem
            {

                Text = "FEMALE",
                Value = "FEMALE"

            }
                );

            ViewBag.GENDERS = selectGenderList;

            var selectPaymentMethod = new List<SelectListItem>();
            selectPaymentMethod.Add(new SelectListItem
            {

                Text = "ONLINE",
                Value = "ONLINE"

            }
                );
            selectPaymentMethod.Add(new SelectListItem
            {

                Text = "HOME PAYMENT",
                Value = "HOME PAYMENT"

            }
                );

            ViewBag.PAYMENTMETHOD = selectPaymentMethod;
            IEnumerable<SelectListItem> SLOTS = _context.obj_SLOT_MASTER.Select(c => new SelectListItem
            {

                Value = c.SLOTID.ToString(),
                Text = c.SLOTNAME

            });
            ViewBag.SLOTS = SLOTS;
            IEnumerable<SelectListItem> VISITTYPES = _context.obj_VISIT_TYPE_MASTER.Select(c => new SelectListItem
            {

                Value = c.VISITTYPEID.ToString(),
                Text = c.VISITTYPENAME

            });
            ViewBag.VISITTYPES = VISITTYPES;
            IEnumerable<SelectListItem> SERVICES = _context.obj_SERVICE_MASTER.Select(c => new SelectListItem
            {

                Value = c.SERVICEID.ToString(),
                Text = c.SERVICENAME

            });
            ViewBag.SERVICES = SERVICES;
            IEnumerable<SelectListItem> GROUPSERVICES = _context.obj_SERVICE_GROUP_MASTER.Select(c => new SelectListItem
            {

                Value = c.SERVICEGROUPID.ToString(),
                Text = c.SERVICEGROUPNAME

            });
            ViewBag.GROUPSERVICES = GROUPSERVICES;
            //IEnumerable<SelectListItem> SLOTTYPES = _context.obj_SLOT_TYPE_MASTER.Select(c => new SelectListItem
            //{

            //    Value = c.SLOTTYPEID.ToString(),
            //    Text = c.SLOTTYPENAME

            //});
            //ViewBag.SLOTTYPES = SLOTTYPES;

            //Fordate slots
            //IEnumerable<SelectListItem> SLOTS1 = db.obj_SLOT_BOOKING_MASTER.Select(c => new SelectListItem
            //{

            //    Value = c.SLOTID.ToString(),
            //    Text = c.SLOTNAME

            //});
            ViewBag.SLOTS = SLOTS;
            
            
            if (id == null)
            {
                return NotFound();
            }

            var class_SLOT_BOOKING_MASTER = await _context.obj_SLOT_BOOKING_MASTER.FindAsync(id);
            if (class_SLOT_BOOKING_MASTER == null)
            {
                return NotFound();
            }
            var PAYMENTMETHOD = _context.obj_SLOT_BOOKING_MASTER.Where(m => m.SLOTBOOKINGID == id).Select(m => m.PAYMENTMETHOD).FirstOrDefault();
            ViewBag.PAYMENTMETHODS = PAYMENTMETHOD;
            ViewBag.SLOTBOOKINGDATETIME = class_SLOT_BOOKING_MASTER.SLOTBOOKINGDATETIME;
            return View(class_SLOT_BOOKING_MASTER);
        }

        // POST: SlotBookingMaster/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SLOTBOOKINGID,SLOTID,SLOTTYPEID,VISITTYPEID,SLOTBOOKINGDATETIME,SERVICETYPEID,SERVICEPROVIDERID,PHONENUMBER,PATIENTNAME,AGE,WEIGHT,GENDER,ADDRESS,ALTERNATEMOBILENUMBER,EMAIL,CREATEDDATE,SERVICEGROUPID,PAYMENTMETHOD,PRESCRIPTIONFILENAME,EXTENSION,PATIENTID,USERID,SERVICEFILENAME,REPORTFILENAME,SERVICEEXTENSION,REPORTEXTENSION")] Class_SLOT_BOOKING_MASTER class_SLOT_BOOKING_MASTER, IFormFile servicefile, IFormFile reportfile)
        {
            if (id != class_SLOT_BOOKING_MASTER.SLOTBOOKINGID)
            {
                return NotFound();
            }
            string servicefilename = "";
            string servicefilenamewithext = "";
            string serviceextension = "";
            string reportfilename = "";
            string reportfilenamewithext = "";
            string reportextension = "";
            if (servicefile == null || servicefile.Length == 0)
                return Content("file not selected");

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot//servicefiles", servicefile.FileName);
            //if (path != "") { return Content(path); }
            using (var stream = new FileStream(path, FileMode.Create))
            {
               
               // await servicefile.CopyToAsync(stream);
            }
            
            servicefilename = Path.GetFileNameWithoutExtension(path).ToLowerInvariant();
            serviceextension = Path.GetExtension(path).ToLowerInvariant();
            if (reportfile == null || reportfile.Length == 0)
                return Content("file not selected");

            var reportpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot//reportfiles", reportfile.FileName);
            using (var stream = new FileStream(reportpath, FileMode.Create))
            {

                await reportfile.CopyToAsync(stream);
            }
            reportfilename = Path.GetFileNameWithoutExtension(reportpath).ToLowerInvariant();
            reportextension = Path.GetExtension(reportpath).ToLowerInvariant();

            class_SLOT_BOOKING_MASTER.SERVICEFILENAME = servicefilename;
            class_SLOT_BOOKING_MASTER.SERVICEEXTENSION = serviceextension;
            class_SLOT_BOOKING_MASTER.REPORTFILENAME = reportfilename;
            class_SLOT_BOOKING_MASTER.REPORTEXTENSION = reportextension;
          //  if (ModelState.IsValid)
            //{
                try
                {
                    _context.Update(class_SLOT_BOOKING_MASTER);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Class_SLOT_BOOKING_MASTERExists(class_SLOT_BOOKING_MASTER.SLOTBOOKINGID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            return RedirectToAction("Details", "SlotBookingMaster", new { id = id });
            //}
            // return View(class_SLOT_BOOKING_MASTER);
        }

        // GET: SlotBookingMaster/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var class_SLOT_BOOKING_MASTER = await _context.obj_SLOT_BOOKING_MASTER
                .FirstOrDefaultAsync(m => m.SLOTBOOKINGID == id);
            if (class_SLOT_BOOKING_MASTER == null)
            {
                return NotFound();
            }

            return View(class_SLOT_BOOKING_MASTER);
        }

        // POST: SlotBookingMaster/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var class_SLOT_BOOKING_MASTER = await _context.obj_SLOT_BOOKING_MASTER.FindAsync(id);
            _context.obj_SLOT_BOOKING_MASTER.Remove(class_SLOT_BOOKING_MASTER);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Class_SLOT_BOOKING_MASTERExists(int id)
        {
            return _context.obj_SLOT_BOOKING_MASTER.Any(e => e.SLOTBOOKINGID == id);
        }

        public string GetSlotName(int SLOTID)
        {
            string SLOTNAME = _context.obj_SLOT_MASTER.Where(s => s.SLOTID == SLOTID).Select(s => s.SLOTNAME).FirstOrDefault();
            return SLOTNAME;

        }
        public string GetPaymentMethod(int? SLOTBOOKINGID)
        {
            string PaymentMethod = _context.obj_SLOT_BOOKING_MASTER.Where(s => s.SLOTBOOKINGID == SLOTBOOKINGID).Select(s => s.PAYMENTMETHOD).FirstOrDefault();
            return PaymentMethod;

        }
        public ActionResult GetPatientDetailsForPatient(string valueOfDropDown)
        {
            Class_SLOT_BOOKING_MASTER class_SLOT_BOOKING_MASTER = new Class_SLOT_BOOKING_MASTER();
            class_SLOT_BOOKING_MASTER = _context.obj_SLOT_BOOKING_MASTER.Where(s => s.PATIENTNAME == valueOfDropDown).FirstOrDefault();
            return Json(new
            {
                success = true,
                value1 = class_SLOT_BOOKING_MASTER.AGE,
                value2 = class_SLOT_BOOKING_MASTER.WEIGHT,
                value3 = class_SLOT_BOOKING_MASTER.GENDER,
                value4 = class_SLOT_BOOKING_MASTER.ADDRESS,
                value5 = class_SLOT_BOOKING_MASTER.ALTERNATEMOBILENUMBER,
                value6 = class_SLOT_BOOKING_MASTER.EMAIL



            });
        }
        private string BuildCcAvenueRequestParameters(string orderId, string amount)
        {
            int intorderid = Convert.ToInt32(orderId);

            var queryParameters = new Dictionary<string, string>
{
{"order_id", orderId},
{"merchant_id", "384342"},
{"amount", amount},
{"currency","INR" },
{"tid",orderId },
{"redirect_url","https://xraidigital.com/SlotBookingMaster/PaymentSuccessful?id=" + intorderid },
{"cancel_url","https://xraidigital.com/SlotBookingMaster/PaymentFailed?id="+ intorderid},
{"request_type","JSON" },
{"response_type","JSON" },
{"version","1.1" }
//add other key value pairs here.
}.Select(item => string.Format("{0}={1}", item.Key, item.Value));
            return string.Join("&", queryParameters);

        }
        public ActionResult ccAvenue()
        {

            return View();
        }
        [HttpPost]
        //public ActionResult PaymentSuccessful(string encResp)
        //{
        //    /*Do not change the encResp.If you change it you will not get any response.
        //    Decode the encResp*/
        //    var decryption = new CCACrypto();
        //    var decryptedParameters = decryption.Decrypt(encResp, "CcAvenueWorkingKey");
        //    /*split the decryptedParameters by & and then by = and save your values*/
        //    return View();
        //}

        public ActionResult PaymentFailed(int? id)
        {
            ViewBag.ID = id;
            return View();
        }
        public ActionResult GetSlotByDate(string dt)
        {// I expect this call to be filtered
         // so I'll leave this to you on how you want this filtered
            DateTime dtSLOTBOOKINGDATETIME = Convert.ToDateTime(dt);
            var projects = _context.obj_SLOT_BOOKING_MASTER.Where(m => m.SLOTBOOKINGDATETIME == dtSLOTBOOKINGDATETIME).Select(s => s.SLOTID).ToList();
            // at this point, projects should already be filtered with "customerId"
            var shifts = _context.obj_SLOT_MASTER.Where(p => !projects.Contains(p.SLOTID)).ToList();
            return Json(new SelectList(shifts, "SLOTID", "SLOTNAME"));


        }
        [HttpGet]
        public JsonResult GetServicesByServiceGroup(int dt)
        {// I expect this call to be filtered
         // so I'll leave this to you on how you want this filtered
         // DateTime dtSLOTBOOKINGDATETIME = Convert.ToDateTime(dt);
            var projects = _context.obj_SERVICE_MASTER.Where(m => m.SERVICEGROUPID == dt).ToList();
            // at this point, projects should already be filtered with "customerId"
            //  var shifts = db.obj_SLOT_MASTER.Where(p => !projects.Contains(p.SLOTID)).ToList();
            return Json(new SelectList(projects, "SERVICEID", "SERVICENAME"));


        }
        
        public ContentResult SaveCapture(string id)
        {
            string fileName = DateTime.Now.ToString("dd-MM-yy hh-mm-ss");
            if (id == "") { }
            else
            {
                //Convert Base64 Encoded string to Byte Array.
                byte[] imageBytes = Convert.FromBase64String(id.Split(',')[1]);


                //Save the Byte Array as Image File.
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot//prescriptionfiles", fileName);
              //  string filePath = Server.MapPath(string.Format("~/prescriptionfiles/{0}.jpg", fileName));
                 System.IO.File.WriteAllBytes(filePath, imageBytes);
                HttpContext.Session.SetString("FileName", fileName);
                HttpContext.Session.SetString("Extension", ".jpg");
               // TempData["FileName"] = fileName;
                //TempData["Extension"] = ".jpg";
            }
            return Content("true");
        }

        public ActionResult CaptureImage()
        {
            return View();
        }
        
        public ActionResult PaymentSuccessful(int? id)
        {
              var class_SLOT_BOOKING_MASTER = new Class_SLOT_BOOKING_MASTER();
            //  int SlotBookingid = Convert.ToInt32(encResp);
            int? SlotBookingid = id;
            class_SLOT_BOOKING_MASTER = _context.obj_SLOT_BOOKING_MASTER.Where(m => m.SLOTBOOKINGID == SlotBookingid).FirstOrDefault();
            class_SLOT_BOOKING_MASTER.PAYMENTSTATUS = "Payment Successful";
            _context.SaveChanges();
            /*Do not change the encResp.If you change it you will not get any response.
            Decode the encResp*/
            //var decryption = new CCACrypto();
            //var decryptedParameters = decryption.Decrypt(encResp,
            //"your-working-key-from-web.config");

            /*split the decryptedParameters by & and then by = and save your values*/
            ViewBag.ID = id;
            return View();
           // return RedirectToAction("Details", "SlotBookingMaster", new { id = SlotBookingid });
        }
        [HttpPost]
        public IActionResult IndexView()
        {
            HttpContext.Session.GetString("RegistrationTypeId");
            
            if (HttpContext.Session.GetString("RegistrationTypeId") == "1")
            {

                int intUserId = Convert.ToInt32(HttpContext.Session.GetString("UserId"));
               // recordsTotal = _context.obj_SLOT_BOOKING_MASTER.Where(m => m.USERID == intUserId).OrderByDescending(m => m.SLOTBOOKINGID).Count();
                var customerData = _context.obj_SLOT_BOOKING_MASTER.Where(m => m.USERID == intUserId).OrderByDescending(m => m.SLOTBOOKINGID).ToListAsync();

                //  return View(_context.obj_SLOT_BOOKING_MASTER.Where(m => m.USERID == intUserId).OrderByDescending(m => m.SLOTBOOKINGID).ToListAsync());
                return Json(JsonConvert.SerializeObject(customerData));
            }
            else
            {
                //  recordsTotal = _context.obj_SLOT_BOOKING_MASTER.OrderByDescending(m => m.SLOTBOOKINGID).Count();
                List<Class_SLOT_MASTER> customerData = (from customer in _context.obj_SLOT_MASTER
                                            select customer).ToList();
               // List<Class_SLOT_BOOKING_MASTER> customerData = _context.obj_SLOT_BOOKING_MASTER.OrderByDescending(m => m.SLOTBOOKINGID).ToList();
                //var data = customerData.Skip(skip).Take(pageSize).ToList();
                // return View(_context.obj_SLOT_BOOKING_MASTER.OrderByDescending(m => m.SLOTBOOKINGID).ToListAsync());
                return Json(JsonConvert.SerializeObject(customerData));
            }

        }
    }
}
