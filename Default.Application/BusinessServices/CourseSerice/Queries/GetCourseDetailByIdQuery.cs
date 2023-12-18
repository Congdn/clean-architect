using Default.Application.BusinessServices.CourseSerice.dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Default.Application.BusinessServices.CourseSerice.Queries
{
    public class GetCourseDetailByIdQuery : IRequest<CourseDetailViewModel>
    {
        public Guid Id { get; set; }
    }
}
