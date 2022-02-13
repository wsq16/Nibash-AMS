using FlatManagement.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlatManagement.Utility
{
    public class SendNotificationMaster
    {

        private readonly FlatDBContext _context;
        private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager;
        public IConfiguration _configuration;
        
        //public SendNotificationMaster()
        //{

        //, Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager, IConfiguration configuration


        //}

        public SendNotificationMaster(FlatDBContext context)
        {
            _context = context;
            //this._userManager = userManager;
            //_configuration = configuration;
        }


        public void SendNotificationType(string type, string roleName, string textBody, string emailSubject)
        {
            var users = (from user in _context.Users
                         join userRole in _context.UserRoles
                         on user.Id equals userRole.UserId
                         join role in _context.Roles
                         on userRole.RoleId equals role.Id
                         where role.Name == roleName
                         select user).ToList();
            BroadCast nB = new BroadCast();
            foreach (var item in users)
            {
                string e_mobile = item.Mobile;
                string e_mail = item.Email;
                if(type== "Email")
                {
                    nB.sendBroadCastMail("Email", e_mail, textBody, emailSubject);
                }else if(type == "SMS")
                {
                    nB.sendBroadCastSMS("SMS", e_mobile, textBody);
                }            
                
            }

        }




        //string textBody = "";
        //string textSubject = "";
        //SendNotification("Committee", textBody, textSubject);
        //SendNotification("Treasurer", textBody, textSubject);
        //OwnerNotification(processVM.FlatNo, textBody, textSubject);

        public void SendNotification(string msgType, string roleName, string textBody, string emailSubject, string attachedFile="")
        {
           var users = (from user in _context.Users
                     join userRole in _context.UserRoles
                     on user.Id equals userRole.UserId
                     join role in _context.Roles
                     on userRole.RoleId equals role.Id
                     select user).ToList();

            if (roleName != "")
            {
                 users = (from user in _context.Users
                             join userRole in _context.UserRoles
                             on user.Id equals userRole.UserId
                             join role in _context.Roles
                             on userRole.RoleId equals role.Id
                             where role.Name == roleName
                             select user).ToList();

            }

            BroadCast nB = new BroadCast();
            
            foreach (var item in users)
            {
                string e_mobile = item.Mobile;
                string e_mail = item.Email;

                if(msgType== "Email")
                {
                    nB.sendBroadCastMail(msgType, e_mail, textBody, emailSubject, attachedFile);
                }
                if (msgType == "SMS")
                {
                    nB.sendBroadCastSMS(msgType, e_mobile, textBody);
                }else if(msgType == "All")
                {
                    nB.sendBroadCastMail("Email", e_mail, textBody, emailSubject, attachedFile);
                    nB.sendBroadCastSMS("SMS", e_mobile, textBody);
                }
            }

        }

       
        public void OwnerNotification(string flatNo, string textBody, string emailSubject, string attachedFile = "")
        {
            var flatVar = _context.Users
                                               .Where(p => p.Flat_No == flatNo)
                                               .Select(p => new { p.Mobile, p.Email, p.Flat_No }).ToList();
            BroadCast nB = new BroadCast();
            foreach (var item in flatVar)
            {
                var e_mail = item.Email;
                var e_mobile = item.Mobile;

                nB.sendBroadCastMail("Email", e_mail, textBody, emailSubject, attachedFile);
                nB.sendBroadCastSMS("SMS", e_mobile, textBody);
            }

        }

        public void FlatOwnerAllNotification(string flatNo, string textBody, string emailSubject)
        {
            var flatVar = _context.Users
                                               .Where(p => p.Tenant == false)
                                               .Select(p => new { p.Mobile, p.Email, p.Flat_No }).ToList();
            BroadCast nB = new BroadCast();
            foreach (var item in flatVar)
            {
                var e_mail = item.Email;
                var e_mobile = item.Mobile;

                nB.sendBroadCastMail("Email", e_mail, textBody, emailSubject);
                nB.sendBroadCastSMS("SMS", e_mobile, textBody);
            }

        }

        public void TenantAllNotification(string flatNo, string textBody, string emailSubject)
        {
            var flatVar = _context.Users
                                               .Where(p => p.Tenant == true)
                                               .Select(p => new { p.Mobile, p.Email, p.Flat_No }).ToList();
            BroadCast nB = new BroadCast();
            foreach (var item in flatVar)
            {
                var e_mail = item.Email;
                var e_mobile = item.Mobile;

                nB.sendBroadCastMail("Email", e_mail, textBody, emailSubject);
                nB.sendBroadCastSMS("SMS", e_mobile, textBody);
            }

        }

    }
}
