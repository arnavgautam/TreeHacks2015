namespace CloudSurvey
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http.Dependencies;
    using CloudSurvey.Controllers;
    using CloudSurvey.Repositories;
    using CloudSurvey.Services;

    public class SimpleDependencyResolver : IDependencyResolver
    {
        public IDependencyScope BeginScope()
        {
            // This example does not support child scopes, so we simply return 'this'.
            return this;
        }

        public object GetService(Type serviceType)
        {
            var repository = new SurveyRepository();

            if (serviceType == typeof(SurveyController))
            {
                return new SurveyController(repository);
            }
            else if (serviceType == typeof(SurveySubmissionController))
            {
                var submissionSummaryService = new SubmissionSummaryService(repository);
                return new SurveySubmissionController(repository, submissionSummaryService);
            }

            return null;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return new List<object>();
        }

        public void Dispose()
        {
            // When BeginScope returns 'this', the Dispose method must be a no-op.
        }
    }
}