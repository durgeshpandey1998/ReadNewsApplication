using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NewsApp.context;
using NewsApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using MailKit;
using System.Net;
using System.Net.Mail;
using System.Data;

namespace NewsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsAPIController : ControllerBase
    {
        private readonly NewsDBContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;
        public IConfiguration _configuration;
        // private readonly IMailService mailService;
     
        public NewsAPIController(NewsDBContext newsContext, IWebHostEnvironment hostEnvironment, IConfiguration configuration)
        {
            _context = newsContext;
            webHostEnvironment = hostEnvironment;
            _configuration = configuration;
          
            // this.mailService = mailService;
        }

        private string EmailSend(PaymentModel Paymentdata)
        {
            Register querydata = new Register();
            querydata = _context.Registers.FirstOrDefault(x => x.Id == Paymentdata.UserId);
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587);
            smtp.EnableSsl = true;
            smtp.Credentials = new NetworkCredential("abc@gmail.com", "password");

            String from = "abc@gmail.com";
            String to = "reciever@gmail.com";
            String subject = "Payment Successfull! Successfully Got Membership.";
            //string path = HttpContext.Current.Server.MapPath("~/files/sample.html");
            string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images/EmailPage.html");
            string content = System.IO.File.ReadAllText(uploadsFolder);
       

            String messageBody = content;
            messageBody = messageBody.Replace("#Paymentdata.Amount", Paymentdata.Amount.ToString());
            messageBody = messageBody.Replace("#Paymentdata.Validity", Paymentdata.Validity.ToString());
            messageBody = messageBody.Replace("#UserName", querydata.UserName);

            MailMessage message = new MailMessage(from, to, subject, messageBody);

            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
              $"{messageBody} <br> <img src=\"cid:Wedding\">",
              null,
              "text/html"
            );

            LinkedResource LinkedImage = new LinkedResource("./1.jpg");
            LinkedImage.ContentId = "Wedding";

            htmlView.LinkedResources.Add(LinkedImage);
            message.AlternateViews.Add(htmlView);

            try
            {
                smtp.Send(message);
            }
            catch (SmtpException ex)
            {
                return ex.Message ;
            }
            return null;
        }


        private string EmailSendForgotPassword(string Email,string Password,string ConfirmPassword)
        {
            Register querydata = new Register();
            querydata = _context.Registers.FirstOrDefault(x => x.Email == Email);
            var newPassword = DecodeFrom64(querydata.Password);
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587);
            smtp.EnableSsl = true;
            smtp.Credentials = new NetworkCredential("abc@gmail.com", "password");

            String from = "abc@gmail.com";
            String to = querydata.Email;
            String subject = "Forgot Password Successfully.";
            //string path = HttpContext.Current.Server.MapPath("~/files/sample.html");
            string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images/ForgotPasswordEmail.html");
            string content = System.IO.File.ReadAllText(uploadsFolder);


            String messageBody = content;
            messageBody = messageBody.Replace("#NewPassword", newPassword.ToString());
            messageBody = messageBody.Replace("#UserName", querydata.UserName);

            MailMessage message = new MailMessage(from, to, subject, messageBody);

            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
              $"{messageBody} <br> <img src=\"cid:Wedding\">",
              null,
              "text/html"
            );

            LinkedResource LinkedImage = new LinkedResource("./1.jpg");
            LinkedImage.ContentId = "Wedding";

            htmlView.LinkedResources.Add(LinkedImage);
            message.AlternateViews.Add(htmlView);

            try
            {
                smtp.Send(message);
            }
            catch (SmtpException ex)
            {
                return ex.Message;
            }
            return null;
        }


        [HttpPost("EmailSend")]
        public ActionResult EmailSend(string name)
        {
          
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587);
                smtp.EnableSsl = true;
                smtp.Credentials = new NetworkCredential("abc@gmail.com", "abc");

                String from = "abc@gmail.com";
                String to = "abc@gmail.com";
                String subject = "Test Email Send";
                String messageBody = "Hello this is ABc ";
                MailMessage message = new MailMessage(from, to, subject, messageBody);

                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
                  $"{messageBody} <br> <img src=\"cid:Wedding\">",
                  null,
                  "text/html"
                );

                LinkedResource LinkedImage = new LinkedResource("./1.jpg");
                LinkedImage.ContentId = "Wedding";

                htmlView.LinkedResources.Add(LinkedImage);
                message.AlternateViews.Add(htmlView);

                try
                {
                    smtp.Send(message);
                }
                catch (SmtpException ex)
                {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
                return null;
            
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        #region Upload Photo When Add News
        private string UploadedFile(NewsContent NewsData)
        {
            string uniqueFileName = null;

            if (NewsData.urlToImage != null)
            {
                //string pathtest = Path.Combine("C:\\Users\\pdurgesh\\source\\repos\\NewsApp\\", "images");
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                //      //  GetFullPath("C:\\Users\\pdurgesh\\source\\repos\\NewsApp\\images");
                //if (uploadsFolder == "D:\\OnlineBookShoppingAPI\\wwwroot\\images")
                //{
                //    uploadsFolder = "D:\\OnlineBookShopping\\wwwroot\\images";
                //}
                uniqueFileName = Guid.NewGuid().ToString() + "_" + NewsData.urlToImage;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                //  .. / .. / _content / OnlineBookShopping / images /
                // string path = Path.Combine(Server.MapPath("~/Images"), Path.GetFileName(filePath));

                System.IO.File.Copy("C:\\Users\\pdurgesh\\Downloads\\" + NewsData.urlToImage, filePath);

            }

            return uniqueFileName;
        }
        #endregion
        [HttpPost("AddPaymentRecord")]
        public ActionResult  AddPaymentRecord([FromBody] PaymentModel PaymentData)
        {
            var allreadypament = _context.Payments.Where(e => e.UserId == PaymentData.UserId).ToList();
            if (allreadypament.Count == 0)
            {
                _context.Payments.Add(PaymentData);
                _context.SaveChanges();
                Register querydata = new Register();
                querydata = _context.Registers.FirstOrDefault(x => x.Id == PaymentData.UserId);
                querydata.RoleName = "Member";
                if (PaymentData.Amount==10)
                {
                    PaymentData.Validity = DateTime.Now.AddMonths(6);
                }
                else
                {
                    PaymentData.Validity = DateTime.Now.AddMonths(12);
                }
                _context.Registers.Update(querydata);
                EmailSend(PaymentData);
                _context.SaveChanges();
                return StatusCode(StatusCodes.Status200OK);
             
            }
            return StatusCode(StatusCodes.Status500InternalServerError);
        }



        [HttpPost("AddNews")]
        public ActionResult AddNews([FromBody] NewsContent NewsData)
        {
            
               // querydata = _context.Registers.FirstOrDefault(x => x.Id == UpdateData.Id);
                var count = _context.NewsContent.Where(x => x.UserId == NewsData.UserId).Count();

            var paymentdata = _context.Payments.Where(x => x.UserId == NewsData.UserId).FirstOrDefault();
            if (paymentdata!=null)
            {
            if (paymentdata.Status=="COMPLETED")
            {
                if (paymentdata.Status=="COMPLETED" && paymentdata.Validity>=DateTime.Now)
                {
                    string UniqueFileName = UploadedFile(NewsData);
                    NewsData.urlToImage = UniqueFileName;
                    _context.NewsContent.Add(NewsData);
                    _context.SaveChanges();
                    return StatusCode(StatusCodes.Status200OK);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            }
            else
            {
                if(count<=2)
                {
                    string UniqueFileName = UploadedFile(NewsData);
                    NewsData.urlToImage = UniqueFileName;
                    _context.NewsContent.Add(NewsData);
                    _context.SaveChanges();
                    return StatusCode(StatusCodes.Status200OK);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
             
            }
        }
        public class YourType
        {
            public List<NewsContent> NewsData { get; set; }
            public string Mypath { get; set; }
        }
   [HttpGet("GetAllUserWithNewsContent")]
   public object GetAllUserWithNewsContent()
        {
            var paymentlist = (from ng in _context.NewsContent
                               join rg in _context.Registers on ng.UserId equals rg.Id
                               select new
                               {
                                   ng.NewsId,
                                   ng.source,
                                   ng.author,
                                   ng.publishedAt,
                                   ng.title,
                                   ng.description,
                                   ng.urlToImage,
                                   ng.dateTime,
                                   rg.UserName
                               }).ToList();
            var json = JsonConvert.SerializeObject(paymentlist, Formatting.Indented,
              new JsonSerializerSettings()
              {
                  ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
              }
              );
            return json;
        }
        [HttpGet("GetAllNews")]
        public object GetAllNews()
        {
            YourType data = new YourType();
         data.NewsData = _context.NewsContent.ToList();
            var paymentlist = (from ng in _context.NewsContent
                               join rg in _context.Registers on ng.UserId equals rg.Id
                               select new
                               {
                                   ng.NewsId,
                                   ng.source,
                                   ng.author,
                                   ng.publishedAt,
                                   ng.title,
                                   ng.description,
                                   ng.urlToImage,
                                   ng.dateTime,
                                   rg.UserName
                               }).ToList();

        

            // string basePath = Environment.CurrentDirectory;
            // string fullPath = Path.GetFullPath(basePath);
            //var test= Path.GetFullPath(webHostEnvironment.WebRootPath);
            var mytest = "http://localhost:6159/images/";          //Path.Combine(test,"images");
            data.Mypath = mytest;
             var json = JsonConvert.SerializeObject(data, Formatting.Indented,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                }
                );
         
            

            return json;
        }
        [HttpGet("GetAllPaymentList")]
        public object GetAllPaymentList()
        {
           // var paymentlist = _context.Payments.ToList();
            var paymentlist = (from rg in _context.Registers
                            join pt in _context.Payments on rg.Id equals pt.UserId
                            select new
                            {
                                rg.Id,
                                rg.Email,
                                rg.RoleName,
                                rg.UserName,
                                pt.Status,
                                pt.Validity,
                                pt.Amount
                            }).ToList();
            var jsonData = JsonConvert.SerializeObject(paymentlist, Formatting.Indented,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling=Newtonsoft.Json.ReferenceLoopHandling.Ignore
                });
            return jsonData;
        }


        [HttpGet("GetOneNews")]
        public Object GetOneNews(int Newsid)
        {
            YourType newsContent = new YourType();
         //   newsContent.NewsData = _context.NewsContent.Where(e=>e.NewsId==Newsid).ToList();
            var mytest = "http://localhost:6159/images/";          //Path.Combine(test,"images");
            newsContent.Mypath = mytest;
            var json = JsonConvert.SerializeObject(newsContent, Formatting.Indented,
               new JsonSerializerSettings()
               {
                   ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
               }
               );
            return json;
        }

        public class Mytest
        {
            public List<Register> Alldata { get; set; }
            public string UserCountdata { get; set; }
        }
        [HttpGet("GetAllUser")]
        public object GetAllUser()
        {
            Mytest obj = new Mytest();
            var data = _context.Registers.ToList();
            obj.Alldata = data;
            //var UserCountMonthWise = _context.Registers
            //      .FromSqlRaw("Select count(Id) As Id,Registerdate AS Registerdate from Registers group by Registerdate Order by Registerdate desc")
            //      .ToList();

            //  SELECT MAX(DATENAME(MM, Registerdate)) AS RegisterationMonth, COUNT(1) AS "TotalUser. Register"
            // FROM Registers GROUP BY MONTH(Registerdate);

            var UserCountMonthWise = _context.Registers
        .GroupBy(g => new { g.Registerdate.Month })
        .Select(g =>  new{
            Month = g.Key.ToString(),
            Count = g.Count().ToString()
        })
        .ToList();
            obj.UserCountdata = UserCountMonthWise.ToString();
            Object[] ArrayOfObjects = new Object[] { data, UserCountMonthWise };
           
            
            var json = JsonConvert.SerializeObject(ArrayOfObjects, Formatting.Indented,
               new JsonSerializerSettings()
               {
                   ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
               }
               );
            return json;
        }

        [HttpPut("UpdateRole")]
        public ActionResult UpdateRole([FromBody]Register UpdateData)
        {
            try
            {
                Register querydata = new Register();
                querydata = _context.Registers.FirstOrDefault(x => x.Id == UpdateData.Id);
                querydata.RoleName = UpdateData.RoleName;
                _context.Registers.Update(querydata);
                _context.SaveChanges();
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
         
        }
        //this function Convert to Encord your Password
        public static string EncodePasswordToBase64(string password)
        {
            try
            {
                byte[] encData_byte = new byte[password.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }

        //this function Convert to Decord your Password
        public static string DecodeFrom64(string password)
        {
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            System.Text.Decoder utf8Decode = encoder.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(password);
            int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            string result = new String(decoded_char);
            return result;
        }

        //ForgotPassword
        [HttpPost]
        [Route("ForgotPassword")]
        public bool ForgotPassword(string Email, string Password, string ConfirmPassword)
        {
            var userEmail = (from s in _context.Registers
                             where s.Email == Email
                             select s).FirstOrDefault();
            if (userEmail!=null)
            {
                Register querydata = new Register();
                querydata = _context.Registers.FirstOrDefault(x => x.Email == userEmail.Email);
                var newPassword = EncodePasswordToBase64(Password);
                var newConfirmPassword = EncodePasswordToBase64(ConfirmPassword);
                querydata.Password = newPassword;
                querydata.ConfirmPassword = newConfirmPassword;
                _context.Registers.Update(querydata);
                _context.SaveChanges();
                EmailSendForgotPassword(Email,Password,ConfirmPassword);
                return true;
            }
            else
            {
                return false;
            }
        }

        //Check Your Email
        [HttpGet]
        [Route("CheckYourEmail")]
        public object CheckYourEmail(string Email)
        {
            var userEmail = (from s in _context.Registers
                             where s.Email == Email
                             select s).FirstOrDefault();
            if (userEmail != null)
            {
                EmailSendForgotPassword(Email, "123", "123");
                var json = JsonConvert.SerializeObject(userEmail, Formatting.Indented,
               new JsonSerializerSettings()
               {
                   ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
               }
               );
                return json;
            }
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
            //Add Book  
            [HttpPost("Register")]
        public ActionResult Register([FromBody] Register RegisterData)
        {
            try
            {
                 var password=EncodePasswordToBase64(RegisterData.Password);
                var confirmpassword = EncodePasswordToBase64(RegisterData.ConfirmPassword);
                RegisterData.Password=password;
                RegisterData.ConfirmPassword = confirmpassword;
                RegisterData.Registerdate = DateTime.Now;
                var UserNameExist = _context.Registers.Where(e => e.Email == RegisterData.Email).ToList();
                if (UserNameExist.Count<1)
                {
                    _context.Registers.Add(RegisterData);
                    _context.SaveChanges();
                    return StatusCode(StatusCodes.Status200OK);
                }
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
          
        }

        [HttpDelete]
        [Route("DeleteNews")]
        public bool DeleteNews(int Newsid)
        {
            if (Newsid != null)
            {
                var newsid = new NewsContent { NewsId = Newsid };
                _context.Entry(newsid).State = EntityState.Deleted;
                _context.SaveChanges();
                return true;
            }

            return false;
        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] Login model)
        {
           
            var password = EncodePasswordToBase64(model.Password);
            var userData1 = (from s in _context.Registers
                           where s.Email == model.Email
                           select s).FirstOrDefault();
            if (userData1.RoleName=="Admin")
            {
                if (userData1.Password == password)
                {
                    if (userData1.Email != null && userData1.Password != null)
                    {
                        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("thisismysecretkey"));
                        var token = new JwtSecurityToken(
                            issuer: _configuration["JWT:ValidIssuer"],
                            audience: _configuration["JWT:ValidAudience"],
                            expires: DateTime.Now.AddHours(3),
                            // claims: authClaims,
                            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                            );
                        return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo,
                            RoleName = userData1.RoleName,
                            UserId = userData1.Id,
                        });
                    }
                }
            }
            else if (userData1.RoleName == "Visitors")
            {
                if (userData1.Password == password)
                {
                    if (userData1.Email != null && userData1.Password != null)
                    {
                        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("thisismysecretkey"));
                        var token = new JwtSecurityToken(
                            issuer: _configuration["JWT:ValidIssuer"],
                            audience: _configuration["JWT:ValidAudience"],
                            expires: DateTime.Now.AddHours(3),
                            // claims: authClaims,
                            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                            );
                        return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo,
                            RoleName = userData1.RoleName,
                            UserId = userData1.Id,
                        });
                    }
                }
            }
            else
            {
                var userData = (from rg in _context.Registers
                                join pt in _context.Payments on rg.Id equals pt.UserId
                                where pt.Status == "COMPLETED" && rg.Email==model.Email
                                select new
                                {
                                    rg.Id,
                                    rg.Email,
                                    rg.RoleName,
                                    rg.UserName,
                                    rg.Password,
                                    pt.Status,
                                    pt.Validity,
                                    pt.Amount
                                }).FirstOrDefault();
                if (userData.Password == password)
                {
                    if (userData.Email != null && userData.Password != null)
                    {
                        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("thisismysecretkey"));
                        var token = new JwtSecurityToken(
                            issuer: _configuration["JWT:ValidIssuer"],
                            audience: _configuration["JWT:ValidAudience"],
                            expires: DateTime.Now.AddHours(3),
                            // claims: authClaims,
                            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                            );
                        return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo,
                            RoleName = userData.RoleName,
                            UserId = userData.Id,
                            Status = userData.Status,
                            Validity = userData.Validity,

                        });
                    }
                }
       
         
         
            }

            #region Claims Token 
            //if (username != null && password)
            //{
            //    var userRoles = await _userManager.GetRolesAsync(user);

            //    var authClaims = new List<Claim>
            //    {
            //        new Claim(ClaimTypes.Name, user.UserName),
            //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            //    };

            //    foreach (var userRole in userRoles)
            //    {
            //        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            //    }

            //    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("thisismysecretkey"));

            //    var token = new JwtSecurityToken(
            //        issuer: _configuration["JWT:ValidIssuer"],
            //        audience: _configuration["JWT:ValidAudience"],
            //        expires: DateTime.Now.AddHours(3),
            //        claims: authClaims,
            //        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            //        );



            //    return Ok(new
            //    {
            //        token = new JwtSecurityTokenHandler().WriteToken(token),
            //        expiration = token.ValidTo
            //    });
            //}
            #endregion
            return Unauthorized();
        }

        public class FileModel
        {
            public string FileName { get; set; }
            public IFormFile FormFile { get; set; }
        }
        [HttpPost("Post")]
        public ActionResult Post([FromForm] FileModel file)
        {
            try
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", file.FileName);
                using (Stream stream= new FileStream(path,FileMode.Create))
                {
                    file.FormFile.CopyTo(stream);
                 }
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
         
        }
    }
}
