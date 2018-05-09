using System;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.FileProviders;
using SFA.DAS.PSRService.Application.Domain;
using SFA.DAS.PSRService.Application.Interfaces;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Application.ReportHandlers
{
    public class CreateReportHandler : RequestHandler<CreateReportRequest,Report>
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

        protected override Report HandleCore(CreateReportRequest request)
        {
            if (String.IsNullOrWhiteSpace(request.Period))
                throw new Exception("Period must be supplied");

            if(string.IsNullOrWhiteSpace(request.EmployerId))
                throw new Exception("Employer Id must be supplied");

            var reportDto = new ReportDto()
            {
                EmployerId = request.EmployerId,
                Submitted = false,
                Id = Guid.NewGuid(),
                ReportingPeriod = request.Period,
                ReportingData = GetQuestionConfig().Result
            };

            _reportRepository.Create(reportDto);

            return _mapper.Map<Report>(reportDto);
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