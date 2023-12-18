using Default.Application.BusinessServices.StudentService.dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Default.Application.BusinessServices.StudentService.Queries
{
    public class GetStudentListQuery : IRequest<List<StudentDetails>>
    {
    }
}
