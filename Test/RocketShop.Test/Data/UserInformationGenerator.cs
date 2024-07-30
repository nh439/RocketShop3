using RocketShop.Database.Model.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Test.Data
{
    public static class UserInformationGenerator
    {
        public static List<UserInformation> GenerateFakeData()
        {
            var random = new Random();
            var departments = new[] { "HR", "IT", "Finance", "Marketing", "Sales" };
            var positions = new[] { "Manager", "Senior Developer", "Junior Developer", "Analyst", "Intern" };

            return Enumerable.Range(1, 20).Select(index => new UserInformation
            {
                UserId = index.ToString("D4"),
                BrithDay = DateTime.Now.AddYears(-random.Next(20, 50)).AddDays(random.Next(0, 365)),
                ResignDate = random.Next(0, 2) == 0 ? (DateTime?)null : DateTime.Now.AddYears(-random.Next(1, 5)).AddDays(random.Next(0, 365)),
                StartWorkDate = DateTime.Now.AddYears(-random.Next(1, 10)).AddDays(random.Next(0, 365)),
                Department = departments[random.Next(departments.Length)],
                CurrentPosition = positions[random.Next(positions.Length)],
                ManagerId = random.Next(0, 2) == 0 ? null : (index > 1 ? (index - 1).ToString("D4") : null),
                Gender = index % 2 == 0 ? 'F' : 'M'
            }).ToList();
        }
    }
}

