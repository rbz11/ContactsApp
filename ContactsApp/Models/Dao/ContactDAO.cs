using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MySql.Data.MySqlClient;

namespace ContactsApp.Models.Dao
{
    public class ContactDAO
    {
        // Method to retrieve all contacts from the database
        public List<ContactDTO> ReadContacts()
        {
            List<ContactDTO> contacts = new List<ContactDTO>();

            try
            {
                using (var connection = Cnx.getCnx()) // Get database connection
                {
                    connection.Open();
                    string query = "SELECT * FROM contacts"; // SQL query to fetch all contacts
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        // Populate ContactDTO object from database row
                        ContactDTO contact = new ContactDTO()
                        {
                            ContactId = reader.GetInt32("ContactId"),
                            first_name = reader.GetString("first_name"),
                            last_name = reader.GetString("last_name"),
                            email = reader.GetString("email"),
                            phone = reader.GetString("phone"),
                        };
                        contacts.Add(contact); // Add to the list of contacts
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return contacts;
        }

        // Method to fetch a specific contact by ID
        public ContactDTO ReadContactById(int ContactId)
        {
            ContactDTO contact = new ContactDTO();
            try
            {
                using (var connection = Cnx.getCnx())
                {
                    connection.Open();
                    string query = "SELECT * FROM contacts WHERE ContactId = @ContactId"; // Query for specific contact
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@ContactId", ContactId);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        // Map each field to the contact object
                        contact.ContactId = reader.GetInt32("ContactId");
                        contact.first_name = reader.GetString("first_name");
                        contact.last_name = reader.GetString("last_name");
                        contact.email = reader.GetString("email");
                        contact.phone = reader.GetString("phone");
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return contact;
        }

        // Method to create a new contact in the database
        public string CreateContact(ContactDTO contact)
        {
            string res = "Failed"; // Default result message

            try
            {
                using (var connection = Cnx.getCnx())
                {
                    connection.Open();
                    string query = "INSERT INTO contacts (first_name, last_name, email, phone) Values (@first_name, @last_name, @email, @phone)";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    // Bind parameters to prevent SQL injection
                    cmd.Parameters.AddWithValue("@first_name", contact.first_name);
                    cmd.Parameters.AddWithValue("@last_name", contact.last_name);
                    cmd.Parameters.AddWithValue("@email", contact.email);
                    cmd.Parameters.AddWithValue("@phone", contact.phone);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0) res = "Success"; // Return "Success" if insertion was successful
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return res;
        }

        // Method to search contacts by email using partial matches
        public List<ContactDTO> ReadContactsByEmail(string email)
        {
            List<ContactDTO> contacts = new List<ContactDTO>();

            try
            {
                using (var connection = Cnx.getCnx())
                {
                    connection.Open();
                    string query = "SELECT * FROM contacts WHERE email LIKE @Email"; // Query with LIKE for partial matches
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Email", "%" + email + "%"); // Wildcard search for partial match
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        // Create a new ContactDTO object for each row
                        ContactDTO contact = new ContactDTO()
                        {
                            ContactId = reader.GetInt32("ContactId"),
                            first_name = reader.GetString("first_name"),
                            last_name = reader.GetString("last_name"),
                            email = reader.GetString("email"),
                            phone = reader.GetString("phone"),
                        };
                        contacts.Add(contact);
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return contacts;
        }

        // Method to delete a contact by ID
        public string DeleteContact(int id)
        {
            string res = "Failed"; // Default result message

            try
            {
                using (var connection = Cnx.getCnx())
                {
                    connection.Open();
                    string query = "DELETE FROM contacts WHERE ContactId = @ContactId"; // Delete query by ContactId
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@ContactId", id);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0) res = "Success"; // Return "Success" if deletion was successful

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return res;
        }
    }
}
