using System.ComponentModel.DataAnnotations;

namespace Bloggi.Models.ViewModels
{
    public class User
    {
     
        public Guid Id { get; set; }
     
        public string Username { get; set; }
      
        public string EmailAddress { get; set; }

    }
}
