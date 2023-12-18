using Default.Application.BusinessServices.StudentService.dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Default.Application.BusinessServices.StudentService.Commands
{
    public class CreateStudentCommand : IRequest<StudentDetails>
    {
        public string StudentName { get; set; }
        public string StudentEmail { get; set; }
        public string StudentAddress { get; set; }
        public int StudentAge { get; set; }

        public CreateStudentCommand(string studentName, string studentEmail, string studentAddress, int studentAge)
        {
            StudentName = studentName;
            StudentEmail = studentEmail;
            StudentAddress = studentAddress;
            StudentAge = studentAge;
        }
    }
}
