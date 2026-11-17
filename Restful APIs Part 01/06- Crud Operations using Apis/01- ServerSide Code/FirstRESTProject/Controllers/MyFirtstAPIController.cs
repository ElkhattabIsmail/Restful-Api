using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// swagger as a client side.

namespace FirstRESTProject.Controllers
{
    //[Route("api/[controller]")]   // Auto Do Problems.
    // Pass the class name when the request comes in.
    [Route("api/MyFirstAPI")]  // even u change the class Name the MyFirstAPI still the Name of the api.
    [ApiController]
    public class MyFirtstAPIController : ControllerBase
    {
      
        [HttpGet ("MyName",Name="MyName")]
        public string GetMyName()
        {

            return "My Name is Elkhattab Smail";

        }

        // 
        [HttpGet("YourName",Name ="YourName")]
        public string GetYourName()
        {

            return "Your Name is [??????] ";

        }
 
        // function Name + his Parameters.
        [HttpGet("sum/{Num1}/{Num2}")]
        public int Sum2Numbers(int Num1, int Num2)
        {
            return Num1 + Num2;
        }

        [HttpGet("multi/{Num1}/{Num2}")]
        public int Multi2Numbers(int Num1, int Num2)
        {
            return Num1 * Num2;
        }


    }
}
