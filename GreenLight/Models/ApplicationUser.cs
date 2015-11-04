using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Greenlight.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace GreenLight.Models
{
    // ApplicationUser 클래스에 더 많은 속성을 추가하여 사용자에 대한 프로필 데이터를 추가할 수 있습니다. 자세히 알아보려면 http://go.microsoft.com/fwlink/?LinkID=317594를 방문하십시오.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // authenticationType은 CookieAuthenticationOptions.AuthenticationType에 정의된 항목과 일치해야 합니다.
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // 여기에 사용자 지정 사용자 클레임 추가
            return userIdentity;
        }

        [InverseProperty("PostedBy")]
        public List<Post> Posts { get; set; }

        [InverseProperty("CreatedBy")]
        public List<Comment> Comments { get; set; }

        [InverseProperty("Voter")]
        public List<Vote> Votes { get; set; }
    }
}