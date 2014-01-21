namespace CloudSurvey.Services
{
    using System;
    using System.Collections.Generic;
    using CloudSurvey.Models;

    public interface ISubmissionSummaryService
    {
        Dictionary<Guid, AnswersSummary> GetSubmissionsSummary(Guid surveyId);
    }
}