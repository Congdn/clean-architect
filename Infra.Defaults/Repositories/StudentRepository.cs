using Default.Application.BusinessServices.StudentService.dtos;
using Default.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Defaults.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        public Task<StudentDetails> AddStudentAsync(StudentDetails studentDetails)
        {
            //viet code tuong tac voi db tai day
            var result = new StudentDetails();
            return Task.Run(() =>
            {
                return result;
            });
            
        }

        public Task<int> DeleteStudentAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<StudentDetails> GetStudentByIdAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<List<StudentDetails>> GetStudentListAsync()
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateStudentAsync(StudentDetails studentDetails)
        {
            throw new NotImplementedException();
        }
    }
}
