using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.FileProviders;
using SFA.DAS.PSRService.Application.Domain;
using SFA.DAS.PSRService.Application.Interfaces;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Application.ReportHandlers
{
    public class CreateReportHandler : IRequestHandler<CreateReportRequest,Report>
    {
        private readonly IReportRepository _reportRepository;
        private readonly IMapper _mapper;
        private readonly IFileProvider _fileProvider;

        public CreateReportHandler(IReportRepository reportRepository, IMapper mapper, IFileProvider fileProvider)
        {
            _reportRepository = reportRepository;
            _mapper = mapper;
            _fileProvider = fileProvider;
        }

        public Task<Report> Handle(CreateReportRequest request, CancellationToken cancellationToken)
        {

            if (String.IsNullOrWhiteSpace(request.Period))
                throw new Exception("Period must be supplied");

            if(request.EmployerId == 0)
                throw new Exception("Employee Id must be supplied");


            var reportDto = new ReportDto()
            {
                EmployerId = request.EmployerId,
                Submitted = false,
                Id = Guid.NewGuid(),
                ReportingPeriod = request.Period,
                ReportingData = GetQuestionConfig().Result
            };

            

            reportDto = _reportRepository.Create(reportDto);

            var report = _mapper.Map<Report>(reportDto);

            return Task.FromResult(report);
        }

        private Task<string> GetQuestionConfig()
        {
            var questionsConfig = _fileProvider.GetFileInfo("/QuestionConfig.json");

            using (var jsonContents = questionsConfig.CreateReadStream())
            {
                using (StreamReader sr = new StreamReader(jsonContents))
                {

                    return Task.FromResult(sr.ReadToEnd());
                }
            }
        }
    }
}