using Default.Application.BusinessServices.StudentService.dtos;
using Default.Application.BusinessServices.StudentService.Queries;
using Default.Application.Interfaces.Repositories;
using MediatR;


namespace Default.Application.BusinessServices.StudentService.Handlers
{
    public class GetStudentByIdHandler : IRequestHandler<GetStudentByIdQuery, StudentDetails>
    {
        private readonly IStudentRepository _studentRepository;

        public GetStudentByIdHandler(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<StudentDetails> Handle(GetStudentByIdQuery query, CancellationToken cancellationToken)
        {
            return await _studentRepository.GetStudentByIdAsync(query.Id);
        }
    }
}
