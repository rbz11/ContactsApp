using ContactsApp.Models;
using ContactsApp.Models.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ContactsApp.Controllers
{
    // Main controller for managing contacts in the application
    public class ContactController : Controller
    {
        private ContactDAO contactDao = new ContactDAO();

        // GET: Displays all contacts by retrieving them from the DAO
        public ActionResult Index()
        {
            return View(contactDao.ReadContacts());
        }

        // GET:  Searches contacts by email. If email is empty, returns all contacts
        public ActionResult Search(string email)
        {
            List<ContactDTO> contacts = string.IsNullOrEmpty(email)
                ? contactDao.ReadContacts() // No search term: return all contacts
                : contactDao.ReadContactsByEmail(email); // Filter by email if provided

            return View("Index", contacts); // Render the "Index" view with search results
        }

        // GET: Details - Displays details of a specific contact by ID
        public ActionResult Details(int id)
        {
            return View(contactDao.ReadContactById(id));
        }

        // GET: Displays the view for adding a new contact
        public ActionResult Create()
        {
            return View();
        }

        // POST: Adds a new contact to the database
        [HttpPost]
        public ActionResult Create(ContactDTO contact)
        {
            try
            {
                if (ModelState.IsValid) // Checks if the form data is valid
                {
                    string res = contactDao.CreateContact(contact); // Attempt to create the contact
                    if (res == "Success") return RedirectToAction("Index"); // Redirect to Index if successful
                    else ModelState.AddModelError("", "Failed to create contact");
                }

                return View(contact); // Reload the form with entered data if validation fails
            }
            catch
            {
                return View(contact); // Show the form again if an exception occurs
            }
        }

        // GET: Delete - Confirms deletion by displaying the contact's details
        public ActionResult Delete(int id)
        {
            ContactDTO contact = contactDao.ReadContactById(id); // Fetch contact by ID

            if (contact == null)
            {
                return HttpNotFound(); // Display 404 if contact does not exist
            }

            return View(contact); // Pass contact to the view to confirm deletion
        }

        // POST: DeleteConfirmed - Deletes a contact after confirmation
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            string result = contactDao.DeleteContact(id); // Delete the contact

            if (result == "Success")
            {
                return RedirectToAction("Index"); // Redirect to Index after deletion
            }

            return RedirectToAction("Details", new { id }); // Redirect to Details if deletion fails
        }
    }
}

