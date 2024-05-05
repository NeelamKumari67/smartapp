using System.ComponentModel.DataAnnotations;

namespace CrudAppliction.Models
{
    public class CustomerTB
    {
        [Key]
       
        public int CustomerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string state { get; set; }
        public string Phoneno { get; set; }
        public string Email { get; set; }
        public string Pincode { get; set; }
    }
}
