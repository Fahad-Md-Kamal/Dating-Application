using System;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DatingApplication.Helpers
{
    public static class Extensions
    {
        public static void AddApplicationError(this HttpResponse respon, string message)
        {
            respon.Headers.Add("Application-Error", message);
            respon.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            respon.Headers.Add("Access-Control-Allow-Origin", "*");
        }

        public static void AddPagination(this HttpResponse respon,
                 int currentPage, int itemsPerPage, int totalItems, int totalPages)
        {
            var paginationHeader = new PaginationHeader(currentPage, itemsPerPage, totalItems, totalPages);
            var camelCaseFormatter = new JsonSerializerSettings();
            camelCaseFormatter.ContractResolver = new CamelCasePropertyNamesContractResolver();
            respon.Headers.Add("Pagination", 
                JsonConvert.SerializeObject(paginationHeader, camelCaseFormatter));
            respon.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }


        public static int CalculateAge(this DateTime theDateTime)
        {
            var age = DateTime.Today.Year - theDateTime.Year;
            
            if(theDateTime.AddYears(age) > DateTime.Today)
                age--;

            return age;
        }
    }
}