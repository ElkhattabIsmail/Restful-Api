
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text;
using System.Threading.Tasks;
//using Newtonsoft.Json; In .netFrameWork for serialization and diserialization library.

// Client Side

namespace StudentApiClient
{
    class Program
    {
        static readonly HttpClient httpClient = new HttpClient(); // Now we are in the client Side so we need
        // the httpClient for deals with the server.

        static async Task Main(string[] args)
        {
            // Set this to the correct URI for your API (End Point).
            httpClient.BaseAddress = new Uri("http://localhost:5215/api/Students/All");

            await GetAllStudents();

            await GetPassedStudents();

            await GetTop1StudentsGrade();



        }
        static async Task GetTop1StudentsGrade()
        {
            try
            {
                Console.WriteLine("\n_____________________________");
                Console.WriteLine("\nFetching all students...\n");
                int? TopStudentGrade = await httpClient.GetFromJsonAsync<int>("Top1");

                if (TopStudentGrade.HasValue)
                    Console.WriteLine($"Top Grade In Class : {TopStudentGrade}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        static async Task GetAllStudents()
        {
            try
            {
                Console.WriteLine("\n_____________________________");
                Console.WriteLine("\nFetching all students...\n");
                var students = await httpClient.GetFromJsonAsync<List<Student>>("All");
                if (students != null)
                {
                    foreach (var student in students)
                    {
                        Console.WriteLine($"ID: {student.Id}, Name: {student.Name}, Age: {student.Age}, Age: {student.Grade}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
        static async Task GetPassedStudents()
        {
            try
            {
                Console.WriteLine("\n_____________________________");
                Console.WriteLine("\nFetching Passed students...\n");
                var students = await httpClient.GetFromJsonAsync<List<Student>>("Passed");
                if (students != null)
                {
                    foreach (var student in students)
                    {
                        Console.WriteLine($"ID: {student.Id}, Name: {student.Name}, Age: {student.Age}, Age: {student.Grade}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }


    }

    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int Grade { get; set; }
    }
}
