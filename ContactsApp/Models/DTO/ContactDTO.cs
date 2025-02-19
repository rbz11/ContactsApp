using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ContactsApp.Models
{
    // Data Transfer Object (DTO) representing a contact
    public class ContactDTO
    {
        public int ContactId { get; set; } // Unique identifier for each contact

        public string first_name { get; set; } // Stores the contact's first name

        public string last_name { get; set; } // Stores the contact's last name

        public string email { get; set; } // Stores the contact's email address

        public string phone { get; set; } // Stores the contact's phone number
    }
}
