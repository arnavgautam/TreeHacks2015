namespace MyFixIt.Persistence
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    using MyFixIt.Logging;

    public class FixItTaskRepository : IFixItTaskRepository, IDisposable
    {
        private MyFixItContext db = new MyFixItContext();
        private ILogger log = null;
        private bool disposed = false;

        public FixItTaskRepository(ILogger logger)
        {
            this.log = logger;
        }

        public async Task<FixItTask> FindTaskByIdAsync(int id)
        {
            FixItTask fixItTask = null;
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                fixItTask = await this.db.FixItTasks.FindAsync(id);
                
                timespan.Stop();
                this.log.TraceApi("SQL Database", "FixItTaskRepository.FindTaskByIdAsync", timespan.Elapsed, "id={0}", id);
            }
            catch (Exception e)
            {
                this.log.Error(e, "Error in FixItTaskRepository.FindTaskByIdAsynx(id={0})", id);
            }

            return fixItTask;
        }

        public async Task<List<FixItTask>> FindOpenTasksByOwnerAsync(string userName)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = await this.db.FixItTasks.Where(t => t.Owner == userName)
                                       .Where(t => t.IsDone == false)
                                       .OrderByDescending(t => t.FixItTaskId)
                                       .ToListAsync();

                timespan.Stop();
                this.log.TraceApi("SQL Database", "FixItTaskRepository.FindTasksByOwnerAsync", timespan.Elapsed, "username={0}", userName);

                return result;
            }
            catch (Exception e)
            {
                this.log.Error(e, "Error in FixItTaskRepository.FindTasksByOwnerAsync(userName={0})", userName);
                return null;
            }
        }

        public async Task<List<FixItTask>> FindTasksByCreatorAsync(string creater)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = await this.db.FixItTasks
                    .Where(t => t.CreatedBy == creater)
                    .OrderByDescending(t => t.FixItTaskId).ToListAsync();

                timespan.Stop();
                this.log.TraceApi("SQL Database", "FixItTaskRepository.FindTasksByOwnerAsync", timespan.Elapsed, "creater={0}", creater);

                return result;
            }
            catch (Exception e)
            {
                this.log.Error(e, "Error in FixItTaskRepository.FindTasksByOwnerAsync(creater={0})", creater);
                return null;
            }
        }

        public async Task CreateAsync(FixItTask taskToAdd)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                this.db.FixItTasks.Add(taskToAdd);
                await this.db.SaveChangesAsync();

                timespan.Stop();
                this.log.TraceApi(
                    "SQL Database",
                    "FixItTaskRepository.CreateAsync",
                    timespan.Elapsed,
                    "taskToAdd={0}",
                    taskToAdd);
            }
            catch (Exception e)
            {
                this.log.Error(e, "Error in FixItTaskRepository.CreateAsync(taskToAdd={0})", taskToAdd);
            }
        }

        public async Task UpdateAsync(FixItTask taskToSave)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                this.db.Entry(taskToSave).State = EntityState.Modified;
                await this.db.SaveChangesAsync();

                timespan.Stop();
                this.log.TraceApi(
                    "SQL Database",
                    "FixItTaskRepository.UpdateAsync",
                    timespan.Elapsed,
                    "taskToSave={0}",
                    taskToSave);
            }
            catch (Exception e)
            {
                this.log.Error(e, "Error in FixItTaskRepository.UpdateAsync(taskToSave={0})", taskToSave);
            }
        }

        public async Task DeleteAsync(int id)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                FixItTask fixittask = await this.db.FixItTasks.FindAsync(id);
                this.db.FixItTasks.Remove(fixittask);
                await this.db.SaveChangesAsync();

                timespan.Stop();
                this.log.TraceApi("SQL Database", "FixItTaskRepository.DeleteAsync", timespan.Elapsed, "id={0}", id);
            }
            catch (Exception e)
            {
                this.log.Error(e, "Error in FixItTaskRepository.DeleteAsync(id={0})", id);
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.db.Dispose();
                }
            }

            this.disposed = true;
        }
    }
}