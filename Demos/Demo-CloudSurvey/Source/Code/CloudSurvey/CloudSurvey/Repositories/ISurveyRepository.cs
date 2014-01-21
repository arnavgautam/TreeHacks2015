namespace CloudSurvey.Repositories
{
    using System;
    using System.Collections.Generic;
    using CloudSurvey.Models;

    public interface ISurveyRepository : IDisposable
    {
        Survey Get(Guid surveyId);

        Survey GetBySlug(string surveySlug);

        IEnumerable<Survey> GetAll();

        Survey Insert(Survey survey);

        void Update(Survey survey, ICollection<SurveyQuestion> updatedQuestions);

        void Remove(Guid surveyId);

        IEnumerable<SurveySubmission> GetSurveySubmissions(Guid surveyId);

        SurveySubmission InsertSurveySubmission(SurveySubmission surveySubmission);
    }
}